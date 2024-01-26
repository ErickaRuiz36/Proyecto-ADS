using ADS.Models;
using ADS.Services.Conexion;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ADS.Services
{
	public interface IRepositorioProyectos
	{
		int ContarProyectos(int criterioBusqueda, string datoBusqueda);
		void EliminarProyecto(int idProyecto);
		bool ExisteProyecto(string Nombre, int NumContrato);
		IEnumerable<Proyecto> ListaProyectos(PaginacionViewModel paginacion);
		void ModificarProyecto(Proyecto proyecto);
		Proyecto ObtenerProyecto(int idProyecto);
		int RegistrarProyecto(Proyecto proyecto);
	}
	public class RepositorioProyectos: IRepositorioProyectos
    {
        private readonly string connectionString;

        public RepositorioProyectos()
        {
            connectionString = Db_Connect.GetConnectionString();
        }

        public int RegistrarProyecto(Proyecto proyecto)
        {
            proyecto.Disponible = true;
            using var connection = new SqlConnection(connectionString);
            var resultado = 0;
            
            if(!ExisteProyecto(proyecto.Nombre, proyecto.NumContrato))
            {
				resultado = connection.QuerySingle<int>(@"insert into Proyecto (Nombre,Descripcion,FechaInicio,FechaEstimadaTermino,RFCContratista,
                NumContrato,FechaContrato,Disponible) values(@Nombre,@Descripcion,@FechaInicio,@FechaEstimadaTermino,@RFCContratista,
                @NumContrato,@FechaContrato,@Disponible); select scope_identity();", proyecto);
			}

            return resultado;
        }

        public bool ExisteProyecto(string Nombre, int NumContrato)
        {
            using var connection = new SqlConnection(connectionString);

            var existe = connection.QueryFirstOrDefault<int>(@"select 1 from Proyecto where 
                (Nombre=@Nombre or NumContrato = @NumContrato) and Disponible = 1;", new { Nombre, NumContrato });

            return existe == 1;
        }

		private string cadenaBusqueda(int criterioBusqueda, string datoBusqueda)
		{
			string busqueda = "";

			if (datoBusqueda != null)
			{
				datoBusqueda = datoBusqueda.Trim();

				if (datoBusqueda != "")
				{
					if (criterioBusqueda == 1)
					{
						busqueda = $"and Nombre like '%{datoBusqueda}%'";
					}
					else if (criterioBusqueda == 2)
					{
						busqueda = $"and Descripcion like '%{datoBusqueda}%'";
					}
					else if (criterioBusqueda == 3)
					{
						busqueda = $"and RFCContratista like '%{datoBusqueda}%'";
					}
					else if (criterioBusqueda == 4)
					{
						busqueda = $"and NumContrato like '%{datoBusqueda}%'";
					}
				
				}
			}

			return busqueda;
		}

		public IEnumerable<Proyecto> ListaProyectos(PaginacionViewModel paginacion)
        {
            using var connection = new SqlConnection(connectionString);
			string criterioOrden = "Nombre"; // Valor predeterminado
			string direccionOrden = "asc"; // Valor predeterminado

			string busqueda = cadenaBusqueda(paginacion.criterioBusqueda, paginacion.datoBusqueda);

			if (paginacion.criterioOrden == 2)
			{
				criterioOrden = "FechaInicio";
			}
			else if (paginacion.criterioOrden == 3)
			{
				criterioOrden = "FechaEstimadaTermino";
			}
			else if (paginacion.criterioOrden == 4)
			{
				criterioOrden = "FechaContrato";
			}

			if (paginacion.direccionOrden == 2)
			{
				direccionOrden = "desc";
			}

			return connection.Query<Proyecto>($@"SELECT *
                                FROM Proyecto
                                WHERE Disponible = 1 {busqueda}
                                ORDER BY {criterioOrden} {direccionOrden}
                                OFFSET {paginacion.registrosSaltar} ROWS
                                FETCH NEXT {paginacion.RegistrosPagina} ROWS ONLY;");

        }

		public int ContarProyectos(int criterioBusqueda, string datoBusqueda)
		{
			string busqueda = cadenaBusqueda(criterioBusqueda, datoBusqueda);
			using var connection = new SqlConnection(connectionString);
			return connection.ExecuteScalar<int>(@$"select count(*) from Proyecto where Disponible = 1 {busqueda}");
		}

		public void ModificarProyecto(Proyecto proyecto)
		{
			using var connection = new SqlConnection(connectionString);
			connection.Execute(@"update Proyecto 
                                set Nombre = @Nombre, Descripcion = @Descripcion,
                                FechaInicio = @FechaInicio,
                                FechaEstimadaTermino = @FechaEstimadaTermino,
                                RFCContratista = @RFCContratista,
                                NumContrato = @NumContrato,
                                FechaContrato = @FechaContrato
                                where idProyecto = @IdProyecto", proyecto);
		}

		public Proyecto ObtenerProyecto(int idProyecto)
		{
			using var connection = new SqlConnection(connectionString);
			var proyecto = connection.QueryFirstOrDefault<Proyecto>(@"select * from Proyecto 
                                            where idProyecto = @idProyecto and Disponible = 1", new { idProyecto });

			var frentesObra = connection.Query<FrenteObra>(@"select * from FrenteObra 
															where idProyecto = @idProyecto 
															and Disponible = 1", new {idProyecto});

			foreach(var frenteObra in frentesObra)
			{
				var nombre = connection.QuerySingle<string>(@"select Nombre from Constructora 
											where idConstructora = @IdConstructora", new {IdConstructora = frenteObra.IdConstructora});
				Constructora constructora = new Constructora();
				constructora.Nombre = nombre.ToString();
				frenteObra.IdConstructoraNavigation = constructora;
				proyecto.FrenteObras.Add(frenteObra);
			}

			return proyecto;
		}

		public void EliminarProyecto(int idProyecto)
		{
			using var connection = new SqlConnection(connectionString);
			connection.Execute(@"update Proyecto
                                set Disponible = 0
                                where idProyecto = @idProyecto", new { idProyecto });
		}
	}
}
