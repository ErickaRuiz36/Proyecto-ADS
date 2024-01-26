using ADS.Models;
using ADS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ADS.Controllers
{
    public class CatalogoConceptosController : Controller
    {
        private readonly IRepositorioCatalogoConceptos repositorioCatalogoConceptos;

        public CatalogoConceptosController(IRepositorioCatalogoConceptos repositorioCatalogoConceptos)
        {
            this.repositorioCatalogoConceptos = repositorioCatalogoConceptos;
        }

        [Authorize(Roles = "Administrador, Superintendente, Residente")]
        public IActionResult ListaCatalogoConceptos(PaginacionViewModel paginacion)
        {
            var conceptos = repositorioCatalogoConceptos.ListaCatalogoConceptos(paginacion, paginacion.id);

            var totalConceptos = repositorioCatalogoConceptos.ContarConceptos(paginacion.criterioBusqueda, paginacion.datoBusqueda, paginacion.id, paginacion.estado);

            var respuestaVM = new PaginacionRespuesta<Concepto>
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
                id = paginacion.id,
                estado = paginacion.estado
            };

            return View(respuestaVM);
        }

        [Authorize(Roles = "Administrador, Superintendente")]
        public IActionResult AgregarConcepto(ConceptoViewModel concepto)
        {
            var tiposConcepto = repositorioCatalogoConceptos.ObtenerTiposConceptos();
            concepto.TiposConcepto = tiposConcepto.Select(x => new SelectListItem(x.TipoConcepto1, x.IdTipoConcepto.ToString()));

            if(concepto.IdConceptoBase != null)
            {
                var temp = concepto.IdConceptoBase;
                concepto = repositorioCatalogoConceptos.ObtenerConcepto(concepto.IdConceptoBase);
                concepto.IdConceptoBase = temp;
            }

            return View(concepto);
        }

        [Authorize(Roles = "Administrador, Superintendente")]
        [HttpPost]
        public IActionResult AgregarConcepto(Concepto concepto)
        {
            var existe = repositorioCatalogoConceptos.ExisteConcepto(concepto.Codigo, concepto.IdFrenteObra);

            if (existe)
            {
                ModelState.AddModelError(nameof(concepto.Codigo), $"El concepto con codigo {concepto.Codigo} ya existe!");
                return View(concepto);
            }

            int resultado = repositorioCatalogoConceptos.AgregarConcepto(concepto);

            if (resultado == 0)
            {
                ModelState.AddModelError(nameof(concepto.Codigo), $"Hubo un error");
                return View(concepto);
            }

            return RedirectToAction("ListaCatalogoConceptos", new {id = concepto.IdFrenteObra});
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult ListaCatalogos(PaginacionViewModel paginacion)
        {
            var catalogos = repositorioCatalogoConceptos.ListaCatalogos(paginacion);
            var totalCatalogos = repositorioCatalogoConceptos.ContarCatalogos(paginacion.criterioBusqueda, paginacion.datoBusqueda);

            var respuestaVM = new PaginacionRespuesta<CatalogosViewModel>
            {
                Elementos = catalogos,
                Pagina = paginacion.Pagina,
                registrosPagina = paginacion.RegistrosPagina,
                totalRegistros = totalCatalogos,
                baseURL = Url.Action(),
                criterioOrden = paginacion.criterioOrden,
                direccionOrden = paginacion.direccionOrden,
                criterioBusqueda = paginacion.criterioBusqueda,
                datoBusqueda = paginacion.datoBusqueda
            };

            return View(respuestaVM);
        }

        [Authorize(Roles = "Administrador, Superintendente")]
        [HttpGet]
        public IActionResult ModificarConcepto(int idConcepto)
        {
            var concepto = repositorioCatalogoConceptos.ObtenerConcepto(idConcepto);

            var tiposConcepto = repositorioCatalogoConceptos.ObtenerTiposConceptos();
            concepto.TiposConcepto = tiposConcepto.Select(x => new SelectListItem(x.TipoConcepto1, x.IdTipoConcepto.ToString()));

            var estados = repositorioCatalogoConceptos.ObtenerEstados();
            concepto.Estados = estados.Select(x => new SelectListItem(x.Estado1, x.IdEstado.ToString()));

            return View(concepto);
        }

        [Authorize(Roles = "Administrador, Superintendente")]
        [HttpPost]
        public IActionResult ModificarConcepto(Concepto concepto)
        {
            repositorioCatalogoConceptos.ModificarConcepto(concepto);
            return RedirectToAction("ListaCatalogoConceptos", new { id = concepto.IdFrenteObra });
        }

        [Authorize(Roles = "Administrador, Superintendente")]
        [HttpGet]
        public IActionResult EliminarConcepto(int idConcepto)
        {
            repositorioCatalogoConceptos.EliminarConcepto(idConcepto);
            var idFrenteObra = repositorioCatalogoConceptos.ObtenerIdFrenteObra(idConcepto);
            return RedirectToAction("ListaCatalogoConceptos", new { id = idFrenteObra });
        }

        public IActionResult AprobarConcepto(int idConcepto)
        {
            repositorioCatalogoConceptos.AprobarConcepto(idConcepto);
            var idFrenteObra = repositorioCatalogoConceptos.ObtenerIdFrenteObra(idConcepto);
            return RedirectToAction("ListaCatalogoConceptos", new { id = idFrenteObra });
        }

        public IActionResult RechazarConcepto(int idConcepto)
        {
            repositorioCatalogoConceptos.RechazarConcepto(idConcepto);
            var idFrenteObra = repositorioCatalogoConceptos.ObtenerIdFrenteObra(idConcepto);
            return RedirectToAction("ListaCatalogoConceptos", new { id = idFrenteObra });
        }
    }
}
