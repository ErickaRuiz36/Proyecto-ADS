using Microsoft.AspNetCore.Mvc;

using ADS.Models;
using ADS.Recursos;
using ADS.Services.ContratoSesion;
//Manejo de cookies
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ADS.Services.ImplementacionSesion;

namespace ADS.Controllers
{
    public class InicioController : Controller
    {
        private readonly IUsuarioService _usuarioServ;

        public InicioController(IUsuarioService usuarioServ)
        {
            _usuarioServ = usuarioServ;
        }

        //LOGIN
        //Devuelve vista
        public IActionResult Login()
        {
            return View();
        }
        //Aloja las funcinoes del controlador Owo
        [HttpPost]
        public async Task<IActionResult> Login(string correo, string clave)
        {
            Usuario usuario_encontrado = await _usuarioServ.GetUsuarios(correo, clave/*, UtilidadesSeguridad.EncriptarClave(clave)*/);
            if (usuario_encontrado == null)
            {
                ViewData["Mensaje"] = "No se encontró usuario dentro del sistema";
                return View();
            }

            List<Claim> claims = new List<Claim>()
            {new Claim(ClaimTypes.Name, usuario_encontrado.NombreUsuario),
                new Claim(ClaimTypes.Role, usuario_encontrado.IdTipoUsuarioNavigation.TipoUsuario1),
                new Claim("idUsuario", usuario_encontrado.IdUsuario.ToString())
            };

            if(usuario_encontrado.IdTipoUsuario != 1)
            {
                var idFrenteObra = await _usuarioServ.ObtenerIdFrenteObra(usuario_encontrado.IdTipoUsuario, usuario_encontrado.IdUsuario);
                claims.Add(new Claim("idFrenteObra", idFrenteObra.ToString()));
            }
            
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties properties = new AuthenticationProperties(){AllowRefresh = true};

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(claimsIdentity), properties);

            /*if (usuario_encontrado.IdTipoUsuario == 1)
            {
                return RedirectToAction("AdministradorIndex", "Home");
            }
            else if (usuario_encontrado.IdTipoUsuario == 2)
            {
                return RedirectToAction("Personal1", "Home");
            }*/
            return RedirectToAction("Index","Home");
        }
    }
}
