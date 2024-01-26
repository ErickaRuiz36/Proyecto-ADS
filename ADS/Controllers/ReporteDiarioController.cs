using ADS.Models;
using ADS.Services;
using Microsoft.AspNetCore.Mvc;
using static ADS.Services.IRepositorioReporteDiario;
using ADS.Services.Conexion;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System;

namespace ADS.Controllers
{
    public class ReporteDiarioController : Controller
    {
        private readonly IRepositorioReporteDiario repositorioReportediario;

        public ReporteDiarioController(IRepositorioReporteDiario repositorioReportediario)
        {
            this.repositorioReportediario = repositorioReportediario;
   
        }
  
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CrearReporte()
        {

            return View();
        }
        public IActionResult EditarReporteDiario()
        {
            return View();
        }
        public IActionResult ListaReportesDiarios(repositorioReporteDiario repositorioReporteDiario)
        {
            IEnumerable<ReporteDiario> reporteDiarios = repositorioReporteDiario.GetReportesDiarios();

            return View(reporteDiarios);
        }
        [HttpGet]
        public IActionResult VisualizarReporteDiario(repositorioReporteDiario  reporteDiario)
        {
            IEnumerable<ReporteDiario> reporteDiarios = Models.ReporteDiario.VerReportes();
            IEnumerable<ReporteDiario> reporteDiarios2 = Models.ReporteDiario.Actividades();
            IEnumerable<ReporteDiario> reporteDiarios3 = Models.ReporteDiario.Problemas();
            IEnumerable<ReporteDiario> reporteDiarios4 = Models.ReporteDiario.ImagenesReporte();
            return View(reporteDiarios, reporteDiarios2,reporteDiarios3,reporteDiarios4);
        }

        private IActionResult View(IEnumerable<ReporteDiario> reporteDiarios, IEnumerable<ReporteDiario> reporteDiarios2, IEnumerable<ReporteDiario> reporteDiarios3, IEnumerable<ReporteDiario> reporteDiarios4)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult CrearReporteDiario(ReporteDiario reporteDiario, repositorioReporteDiario RepositorioReporteDiario)
        {
            if (!ModelState.IsValid)
            {
                reporteDiario.Fecha = DateTime.Now;

                //Agregar a la base de datos
                return View(reporteDiario);

            }

            return RedirectToAction("VisualizarReporteDiario", new { IdReporte = reporteDiario.IdReporte });

 
        }


        public IActionResult Informacion(int idReporte)
        {
            return View();
        }
        /*[HttpGet]
        public IActionResult EditarReporte(IdReporte) {
            string? rp = repositorioReporteDiario.Editar();
            return View(rp);
        }*/

       
        /*[HttpPost]
        public IActionResult EditarReporte(ReporteDiario ReporteDiario, repositorioReporteDiario repositorioReporteDiario)
        {
           var modificar = repositorioReporteDiario.Editar(ReporteDiario);
            return RedirectToAction("DetallesProyecto", new { idReporte = ReporteDiario.IdReporte });
        }*/

       


    }
}
