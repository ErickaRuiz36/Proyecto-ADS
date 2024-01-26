using ADS.Models;
using ADS.Services.Conexion;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Drawing;

namespace ADS.Services
{
    public interface IRepositorioFacturas
    {
        void ActualizarComprobacionFactura(int IdFactura, int IdUsuario, int IdEstado);
        int ContarFacturas(int criterioBusqueda, string datoBusqueda, int idFrenteObra);
        bool CrearConceptosFactura(IList<ConceptoTemporal> Lista, int IdFactura);
        bool ExisteFactura(DateTime Fecha, string Periodo);
        IEnumerable<Concepto> ListaConceptos(int IdFrenteObra);
        IEnumerable<Concepto> ListaConceptosFactura(int IdFactura);
        IEnumerable<Factura> ListaFacturas(PaginacionViewModel paginacion);
        Factura ObtenerFactura(int IdFactura);
        int ObtenerIdProyecto(int idFrenteObra);
        string Periodo();
        int RegistrarFactura(Factura factura);
    }

    public class RepositorioFacturas: IRepositorioFacturas
    {
        private readonly string connectionString;

        public RepositorioFacturas()
        {
            connectionString = Db_Connect.GetConnectionString();
        }

        public int RegistrarFactura(Factura factura)
        {
            factura.Disponible = true;
            using var connection = new SqlConnection(connectionString);
            var resultado = 0;

            if (!ExisteFactura(factura.FechaRegistro, factura.Periodo))
            {
                resultado = connection.QuerySingle<int>(@"insert into Factura (idFrenteObra,idProyecto,Periodo,FechaRegistro,DescripcionServicio,
                SubTotal,Total,Disponible) values(@IdFrenteObra,@IdProyecto,@Periodo,@FechaRegistro,@DescripcionServicio,
                @SubTotal,@Total,@Disponible); select scope_identity();", factura);
            }
            connection.Query(@"INSERT INTO FacturaAprobacion (idFactura,idUsuario,idEstado) values(@resultado,null,1);",new { resultado });

            return resultado;
        }
        public bool ExisteFactura(DateTime Fecha, String Periodo)
        {
            using var connection = new SqlConnection(connectionString);

            var existe = connection.QueryFirstOrDefault<int>(@"select 1 from Factura where 
                (FechaRegistro=@Fecha and Periodo = @Periodo) and Disponible = 1;", new { Fecha, Periodo });

            return existe == 1;
        }
        public IEnumerable<Concepto> ListaConceptos(int IdFrenteObra)
        {
            using var connection = new SqlConnection(connectionString);
            return connection.Query<Concepto>(@"select * from Concepto Where idFrenteObra = @IdFrenteObra", new { IdFrenteObra});
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
                        busqueda = $"and FechaRegistro like '%{datoBusqueda}%'";
                    }
                    else if (criterioBusqueda == 2)
                    {
                        busqueda = $"and Periodo like '%{datoBusqueda}%'";
                    }

                }
            }

