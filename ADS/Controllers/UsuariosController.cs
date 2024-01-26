using ADS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ADS.Services.Conexion;
using Dapper;
using ADS.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace ADS.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IRepositorioUsuarios repositorioUsuarios;

        public UsuariosController(IRepositorioUsuarios repositorioUsuarios) 
        {
            this.repositorioUsuarios = repositorioUsuarios;
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult ListaUsuarios(PaginacionViewModel paginacion)
        {
            var usuarios = repositorioUsuarios.ListaUsuarios(paginacion);
            var totalUsuarios = repositorioUsuarios.ContarUsuarios(paginacion.criterioBusqueda,paginacion.datoBusqueda);

            var respuestaVM = new PaginacionRespuesta<UsuarioViewModel>
            {
                Elementos = usuarios,
                Pagina = paginacion.Pagina,
                registrosPagina = paginacion.RegistrosPagina,
                totalRegistros = totalUsuarios,
                baseURL = Url.Action(),
                criterioOrden = paginacion.criterioOrden,
                direccionOrden = paginacion.direccionOrden,
                criterioBusqueda = paginacion.criterioBusqueda,
                datoBusqueda = paginacion.datoBusqueda
			};

            return View(respuestaVM);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult RegistrarUsuario()
        {
            var tiposUsuario = repositorioUsuarios.ObtenerTiposUsuario();
            var modelo = new UsuarioViewModel();
            modelo.TiposUsuario = tiposUsuario.Select(x => new SelectListItem(x.TipoUsuario1, x.IdTipoUsuario.ToString()));
            return View(modelo);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult RegistrarUsuario(Usuario usuario)
        {
            var existe = repositorioUsuarios.ExisteUsuario(usuario.NombreUsuario);

            if(!ModelState.IsValid)
            {
                return View(usuario);
            }

            if (existe)
            {
                ModelState.AddModelError(nameof(usuario.NombreUsuario), $"El usuario {usuario.NombreUsuario} ya existe!");
                return View(usuario);
            }

            int resultado = repositorioUsuarios.RegistrarUsuario(usuario);

            if(resultado == 0)
            {
                ModelState.AddModelError(nameof(usuario.NombreUsuario), $"Hubo un error");
                return View(usuario);
            }

            return RedirectToAction("ListaUsuarios");
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult ModificarUsuario(int idUsuario)
        {
            var usuario = repositorioUsuarios.ObtenerUsuario(idUsuario);
            var tiposUsuario = repositorioUsuarios.ObtenerTiposUsuario();
            usuario.TiposUsuario = tiposUsuario.Select(x => new SelectListItem(x.TipoUsuario1, x.IdTipoUsuario.ToString()));
            return View(usuario);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult ModificarUsuario(Usuario usuario)
        {
            repositorioUsuarios.ModificarUsuario(usuario);
            return RedirectToAction("DetallesUsuario", new { idUsuario = usuario.IdUsuario });
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult DetallesUsuario(int idUsuario)
        {
            var usuario = repositorioUsuarios.ObtenerUsuario(idUsuario);
            return View(usuario);
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult EliminarUsuario(int idUsuario)
        {
            repositorioUsuarios.EliminarUsuario(idUsuario);
            return RedirectToAction("ListaUsuarios");
        }
    }
}
