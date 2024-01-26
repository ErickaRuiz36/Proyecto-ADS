using ADS.Models;
using ADS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ADS.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ProyectosController : Controller
    {
        private readonly IRepositorioProyectos repositorioProyectos;

        public ProyectosController(IRepositorioProyectos repositorioProyectos)
        {
            this.repositorioProyectos = repositorioProyectos;
        }

        public IActionResult ListaProyectos(PaginacionViewModel paginacion)
        {
			var proyectos = repositorioProyectos.ListaProyectos(paginacion);
			var totalProyectos = repositorioProyectos.ContarProyectos(paginacion.criterioBusqueda, paginacion.datoBusqueda);

			var respuestaVM = new PaginacionRespuesta<Proyecto>
			{
				Elementos = proyectos,
				Pagina = paginacion.Pagina,
				registrosPagina = paginacion.RegistrosPagina,
				totalRegistros = totalProyectos,
				baseURL = Url.Action(),
				criterioOrden = paginacion.criterioOrden,
				direccionOrden = paginacion.direccionOrden,
				criterioBusqueda = paginacion.criterioBusqueda,
				datoBusqueda = paginacion.datoBusqueda
			};

			return View(respuestaVM);
        }

        public IActionResult RegistrarProyecto()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegistrarProyecto(Proyecto proyecto)
        {
            var existe = repositorioProyectos.ExisteProyecto(proyecto.Nombre, proyecto.NumContrato);

            if (existe)
            {
                ModelState.AddModelError(nameof(proyecto.Nombre), $"El proyecto {proyecto.Nombre} ya existe!");
                ModelState.AddModelError(nameof(proyecto.NumContrato), $"El proyecto con contrato {proyecto.NumContrato} ya existe!");
                return View(proyecto);
            }

            int resultado = repositorioProyectos.RegistrarProyecto(proyecto);

            if (resultado == 0)
            {
                ModelState.AddModelError(nameof(proyecto.Nombre), $"Hubo un error");
                return View(proyecto);
            }

            return RedirectToAction("ListaProyectos");
        }

		[HttpGet]
		public IActionResult ModificarProyecto(int idProyecto)
		{
			var proyecto = repositorioProyectos.ObtenerProyecto(idProyecto);
			return View(proyecto);
		}

		[HttpPost]
		public IActionResult ModificarProyecto(Proyecto proyecto)
		{
			repositorioProyectos.ModificarProyecto(proyecto);
			return RedirectToAction("DetallesProyecto", new { idProyecto = proyecto.IdProyecto });
		}

		public IActionResult DetallesProyecto(int idProyecto)
		{
			var proyecto = repositorioProyectos.ObtenerProyecto(idProyecto);
			return View(proyecto);
		}

		[HttpGet]
		public IActionResult EliminarProyecto(int idProyecto)
		{
			repositorioProyectos.EliminarProyecto(idProyecto);
			return RedirectToAction("ListaProyectos");
		}
	}
}