            return busqueda;
        }

        public IList<ConceptosFactura> ListaConceptosFactura()
        {
            using var connection = new SqlConnection(connectionString);
            return (IList<ConceptosFactura>)connection.Query<ConceptosFactura>(@"select * from Concepto;");
        }

        public bool CrearConceptosFactura(IList<ConceptoTemporal> Lista, int IdFactura)
        {
            using var connection = new SqlConnection(connectionString);
            ConceptoTemporalFactura con = new ConceptoTemporalFactura() {IdFactura = IdFactura };
            List<int> ListaId = new List<int>();
            decimal Valor;
            int IdConcepto;
            for (int i = 0; i < Lista.Count; i++)
            {
                con.Cantidad = Lista[i].Cantidad;
                con.IdConcepto = Lista[i].Concepto.IdConcepto;
                IdConcepto = con.IdConcepto;
                Valor = (decimal)Lista[i].Concepto.Cantidad - (decimal)Lista[i].Cantidad;

                ListaId.Add(connection.QuerySingle<int>(@"insert into ConceptosFactura (IdFactura, IdConcepto, Cantidad ) values(@IdFactura,@IdConcepto,@Cantidad); select scope_identity();", con));
                connection.Query(@"UPDATE Concepto 
                                SET Cantidad = @Valor
                                WHERE Concepto.idConcepto = @IdConcepto", new { Valor ,  IdConcepto });


            }
            return true;
        }

        public int ObtenerIdProyecto(int idFrenteObra)
        {
            int idProyecto = 0;

            using var connection = new SqlConnection(connectionString);

            idProyecto = connection.QueryFirstOrDefault<int>(@"select idProyecto from FrenteObra where 
                    idFrenteObra = @idFrenteObra", new { idFrenteObra });

            return idProyecto;
        }

        public string Periodo()
        {
            int mes = DateTime.Now.Month;
            int año = DateTime.Now.Year;
            if (mes < 10)
            {
                string mesString = '0' + mes.ToString();
                string Ans = año.ToString() + '-' + mesString;
                return Ans;
            }
            string Respuesta = año.ToString() + '-' + mes.ToString();
            return Respuesta;
        }

        public IEnumerable<Factura> facturas()
        {
            using var connection = new SqlConnection(connectionString);
            return connection.Query<Factura>(@"select * from Facturas ");

        }

        //-----------------Paginacion

        private string cadenaBusquedaFacturas(int criterioBusqueda, string datoBusqueda)
        {
            string busqueda = "";

            if (datoBusqueda != null)
            {
                datoBusqueda = datoBusqueda.Trim();

                if (datoBusqueda != "")
                {
                    if (criterioBusqueda == 1)
                    {
                        busqueda = $"and Factura.Periodo like '%{datoBusqueda}%'";
                    }
                    else if (criterioBusqueda == 2)
                    {
                        busqueda = $"and FrenteObra.Nombre like '%{datoBusqueda}%'";
                    }
                }
            }

            return busqueda;
        }
        public IEnumerable<Factura> ListaFacturas(PaginacionViewModel paginacion)
        {
            using var connection = new SqlConnection(connectionString);

            string criterioOrden = "Factura.Periodo"; // Valor predeterminado
            string direccionOrden = "asc"; // Valor predeterminado

            string busqueda = cadenaBusquedaFacturas(paginacion.criterioBusqueda, paginacion.datoBusqueda);

            if (paginacion.criterioOrden == 2)
            {
                criterioOrden = "FrenteObra.Nombre";
            }

            if (paginacion.direccionOrden == 2)
            {
                direccionOrden = "desc";
            }

            var Facturas = connection.Query<Factura>($@"SELECT Usuario.NombreUsuario AS Residente ,
                                    Factura.idFactura as IdFactura, Factura.idFrenteObra as IdFrenteObra, 
                                    Factura.Periodo, Factura.DescripcionServicio , Factura.Total, FacturaAprobacion.idEstado as IdEstado 
                                    FROM Factura 
                                    LEFT JOIN FrenteObra ON Factura.idFrenteObra = FrenteObra.idFrenteObra 
                                    LEFT JOIN FacturaAprobacion ON FacturaAprobacion.idFactura = Factura.idFactura 
                                    LEFT JOIN Usuario ON Usuario.idUsuario = FacturaAprobacion.idUsuario
                                    WHERE Factura.Disponible = 1 {busqueda}
                                    ORDER BY {criterioOrden} {direccionOrden}
                                    OFFSET {paginacion.registrosSaltar} ROWS
                                    FETCH NEXT {paginacion.RegistrosPagina} ROWS ONLY;");


            return Facturas;
        }

        public IEnumerable<Concepto> ListaConceptosFactura(int IdFactura) {
            using var connection = new SqlConnection(connectionString);
            var FrenteObra = connection.Query<Concepto>($@"SELECT ConceptosFactura.Cantidad AS Numero, Concepto.idConcepto AS IdConcepto, Concepto.Concepto AS Concepto1,
                                                        Concepto.Descripcion, Concepto.Cantidad, Concepto.Unidad, Concepto.idFrenteObra as IdFrenteObra
                                                        FROM Factura INNER JOIN ConceptosFactura ON Factura.idFactura = ConceptosFactura.idFactura 
                                                        JOIN Concepto ON Concepto.idConcepto = ConceptosFactura.idConcepto
                                                        WHERE Factura.idFactura = @IdFactura;", new { IdFactura});
            return FrenteObra;
        }

        public int ContarFacturas(int criterioBusqueda, string datoBusqueda, int idFrenteObra)
        {
            string busqueda = cadenaBusquedaFacturas(criterioBusqueda, datoBusqueda);
            using var connection = new SqlConnection(connectionString);
            return connection.ExecuteScalar<int>(@$"select count(*) from Factura 
                                                where Disponible = 1 and idFrenteObra = {idFrenteObra} {busqueda}");
        }

        public Factura ObtenerFactura(int IdFactura)
        {
            using var connection = new SqlConnection (connectionString);
            Factura FacturaResultado = connection.Query<Factura>(@"SELECT * FROM Factura 
                                                                WHERE Factura.idFactura = @IdFactura", new { IdFactura}).FirstOrDefault();

            FacturaResultado.ConceptosFacturas = connection.Query<Concepto>(@"SELECT Concepto.idConcepto AS IdConcepto, Concepto.Codigo,Concepto.Concepto AS Concepto1,Concepto.Descripcion,
                                                                            Concepto.Cantidad, Concepto.CostoUnidad,Concepto.Disponible,Concepto.Unidad,ConceptosFactura.Cantidad AS Numero, Concepto.idFrenteObra AS IdFrenteObra 
                                                                            FROM ConceptosFactura INNER JOIN Factura ON Factura.idFactura = ConceptosFactura.idFactura INNER JOIN Concepto ON Concepto.idConcepto = ConceptosFactura.idConcepto
                                                                            WHERE Factura.idFactura =@IdFactura", new { IdFactura });

            return FacturaResultado;
        }
        public void ActualizarComprobacionFactura(int IdFactura, int IdUsuario, int IdEstado)
        {
            using var connection = new SqlConnection(connectionString);

            connection.Query(@"UPDATE FacturaAprobacion 
                                SET idUsuario = @IdUsuario, idEstado = @IdEstado 
                                WHERE FacturaAprobacion.idFactura = @IdFactura", new { IdUsuario,IdEstado,IdFactura });
          
        }
    }
}
