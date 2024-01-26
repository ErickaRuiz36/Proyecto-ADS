using ADS.Models;
using ADS.Services.Conexion;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace ADS.Services
{
    public interface IRepositorioCatalogoConceptos
    {
        int AgregarConcepto(Concepto concepto);
        void AprobarConcepto(int idConcepto);
        int ContarCatalogos(int criterioBusqueda, string datoBusqueda);
        int ContarConceptos(int criterioBusqueda, string datoBusqueda, int idFrenteObra, int estado);
        void EliminarConcepto(int idConcepto);
        bool ExisteConcepto(string Codigo, int? idFrenteObra);
        IEnumerable<Concepto> ListaCatalogoConceptos(PaginacionViewModel paginacion, int idFrenteObra);
        IEnumerable<CatalogosViewModel> ListaCatalogos(PaginacionViewModel paginacion);
        void ModificarConcepto(Concepto concepto);
        ConceptoViewModel ObtenerConcepto(int? idConcepto);
        IEnumerable<Estado> ObtenerEstados();
        int ObtenerIdFrenteObra(int idConcepto);
        IEnumerable<TipoConcepto> ObtenerTiposConceptos();
        void RechazarConcepto(int idConcepto);
    }

    public class RepositorioCatalogoConceptos: IRepositorioCatalogoConceptos
    {
        private readonly string connectionString;

        public RepositorioCatalogoConceptos()
        {
            connectionString = Db_Connect.GetConnectionString();
        }

        public bool ExisteConcepto(string Codigo, int? idFrenteObra)
        {
            using var connection = new SqlConnection(connectionString);

            var existe = connection.QueryFirstOrDefault<int>(@"select 1 from Concepto where 
                Codigo=@Codigo and idFrenteObra = @idFrenteObra", new { Codigo, idFrenteObra });

            return existe == 1;
        }

        public int AgregarConcepto(Concepto concepto)
        {
            concepto.Disponible = true;
            concepto.IdEstado = 1;
            using var connection = new SqlConnection(connectionString);
            var Resultado = connection.QuerySingle<int>(@"insert into Concepto (Codigo,Concepto,Descripcion,Cantidad,Unidad,idTipoConcepto,
                CostoUnidad,idConceptoBase,Disponible,idEstado,idFrenteObra) values(@Codigo,@Concepto1,@Descripcion,@Cantidad,@Unidad,@IdTipoConcepto,
                @CostoUnidad,@IdConceptoBase,@Disponible, @IdEstado,@IdFrenteObra); select scope_identity();", concepto);

            return Resultado;
        }
        private string cadenaBusquedaCatalogos(int criterioBusqueda, string datoBusqueda)
        {
            string busqueda = "";

            if (datoBusqueda != null)
            {
                datoBusqueda = datoBusqueda.Trim();

                if (datoBusqueda != "")
                {
                    if (criterioBusqueda == 1)
                    {
                        busqueda = $"and Proyecto.Nombre like '%{datoBusqueda}%'";
                    }
                    else if (criterioBusqueda == 2)
                    {
                        busqueda = $"and FrenteObra.Nombre like '%{datoBusqueda}%'";
                    }
                    else if (criterioBusqueda == 3)
                    {
                        busqueda = $"and Proyecto.NumContrato like '%{datoBusqueda}%'";
                    }
                    else if (criterioBusqueda == 4)
                    {
                        busqueda = $"and FrenteObra.NumContrato like '%{datoBusqueda}%'";
                    }
                }
            }

            return busqueda;
        }
        public IEnumerable<CatalogosViewModel> ListaCatalogos(PaginacionViewModel paginacion)
        {
            using var connection = new SqlConnection(connectionString);

            string criterioOrden = "Proyecto.Nombre"; // Valor predeterminado
            string direccionOrden = "asc"; // Valor predeterminado

            string busqueda = cadenaBusquedaCatalogos(paginacion.criterioBusqueda, paginacion.datoBusqueda);

            if (paginacion.criterioOrden == 2)
            {
                criterioOrden = "FrenteObra.Nombre";
            }

            if (paginacion.direccionOrden == 2)
            {
                direccionOrden = "desc";
            }

            var catalogos = connection.Query<CatalogosViewModel>($@"select Proyecto.idProyecto, Proyecto.Nombre as NombreProyecto, 
                                    Proyecto.NumContrato as NumContratoProyecto, 
                                    FrenteObra.idFrenteObra, FrenteObra.Nombre as NombreFrenteObra, FrenteObra.NumContrato as NumContratoFrente from Proyecto 
                                    inner join FrenteObra on FrenteObra.idProyecto = Proyecto.idProyecto
                                    where Proyecto.Disponible = 1 and FrenteObra.Disponible = 1 {busqueda}
                                    ORDER BY {criterioOrden} {direccionOrden}
                                    OFFSET {paginacion.registrosSaltar} ROWS
                                    FETCH NEXT {paginacion.RegistrosPagina} ROWS ONLY;");

            foreach(var catalogo in catalogos)
            {
                catalogo.NumConceptos = connection.QuerySingle<int>(@"select COUNT(*) from Concepto 
                                    where idFrenteObra = @idFrenteObra;", new {catalogo.idFrenteObra});
            }

            return catalogos;
        }

        public int ContarCatalogos(int criterioBusqueda, string datoBusqueda)
        {
            string busqueda = cadenaBusquedaCatalogos(criterioBusqueda, datoBusqueda);
            using var connection = new SqlConnection(connectionString);
            return connection.ExecuteScalar<int>(@$"select count(*) from FrenteObra 
                                                inner join Proyecto on Proyecto.idProyecto = FrenteObra.idProyecto
                                                where Proyecto.Disponible = 1 and FrenteObra.Disponible = 1 {busqueda}");
        }

        public IEnumerable<TipoConcepto> ObtenerTiposConceptos()
        {
            using var connection = new SqlConnection(connectionString);
            return connection.Query<TipoConcepto>(@"select idTipoConcepto, TipoConcepto as TipoConcepto1 from TipoConcepto");
        }

        public IEnumerable<Estado> ObtenerEstados()
        {
            using var connection = new SqlConnection(connectionString);
            return connection.Query<Estado>(@"select idEstado, Estado as Estado1 from Estado");
        }

        private string CadenaBusquedaConceptos(int criterioBusqueda, string datoBusqueda)
        {
            string busqueda = "";

            if (datoBusqueda != null)
            {
                datoBusqueda = datoBusqueda.Trim();

                if (datoBusqueda != "")
                {
                    if (criterioBusqueda == 1)
                    {
                        busqueda = $"and Codigo like '%{datoBusqueda}%'";
                    }
                    else if (criterioBusqueda == 2)
                    {
                        busqueda = $"and Concepto like '%{datoBusqueda}%'";
                    }
                    else if (criterioBusqueda == 3)
                    {
                        busqueda = $"and Descripcion like '%{datoBusqueda}%'";
                    }
                }
            }

            return busqueda;
        }

        public IEnumerable<Concepto> ListaCatalogoConceptos(PaginacionViewModel paginacion, int idFrenteObra)
        {
            using var connection = new SqlConnection(connectionString);

            string criterioOrden = "Codigo"; // Valor predeterminado
            string direccionOrden = "asc"; // Valor predeterminado

            string busqueda = CadenaBusquedaConceptos(paginacion.criterioBusqueda, paginacion.datoBusqueda);

            if (paginacion.criterioOrden == 2)
            {
                criterioOrden = "Concepto";
            }else if(paginacion.criterioOrden == 3)
            {
                criterioOrden = "CostoUnidad";
            }

            if (paginacion.direccionOrden == 2)
            {
                direccionOrden = "desc";
            }

            var conceptos = connection.Query<Concepto>($@"select idConcepto, Codigo, Concepto as Concepto1, Descripcion, Cantidad, idTipoConcepto,
                                    CostoUnidad, idConceptoBase, Disponible, idEstado, Unidad, idFrenteObra from Concepto 
                                    where idFrenteObra = @idFrenteObra and Disponible = 1 and idEstado = {paginacion.estado} {busqueda}
                                    ORDER BY {criterioOrden} {direccionOrden}
                                    OFFSET {paginacion.registrosSaltar} ROWS
                                    FETCH NEXT {paginacion.RegistrosPagina} ROWS ONLY;", new { idFrenteObra });
             
            if (conceptos.IsNullOrEmpty())
            {
                var temp = new Concepto();
                temp.IdFrenteObra = idFrenteObra;
                conceptos = conceptos.Concat(new[] { temp });
            }
            else
            {
                foreach(var concepto in conceptos)
                {
                    concepto.IdTipoConceptoNavigation = connection.QuerySingle<TipoConcepto>(@"select idTipoConcepto, TipoConcepto as TipoConcepto1
                                                                    from TipoConcepto where idTipoConcepto = @IdTipoConcepto", new {concepto.IdTipoConcepto});
                    concepto.IdEstadoNavigation = connection.QuerySingle<Estado>(@"select idEstado, Estado as Estado1
                                                                    from Estado where idEstado = @IdEstado", new { concepto.IdEstado });
                }
            }

            return conceptos;
        }

        public int ContarConceptos(int criterioBusqueda, string datoBusqueda, int idFrenteObra, int estado)
        {
            string busqueda = CadenaBusquedaConceptos(criterioBusqueda, datoBusqueda);
            using var connection = new SqlConnection(connectionString);
            return connection.ExecuteScalar<int>(@$"select count(*) from Concepto 
                                                where Disponible = 1 and idFrenteObra = {idFrenteObra} and idEstado = {estado} {busqueda}");
        }

        public ConceptoViewModel ObtenerConcepto(int? idConcepto)
        {
            using var connection = new SqlConnection(connectionString);
            var concepto = connection.QueryFirstOrDefault<ConceptoViewModel>(@"select idConcepto, Codigo, Concepto as Concepto1, Descripcion, Cantidad, idTipoConcepto,
                                    CostoUnidad, idConceptoBase, Disponible, idEstado, Unidad, idFrenteObra from Concepto 
                                    where idConcepto = @idConcepto and Disponible = 1", new {idConcepto});
            return concepto;
        }

        public void ModificarConcepto(Concepto concepto)
        {
            using var connection = new SqlConnection( connectionString);
            connection.Execute(@"update Concepto 
                                set Codigo = @Codigo, Concepto = @Concepto1,
                                Descripcion = @Descripcion, Cantidad = @Cantidad,
                                idTipoConcepto = @IdTipoConcepto,
                                CostoUnidad = @CostoUnidad,
                                idConceptoBase = @IdConceptoBase,
                                idEstado = @IdEstado,
                                Unidad = @Unidad
                                where idConcepto = @IdConcepto", concepto);
        }

        public void EliminarConcepto(int idConcepto)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Execute(@"update Concepto
                                set Disponible = 0
                                where idConcepto = @idConcepto", new { idConcepto });
        }

        public int ObtenerIdFrenteObra(int idConcepto)
        {
            int idFrenteObra = 0;

            using var connection = new SqlConnection(connectionString);

            idFrenteObra = connection.QueryFirstOrDefault<int>(@"select idFrenteObra from Concepto where 
                    idConcepto = @idConcepto", new { idConcepto });

            return idFrenteObra;
        }

        public void AprobarConcepto(int idConcepto)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Execute(@"update Concepto
                                set idEstado = 2
                                where idConcepto = @idConcepto", new { idConcepto });
        }

        public void RechazarConcepto(int idConcepto)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Execute(@"update Concepto
                                set idEstado = 3
                                where idConcepto = @idConcepto", new { idConcepto });
        }
    }
}
