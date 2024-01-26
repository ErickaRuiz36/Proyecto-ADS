using ADS.Models;
using ADS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ADS.Controllers
{
    public class FrenteObraController : Controller
    {
        private readonly IRepositorioFrenteObra repositorioFrenteObra;

        public FrenteObraController(IRepositorioFrenteObra repositorioFrenteObra, IRepositorioProyectos repositorioProyectos)
        {
            this.repositorioFrenteObra = repositorioFrenteObra;
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult AgregarFrenteObra(int idProyecto)
        {
            var frenteObra = new FrenteObra();
            frenteObra.IdProyecto = idProyecto;
            return View(frenteObra);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult AgregarFrenteObra(FrenteObra frenteObra)
        {
            var existe = repositorioFrenteObra.ExisteFrenteObra(frenteObra.Nombre, frenteObra.IdConstructora,frenteObra.IdProyecto);

            if (existe)
            {
                ModelState.AddModelError(nameof(frenteObra.Nombre), $"El frente de obra {frenteObra.Nombre} ya existe!");
                ModelState.AddModelError(nameof(frenteObra.IdConstructora), $"El frente de obra con Constructora {frenteObra.IdConstructora} ya existe!");
                return View(frenteObra);
            }

            int resultado = repositorioFrenteObra.AgregarFrenteObra(frenteObra);

            if (resultado == 0)
            {
                ModelState.AddModelError(nameof(frenteObra.Nombre), $"Hubo un error");
                return View(frenteObra);
            }

            return RedirectToAction("DetallesProyecto", "Proyectos", new { idProyecto = frenteObra.IdProyecto });
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult DetallesFrenteObra(int idFrenteObra)
        {
            var frenteObra = repositorioFrenteObra.ObtenerFrenteObra(idFrenteObra);
            return View(frenteObra);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult SeleccionarUsuario(int idTipoUsurio)
        {
            ViewBag.IdTipoUsuario = idTipoUsurio;
            return PartialView("_BuscarUsuario");
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult ModificarFrenteObra(int idFrenteObra)
        {
            var frente = repositorioFrenteObra.ObtenerFrenteObra(idFrenteObra);
            return View(frente);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult ModificarFrenteObra(FrenteObra frenteObra)
        {
            repositorioFrenteObra.ModificarFrenteObra(frenteObra);
            return RedirectToAction("DetallesFrenteObra", new { idFrenteObra = frenteObra.IdFrenteObra });
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult EliminarFrenteObra(int idFrenteObra)
        {
            repositorioFrenteObra.EliminarFrenteObra(idFrenteObra);
            var proyecto = repositorioFrenteObra.ObtenerIdProyecto(idFrenteObra);
            return RedirectToAction("DetallesProyecto", "Proyectos", new {idProyecto = proyecto});
        }
    }
}
