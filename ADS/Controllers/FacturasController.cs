using ADS.Models;
using ADS.Services;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ADS.Controllers
{
    public class FacturasController : Controller
    {
        private readonly IRepositorioFacturas repositorioFacturas;
        private ConceptosViewModel ConceptosViewModel;
        private RevisarFacturaViewModel RevisarViewModel;
        public FacturasController(IRepositorioFacturas repositorioFacturas)
        {
            this.repositorioFacturas = repositorioFacturas;
            this.ConceptosViewModel = new ConceptosViewModel()
            { ConceptoTemporal = new List<ConceptoTemporal>() };
            this.RevisarViewModel = new RevisarFacturaViewModel()
            { FacturaRev = new Factura()};
            
        }

        // ------------------------------- Acciones crear facturas

        public IActionResult CrearFactura()
        {
            int frenteObra = FrenteObtener();
            IEnumerable<Concepto> lista = repositorioFacturas.ListaConceptos(frenteObra);
            IList<Concepto> ListaSi = lista.ToList();
            for (int i = 0; i < lista.Count(); i++)
            {
                ConceptosViewModel.ConceptoTemporal.Add(new ConceptoTemporal { Concepto = ListaSi[i], Seleccion = false, Cantidad = 0 });
            }

            return View("CrearFactura", ConceptosViewModel);
        }
        [HttpPost]
        public IActionResult Recibir(ConceptosViewModel ListaFactura)
        {
            List<ConceptoTemporal> ListaTemp = ListaFactura.ConceptoTemporal.Where(x => x.Seleccion == true).ToList();
            ConceptosViewModel.ConceptoTemporal = ListaFactura.ConceptoTemporal;
            ConceptosViewModel.ConceptoTemporals = ListaTemp;
            return View("CrearFactura", ConceptosViewModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult CrearFacturaAccion(ConceptosViewModel ListaFactura)
        {
            List<ConceptoTemporal> ListaTemp = ListaFactura.ConceptoTemporals.Where(x => x.Cantidad >= 1).ToList();
            Factura factura = new Factura();
            if(ComprobarExistencias(ListaTemp) == false)
            {
                return View("Error", new ErrorViewModel { RequestId = "Error, no hay suficientes existencias." });
            }
            decimal tempSubTotal = 0;
            int IdFactura = 0;
            for (int i = 0; i < ListaTemp.Count; i++)
            {
                tempSubTotal = (decimal)ListaTemp[i].Concepto.CostoUnidad * ListaTemp[i].Cantidad + tempSubTotal;
            }

            factura.SubTotal = tempSubTotal;
            factura.Total = tempSubTotal * (decimal)1.16;
            factura.FechaRegistro = DateTime.Now;
            factura.IdFrenteObra = FrenteObtener();
            factura.Periodo = repositorioFacturas.Periodo();
            factura.IdEstado = 0;
            factura.IdProyecto = repositorioFacturas.ObtenerIdProyecto((int)factura.IdFrenteObra);
            factura.DescripcionServicio = ListaFactura.Descripcion;
            IdFactura = repositorioFacturas.RegistrarFactura(factura);
            

            if (IdFactura != 0)
            {
                repositorioFacturas.CrearConceptosFactura(ListaTemp,IdFactura);
            }
            else
            {
                return View("Error", new ErrorViewModel { RequestId = "Error, no se pudo crear la factura." });
            }

            return View("Aceptacion");
            
        }
        //------------------------- Acciones listado facturas

        public IActionResult ListadoFacturas(PaginacionViewModel paginacion)
        {
            var conceptos = repositorioFacturas.ListaFacturas(paginacion);

            var totalConceptos = repositorioFacturas.ContarFacturas(paginacion.criterioBusqueda, paginacion.datoBusqueda, paginacion.id);

            var respuestaVM = new PaginacionRespuesta<Factura>
            {
                Elementos = conceptos,
                Pagina = paginacion.Pagina,
                registrosPagina = paginacion.RegistrosPagina,
                totalRegistros = totalConceptos,
                baseURL = Url.Action(),
                criterioOrden = paginacion.criterioOrden,
                direccionOrden = paginacion.direccionOrden,
                criterioBusqueda = paginacion.criterioBusqueda,
                datoBusqueda = paginacion.datoBusqueda,
                id = paginacion.id
            };

            return View("Facturas",respuestaVM);
        }

        //public
        //------------------------- Acciones Revisar facturas
        public IActionResult RevisarFactura(int IdFactura)
        {
            Factura factura = repositorioFacturas.ObtenerFactura(IdFactura);
            RevisarViewModel.FacturaRev=factura;
            return View("RevisarFactura",RevisarViewModel);
        }

        public IActionResult RevisarFacturaAcceptar(int IdFactura)
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string nombreUsuario = "";
            int IdUsuario = 0;
            string tipoUsuario = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                nombreUsuario = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();

                tipoUsuario = claimuser.Claims.Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value).SingleOrDefault();

                // Obtener el valor como cadena
                string idFrenteObraString = claimuser.Claims.Where(c => c.Type == "idUsuario")
                    .Select(c => c.Value).SingleOrDefault();

                // Convertir el valor a int
                if (int.TryParse(idFrenteObraString, out IdUsuario))
                {
                    // El valor se pudo convertir correctamente a int
                    // Puedes continuar utilizando idFrenteObra como un entero
                }
                else
                {
                    // Manejar la situación en la que no se pudo convertir a int
                    // Puedes lanzar una excepción, asignar un valor por defecto, etc.
                    // Por ejemplo:
                    // idFrenteObra = 0; // Valor por defecto
                    // O puedes lanzar una excepción
                    //throw new InvalidOperationException("No se pudo convertir idFrenteObra a un entero.");
                }
            }
            if (IdUsuario != 0 )
            {
                repositorioFacturas.ActualizarComprobacionFactura(IdFactura,IdUsuario,2);
                return View("Aceptacion");
            }

            return View("Error", new ErrorViewModel { RequestId = "Error, no se pudo aprovar/rechazar la factura." });
        }

        public IActionResult RevisarFacturaRechazar(int IdFactura)
        {

            ClaimsPrincipal claimuser = HttpContext.User;
            string nombreUsuario = "";
            int IdUsuario = 0;
            string tipoUsuario = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                nombreUsuario = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();

                tipoUsuario = claimuser.Claims.Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value).SingleOrDefault();

                // Obtener el valor como cadena
                string idFrenteObraString = claimuser.Claims.Where(c => c.Type == "idUsuario")
                    .Select(c => c.Value).SingleOrDefault();

                // Convertir el valor a int
                if (int.TryParse(idFrenteObraString, out IdUsuario))
                {
                    // El valor se pudo convertir correctamente a int
                    // Puedes continuar utilizando idFrenteObra como un entero
                }
                else
                {
                    // Manejar la situación en la que no se pudo convertir a int
                    // Puedes lanzar una excepción, asignar un valor por defecto, etc.
                    // Por ejemplo:
                    // idFrenteObra = 0; // Valor por defecto
                    // O puedes lanzar una excepción
                    //throw new InvalidOperationException("No se pudo convertir idFrenteObra a un entero.");
                }
            }
            if (IdUsuario != 0)
            {
                repositorioFacturas.ActualizarComprobacionFactura(IdFactura, IdUsuario, 3);
                return View("Aceptacion");
            }

            return View("Error", new ErrorViewModel { RequestId = "Error, no se pudo aprovar/rechazar la factura." });
        }

        //------------------------- Extras

        public bool ComprobarExistencias(List<ConceptoTemporal> Lista)
        {
            int num_desabasto = 0;
            foreach (var conceptoTemporal in Lista)
            {
                if(conceptoTemporal.Cantidad > conceptoTemporal.Concepto.Cantidad)
                {
                    num_desabasto++;
                }
            }

            if (num_desabasto > 0)
                return false;
            else
                return true;

        }
        public int FrenteObtener()
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string nombreUsuario = "";
            int idFrenteObra = 0;
            string tipoUsuario = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                nombreUsuario = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();

                tipoUsuario = claimuser.Claims.Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value).SingleOrDefault();

                // Obtener el valor como cadena
                string idFrenteObraString = claimuser.Claims.Where(c => c.Type == "idFrenteObra")
                    .Select(c => c.Value).SingleOrDefault();

                // Convertir el valor a int
                if (int.TryParse(idFrenteObraString, out idFrenteObra))
                {
                    // El valor se pudo convertir correctamente a int
                    // Puedes continuar utilizando idFrenteObra como un entero
                }
                else
                {
                    // Manejar la situación en la que no se pudo convertir a int
                    // Puedes lanzar una excepción, asignar un valor por defecto, etc.
                    // Por ejemplo:
                    // idFrenteObra = 0; // Valor por defecto
                    // O puedes lanzar una excepción
                    //throw new InvalidOperationException("No se pudo convertir idFrenteObra a un entero.");
                }
            }

            return idFrenteObra;
        }

    }
}