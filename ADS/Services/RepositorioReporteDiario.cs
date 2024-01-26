using ADS.Models;
using ADS.Services.Conexion;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Configuration;
using System.Threading.Tasks;
//using Aspose.Html;
//using Aspose.Html.Converters;
//using Aspose.Html.Saving;

namespace ADS.Services
{
    public interface IRepositorioReporteDiario
    {
        int ContarReportes(int criterioBusqueda, string datoBusqueda);
        //bool ExisteProyecto(int IdReporte);
        IEnumerable<ReporteDiario> GetReportesDiarios();
        void Editar(ReporteDiario reporteDiario);
        // Proyecto ObtenerProyecto(int idProyecto);
        void CrearReporteDiario(ReporteDiario reporteDiario);
        void AgregarActividades(ReporteDiario reporteDiario);
        void EditarActividades(ReporteDiario reporteDiario);
        void EliminarActividades(ReporteDiario reporteDiario);
        void AgregarProblemas(ReporteDiario reporteDiario);
        void EditarProblemas(ReporteDiario reporteDiario);
        void EliminarProblemas(ReporteDiario reporteDiario);
        IEnumerable<ReporteDiario> VerReportes();
        IEnumerable<ReporteDiario> Actividades();
        IEnumerable<ReporteDiario> Problemas();
        IEnumerable<ReporteDiario> ImagenesReporte();

    }

    public class repositorioReporteDiario : IRepositorioReporteDiario
      {
            private readonly string connectionString;

        public bool IdReporte { get; private set; }

        public repositorioReporteDiario()
            {
                connectionString = Db_Connect.GetConnectionString();
            }

        public int ContarReportes(int criterioBusqueda, string datoBusqueda)
        {
            throw new NotImplementedException();
        }

        public void CrearReporteDiario(ReporteDiario reporteDiario)
        {

            using var connection = new SqlConnection(connectionString);
            var id = connection.QuerySingle<int>($@"INSERT INTO ReportesDiarios(Fecha, HorasLluvia, Progreso ) VALUES(@Fecha,@HorasLluvia,@Progreso); SELECT SCOPE_IDENTITY();", reporteDiario);
            var id2 = connection.QuerySingle<int>($@"INSERT INTO ImagenesReporte(Src) VALUES(@SrcNavigation); SELECT SCOPE_IDENTITY();", reporteDiario);
            var id3 = connection.QuerySingle<int>($@"INSERT INTO ActividadesReporte(Actividad,Descripcion) VALUES (@Actividad, @Descripcion); SELECT SCOPE_IDENTITY();", reporteDiario);

            var id4 = connection.QuerySingle<int>($@"INSERT INTO ProblemasReporte(Problema, ActividadesCorrectivas) VALUES(@Problema, @ActividadesCorrectivas); SELECT SCOPE_IDENTITY();", reporteDiario);
        }

        /*public bool ExisteProyecto(int IdReporte)
        {
            throw new NotImplementedException();
        }*/

        public IEnumerable<ReporteDiario> GetReportesDiarios()
        {
            using var connection = new SqlConnection(connectionString);
            return (IEnumerable<ReporteDiario>)connection.Query<ReporteDiario>($@"SELECT * FROM ReportesDiarios");
        }
        public void Editar(ReporteDiario reporteDiario)
        {
            using var connection = new SqlConnection(connectionString);
           // connection.Execute($@"UPDATE ReportesDiarios SET Fecha = @Fecha, HorasLluvia = @HorasLluvia, Progreso = @Progreso WHERE IdReporte = @IdReporte", ReporteDiario);
           // connection.Execute($@"UPDATE ActividadesReporte SET ");  
        }
        //CRUD
        public void  AgregarActividades(ReporteDiario reporteDiario)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Execute($@"INSERT INTO ActividadesReporte(Actividad, Descripcion) VALUES(@Actividad, @Descripcion)", reporteDiario);
        }
        public void EditarActividades(ReporteDiario reporteDiario)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Execute($@"UPDATE ActividadesReporte SET Actividad = @Actividad, Descripcion = @Descripcion WHERE IdActividad = @IdActividad", reporteDiario);
        }
        public void EliminarActividades(ReporteDiario reporteDiario)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Execute($@"DELETE FROM ActividadesReporte WHERE IdActividad = @IdActividad", reporteDiario);
        }
        public void AgregarProblemas(ReporteDiario reporteDiario)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Execute($@"INSERT INTO ProblemasReporte(Problema, ActividadesCorrectivas) VALUES(@Problema, @ActividadesCorrectivas)", reporteDiario);
        }
        public void EditarProblemas(ReporteDiario reporteDiario)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Execute($@"UPDATE ProblemasReporte SET Problema = @Problema, ActividadesCorrectivas = @ActividadesCorrectivas WHERE IdProblema = @IdProblema", reporteDiario);
        }
        public void EliminarProblemas(ReporteDiario reporteDiario)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Execute($@"DELETE FROM ProblemasReporte WHERE IdProblema = @IdProblema", reporteDiario);
        }
        //TERMINA CRUD NO SLAIO :((( PE
        //VER REPORTES Querys
        public IEnumerable<ReporteDiario> VerReportes() { 
            
            using var connection = new SqlConnection(connectionString);

            return (IEnumerable<ReporteDiario>)connection.Query<ReporteDiario>($@"SELECT * FROM ReportesDiarios where IdReporte = @IdReporte");

        }
        //
        public IEnumerable<ReporteDiario> Actividades()
        {
            using var connection = new SqlConnection(connectionString);
                return (IEnumerable<ReporteDiario>)connection.Query<ReporteDiario>($@"SELECT * FROM ActividadesReporte where IdReporte = @IdReporte");

        }
        public IEnumerable<ReporteDiario> Problemas()
        {
            using var connection = new SqlConnection(connectionString);
                return (IEnumerable<ReporteDiario>)connection.Query<ReporteDiario>($@"SELECT * FROM ProblemasReporte where IdReporte = @IdReporte");

        }
        public IEnumerable<ReporteDiario> ImagenesReporte()
        {
            using var connection = new SqlConnection(connectionString);

                return (IEnumerable<ReporteDiario>)connection.Query<ReporteDiario>($@"SELECT * FROM ImagenesReporte where IdReporte = @IdReporte");
 

        }
        /* public void CrearPDF(ReporteDiario reporteDiario)
         {
            if(IdEstado == 1)
             {
                using var document = new HTMLDocument("document.html");
                var options = new PdfSaveOptions();
                Converter.ConvertHTML(document, options, "output.pdf");

             }
         }*/


    }
}

        
      

     


