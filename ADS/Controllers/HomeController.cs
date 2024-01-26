using ADS.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ADS.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
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

            ViewData["nombreUsuario"] = nombreUsuario;
            ViewData["idFrenteObra"] = idFrenteObra;
            ViewData["tipoUsuario"] = tipoUsuario;
            
            if(idFrenteObra != 0)
            {
                return View();
            }else if(tipoUsuario == "Administrador")
            {
                return View();
            }
            else
            {
                return RedirectToAction("SinFrenteObra");
            }
            
        }

        [Authorize]
        public IActionResult SinFrenteObra()
        {
            return View();
        }

        public IActionResult AccesoDenegado()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /*A MIMIR*/
        [Authorize]
        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Inicio");
        }

    }
}
