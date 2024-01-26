using ADS.Models;
using ADS.Services.Conexion;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ADS.Services
{
    public interface IRepositorioUsuarios
    {
		int ContarUsuarios(int criterioBusqueda, string datoBusqueda);
		void EliminarUsuario(int idUsuario);
        bool ExisteUsuario(string NombreUsuario);
        IEnumerable<UsuarioViewModel> ListaUsuarios(PaginacionViewModel paginacion);
        void ModificarUsuario(Usuario usuario);
        IEnumerable<TipoUsuario> ObtenerTiposUsuario();
        UsuarioViewModel ObtenerUsuario(int idUsuario);
        int RegistrarUsuario(Usuario usuario);
    }
    public class RepositorioUsuarios: IRepositorioUsuarios
    {
        //    copiar
        private readonly string connectionString;

        public RepositorioUsuarios()
        {
            connectionString = Db_Connect.GetConnectionString();
        }
        // copiar
        
        public int RegistrarUsuario(Usuario usuario)
        {
            
            usuario.Disponible = true;
            using var connection = new SqlConnection(connectionString);  // copiar
            var Resultado = connection.QuerySingle<int>($@"insert into Usuario (Correo,NombreUsuario,Clave,Nombre,ApellidoP,
                ApellidoM,RFC,Telefono,idTipoUsuario,Disponible) values(@Correo,@NombreUsuario,@Clave,@Nombre,@ApellidoP,
                @ApellidoM,@RFC,@Telefono,@idTipoUsuario,@Disponible); select scope_identity();", usuario);

            return Resultado;
        }
        

        /*
        public int RegistrarUsuario(Usuario usuario)
        {
            int resultado;
            usuario.Disponible = true;
            using var connection = new SqlConnection(connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@Correo", usuario.Correo);
            parameters.Add("@Clave", usuario.Clave);
            parameters.Add("@Nombre", usuario.Nombre);
            parameters.Add("@ApellidoP", usuario.ApellidoP);
            parameters.Add("@ApellidoM", usuario.ApellidoM);
            parameters.Add("@RFC", usuario.RFC);
            parameters.Add("@Telefono", usuario.Telefono);
            parameters.Add("@idTipoUsuario", usuario.IdTipoUsuario);
            parameters.Add("@Disponible", usuario.Disponible);
            parameters.Add("@NombreUsuario", usuario.NombreUsuario);
            // Otros parámetros...
            parameters.Add("@Resultado", dbType: DbType.Int32, direction: ParameterDirection.Output);

            connection.Execute("InsertarUsuario", parameters, commandType: CommandType.StoredProcedure);

            resultado = parameters.Get<int>("@Resultado");

            return resultado;
        }

        */

        public bool ExisteUsuario(string NombreUsuario)
        {
            using var connection = new SqlConnection(connectionString);

            var existe = connection.QueryFirstOrDefault<int>(@"select 1 from Usuario where 
                NombreUsuario=@NombreUsuario and Disponible = 1;", new { NombreUsuario });

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
						busqueda = $"and NombreUsuario like '%{datoBusqueda}%'";
					}
					else if (criterioBusqueda == 2)
					{
						busqueda = $"and Correo like '%{datoBusqueda}%'";
					}
					else if (criterioBusqueda == 3)
					{
						busqueda = $"and Nombre like '%{datoBusqueda}%'";
					}
					else if (criterioBusqueda == 4)
					{
						busqueda = $"and ApellidoP like '%{datoBusqueda}%'";
					}
					else if (criterioBusqueda == 5)
					{
						busqueda = $"and ApellidoM like '%{datoBusqueda}%'";
					}
					else if (criterioBusqueda == 6)
					{
						busqueda = $"and Rfc like '%{datoBusqueda}%'";
					}
				}
			}

            return busqueda;
		}

		public IEnumerable<UsuarioViewModel> ListaUsuarios(PaginacionViewModel paginacion)
		{
			using var connection = new SqlConnection(connectionString);

			string criterioOrden = "NombreUsuario"; // Valor predeterminado
			string direccionOrden = "asc"; // Valor predeterminado

            string busqueda = cadenaBusqueda(paginacion.criterioBusqueda,paginacion.datoBusqueda);

			if (paginacion.criterioOrden == 2)
			{
				criterioOrden = "Correo";
			}
			else if (paginacion.criterioOrden == 3)
			{
				criterioOrden = "Nombre";
			}

			if (paginacion.direccionOrden == 2)
			{
				direccionOrden = "desc";
			}

			return connection.Query<UsuarioViewModel>($@"SELECT *
                                FROM Usuario
                                INNER JOIN TipoUsuario ON Usuario.idTipoUsuario = TipoUsuario.idTipoUsuario
                                WHERE Disponible = 1 {busqueda}
                                ORDER BY Usuario.{criterioOrden} {direccionOrden}
                                OFFSET {paginacion.registrosSaltar} ROWS
                                FETCH NEXT {paginacion.RegistrosPagina} ROWS ONLY;
                                ");
		}

		public IEnumerable<TipoUsuario> ObtenerTiposUsuario()
        {
            using var connection = new SqlConnection(connectionString);
            return connection.Query<TipoUsuario>(@"select idTipoUsuario, TipoUsuario as TipoUsuario1 from TipoUsuario");
        }

        public int ContarUsuarios(int criterioBusqueda, string datoBusqueda)
        {
            string busqueda = cadenaBusqueda(criterioBusqueda,datoBusqueda);
            using var connection = new SqlConnection(connectionString);
            return connection.ExecuteScalar<int>(@$"select count(*) from Usuario where Disponible = 1 {busqueda}");
        }

        public void ModificarUsuario(Usuario usuario)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Execute(@"update Usuario 
                                set Correo = @Correo, Clave = @Clave,
                                Nombre = @Nombre,
                                ApellidoP = @ApellidoP,
                                ApellidoM = @ApellidoM,
                                RFC = @Rfc,
                                Telefono = @Telefono,
                                idTipoUsuario = @IdTipoUsuario,
                                NombreUsuario = @NombreUsuario
                                where idUsuario = @IdUsuario",usuario);
        }

        public UsuarioViewModel ObtenerUsuario(int idUsuario)
        {
            using var connection = new SqlConnection(connectionString);
            return connection.QueryFirstOrDefault<UsuarioViewModel>(@"select * from Usuario 
                                            inner join TipoUsuario 
                                            on Usuario.idTipoUsuario = TipoUsuario.idTipoUsuario 
                                            where idUsuario = @IdUsuario and Disponible = 1", new {idUsuario});
        }

        public void EliminarUsuario(int idUsuario)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Execute(@"update Usuario
                                set Disponible = 0
                                where idUsuario = @idUsuario",new {idUsuario});
        }
    }
}
