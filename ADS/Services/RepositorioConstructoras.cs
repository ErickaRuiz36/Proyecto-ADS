using ADS.Models;
using ADS.Services.Conexion;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ADS.Services
{
    public interface IRepositorioConstructoras
    {
        int AgregarConstructora(Constructora constructora);
        int ContarConstructoras(int criterioBusqueda, string datoBusqueda);
        void EliminarConstructora(int idConstructora);
        bool ExisteConstructora(string Nombre, string rfc);
        IEnumerable<Constructora> ListaConstructoras(PaginacionViewModel paginacion);
        void ModificarConstructora(Constructora constructora);
        Constructora ObtenerConstructora(int idConstructora);
    }
    public class RepositorioConstructoras: IRepositorioConstructoras
    {
        private readonly string connectionString;

        public RepositorioConstructoras()
        {
            connectionString = Db_Connect.GetConnectionString();
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
                        busqueda = $"and CorreoContacto like '%{datoBusqueda}%'";
                    }
                    else if (criterioBusqueda == 3)
                    {
                        busqueda = $"and RFC like '%{datoBusqueda}%'";
                    }

                }
            }

            return busqueda;
        }

        public IEnumerable<Constructora> ListaConstructoras(PaginacionViewModel paginacion)
        {
            using var connection = new SqlConnection(connectionString);
            string criterioOrden = "Nombre"; // Valor predeterminado
            string direccionOrden = "asc"; // Valor predeterminado

            string busqueda = cadenaBusqueda(paginacion.criterioBusqueda, paginacion.datoBusqueda);

            if (paginacion.criterioOrden == 2)
            {
                criterioOrden = "CorreoContacto";
            }

            if (paginacion.direccionOrden == 2)
            {
                direccionOrden = "desc";
            }

            return connection.Query<Constructora>($@"SELECT *
                                FROM Constructora
                                WHERE Disponible = 1 {busqueda}
                                ORDER BY {criterioOrden} {direccionOrden}
                                OFFSET {paginacion.registrosSaltar} ROWS
                                FETCH NEXT {paginacion.RegistrosPagina} ROWS ONLY;");

        }

        public int ContarConstructoras(int criterioBusqueda, string datoBusqueda)
        {
            string busqueda = cadenaBusqueda(criterioBusqueda, datoBusqueda);
            using var connection = new SqlConnection(connectionString);
            return connection.ExecuteScalar<int>(@$"select count(*) from Constructora where Disponible = 1 {busqueda}");
        }

        public bool ExisteConstructora(string Nombre, string rfc)
        {
            using var connection = new SqlConnection(connectionString);

            var existe = connection.QueryFirstOrDefault<int>(@"select 1 from Constructora where 
                (Nombre=@Nombre or RFC = @rfc) and Disponible = 1;", new { Nombre, rfc });

            return existe == 1;
        }

        public int AgregarConstructora(Constructora constructora)
        {
            constructora.Disponible = true;
            using var connection = new SqlConnection(connectionString);
            var resultado = 0;

            if (!ExisteConstructora(constructora.Nombre, constructora.Rfc))
            {
                resultado = connection.QuerySingle<int>(@"insert into Constructora (Nombre,RFC,Telefono,CorreoContacto,
                                                        Disponible) values(@Nombre,@Rfc,@Telefono,@CorreoContacto,
                                                        @Disponible); select scope_identity();", constructora);
            }

            return resultado;
        }

        public void ModificarConstructora(Constructora constructora)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Execute(@"update Constructora 
                                set Nombre = @Nombre, RFC = @Rfc,
                                Telefono = @Telefono,
                                CorreoContacto = @CorreoContacto
                                where idConstructora = @IdConstructora", constructora);
        }

        public Constructora ObtenerConstructora(int idConstructora)
        {
            using var connection = new SqlConnection(connectionString);
            var constructora = connection.QueryFirstOrDefault<Constructora>(@"select * from Constructora 
                                            where idConstructora = @idConstructora and Disponible = 1", new { idConstructora });

            return constructora;
        }

        public void EliminarConstructora(int idConstructora)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Execute(@"update Constructora
                                set Disponible = 0
                                where idConstructora = @idConstructora", new { idConstructora });
        }
    }
}
