using Microsoft.AspNetCore.Mvc;

namespace ADS.Controllers
{
    public class AprobarReportesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ListaReportesdiarios()
        {
            return View();
        }
        /*public IActionResult AprobarReporteDiario(AprobarReportes aprobarReportes)
        {
            if (!ModelState.Valid)
            {
                return View();
            }
            return View();
        }

        public IActionResult About() { }*/
    }
}
