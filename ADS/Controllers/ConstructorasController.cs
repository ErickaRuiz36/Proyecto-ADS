using ADS.Models;
using ADS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ADS.Controllers
{
    public class ConstructorasController : Controller
    {
        private readonly IRepositorioConstructoras reporsitorioConstructoras;

        public ConstructorasController(IRepositorioConstructoras reporsitorioConstructoras)
        {
            this.reporsitorioConstructoras = reporsitorioConstructoras;
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult ListaConstructoras(PaginacionViewModel paginacion)
        {
            var constructoras = reporsitorioConstructoras.ListaConstructoras(paginacion);
            var totalConstructoras = reporsitorioConstructoras.ContarConstructoras(paginacion.criterioBusqueda, paginacion.datoBusqueda);

            var respuestaVM = new PaginacionRespuesta<Constructora>
            {
                Elementos = constructoras,
                Pagina = paginacion.Pagina,
                registrosPagina = paginacion.RegistrosPagina,
                totalRegistros = totalConstructoras,
                baseURL = Url.Action(),
                criterioOrden = paginacion.criterioOrden,
                direccionOrden = paginacion.direccionOrden,
                criterioBusqueda = paginacion.criterioBusqueda,
                datoBusqueda = paginacion.datoBusqueda
            };

            return View(respuestaVM);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult AgregarConstructora()
        {
            return View();
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult AgregarConstructora(Constructora constructora)
        {
            var existe = reporsitorioConstructoras.ExisteConstructora(constructora.Nombre, constructora.Rfc);

            if (existe)
            {
                ModelState.AddModelError(nameof(constructora.Nombre), $"La constructora {constructora.Nombre} ya existe!");
                ModelState.AddModelError(nameof(constructora.Rfc), $"La constructora con RFC {constructora.Rfc} ya existe!");
                return View(constructora);
            }

            int resultado = reporsitorioConstructoras.AgregarConstructora(constructora);

            if (resultado == 0)
            {
                ModelState.AddModelError(nameof(constructora.Nombre), $"Hubo un error");
                return View(constructora);
            }

            return RedirectToAction("ListaConstructoras");
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult ModificarConstructora(int idConstructora)
        {
            var constructora = reporsitorioConstructoras.ObtenerConstructora(idConstructora);
            
            return View(constructora);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult ModificarConstructora(Constructora constructora)
        {
            reporsitorioConstructoras.ModificarConstructora(constructora);
            return RedirectToAction("ListaConstructoras");
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult EliminarConstructora(int idConstructora)
        {
            reporsitorioConstructoras.EliminarConstructora(idConstructora);
            return RedirectToAction("ListaConstructoras");
        }
    }
}
