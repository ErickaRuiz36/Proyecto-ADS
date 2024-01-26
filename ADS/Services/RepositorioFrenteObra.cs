using ADS.Models;
using ADS.Services.Conexion;
using Dapper;
using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ADS.Services
{
    public interface IRepositorioFrenteObra
    {
        int AgregarFrenteObra(FrenteObra frenteObra);
        void EliminarFrenteObra(int idFrenteObra);
        bool ExisteFrenteObra(string? Nombre, int? idConstructora, int? idProyecto);
        void ModificarFrenteObra(FrenteObra frenteObra);
        FrenteObra ObtenerFrenteObra(int idFrenteObra);
        int ObtenerIdProyecto(int idFrenteObra);
    }
    public class RepositorioFrenteObra: IRepositorioFrenteObra
    {
        private readonly string connectionString;

        public RepositorioFrenteObra()
        {
            connectionString = Db_Connect.GetConnectionString();
        }

        public int AgregarFrenteObra(FrenteObra frenteObra)
        {
            using var connection = new SqlConnection(connectionString);
            frenteObra.Disponible = true;
            var resultado = 0;

            if (!ExisteFrenteObra(frenteObra.Nombre, frenteObra.IdConstructora,frenteObra.IdProyecto))
            {
                resultado = connection.QuerySingle<int>(@"insert into FrenteObra (Nombre, Descripcion, idProyecto,
                                            idResidente,idSupervisor,idSuperintendente,idConstructora,
                                            FechaInicio,FechaEstimadaTermino,Disponible,NumContrato,FechaContrato) 
                                            values (@Nombre,@Descripcion,@IdProyecto,@IdResidente,
                                            @IdSupervisor,@IdSuperintendente,@IdConstructora,@FechaInicio,
                                            @FechaEstimadaTermino,@Disponible,@NumContrato,@FechaContrato); 
                                            select scope_identity();", frenteObra);
            }

            return resultado;
        }

        public bool ExisteFrenteObra(string? Nombre, int? idConstructora, int? idProyecto)
        {
            using var connection = new SqlConnection(connectionString);

            var existe = connection.QueryFirstOrDefault<int>(@"select 1 from FrenteObra where 
                (Nombre=@Nombre or idConstructora = @idConstructora) and Disponible = 1 
                    and idProyecto = @idProyecto;", new { Nombre, idConstructora,idProyecto });

            return existe == 1;
        }

        public FrenteObra ObtenerFrenteObra(int idFrenteObra)
        {
            using var connection = new SqlConnection(connectionString);
            var frenteObra = connection.QueryFirstOrDefault<FrenteObra>(@"select * from FrenteObra 
                                            where idFrenteObra = @idFrenteObra and Disponible = 1", new { idFrenteObra });

            frenteObra.IdResidenteNavigation = connection.QueryFirstOrDefault<Usuario>(@"select * from Usuario 
                                            where idUsuario = @IdUsuario", new {IdUsuario = frenteObra.IdResidente});
            frenteObra.IdSupervisorNavigation = connection.QueryFirstOrDefault<Usuario>(@"select * from Usuario 
                                            where idUsuario = @IdUsuario", new { IdUsuario = frenteObra.IdSupervisor });
            frenteObra.IdSuperintendenteNavigation = connection.QueryFirstOrDefault<Usuario>(@"select * from Usuario 
                                            where idUsuario = @IdUsuario", new { IdUsuario = frenteObra.IdSuperintendente });
            frenteObra.IdConstructoraNavigation = connection.QueryFirstOrDefault<Constructora>(@"select * from Constructora 
                                            where idConstructora = @IdConstructora", new { IdConstructora = frenteObra.IdConstructora });
            frenteObra.IdProyectoNavigation = connection.QueryFirstOrDefault<Proyecto>(@"select * from Proyecto 
                                where idProyecto = @IdProyecto", new { IdProyecto = frenteObra.IdProyecto });

            return frenteObra;
        }

        public void ModificarFrenteObra(FrenteObra frenteObra)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Execute(@"update FrenteObra 
                                set Nombre = @Nombre, Descripcion = @Descripcion,
                                idResidente = @IdResidente, idSupervisor = @IdSupervisor,
                                idSuperintendente = @IdSuperintendente,
                                idConstructora = @IdConstructora,
                                NumContrato = @NumContrato,
                                FechaContrato = @FechaContrato,
                                FechaInicio = @FechaInicio,
                                FechaEstimadaTermino = @FechaEstimadaTermino
                                where idFrenteObra = @IdFrenteObra", frenteObra);
        }

        public void EliminarFrenteObra(int idFrenteObra)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Execute(@"update FrenteObra
                                set Disponible = 0
                                where idFrenteObra = @idFrenteObra", new { idFrenteObra });
        }

        public int ObtenerIdProyecto(int idFrenteObra)
        {
            int idProyecto = 0;

            using var connection = new SqlConnection(connectionString);

            idProyecto = connection.QueryFirstOrDefault<int>(@"select idProyecto from FrenteObra where 
                    idFrenteObra = @idFrenteObra", new {idFrenteObra });

            return idProyecto;
        }
    }
}
