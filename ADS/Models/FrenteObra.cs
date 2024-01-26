using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ADS.Models;

public partial class FrenteObra
{
    public int IdFrenteObra { get; set; }
    [Required(ErrorMessage = "El campo Nombre es requerido.")]
    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public int? IdProyecto { get; set; }
    [Required(ErrorMessage = "El campo Residente es requerido.")]
    public int? IdResidente { get; set; }
    [Required(ErrorMessage = "El campo Supervisor es requerido.")]
    public int? IdSupervisor { get; set; }
    [Required(ErrorMessage = "El campo Superintendente es requerido.")]
    public int? IdSuperintendente { get; set; }
    [Required(ErrorMessage = "El campo Constructora es requerido.")]
    public int? IdConstructora { get; set; }
    [Required(ErrorMessage = "El campo Fecha de inicio es requerido.")]
    public DateTime? FechaInicio { get; set; }
    [Required(ErrorMessage = "El campo Fecha estimada de termino es requerido.")]
    public DateTime? FechaEstimadaTermino { get; set; }

    public int NumContrato { get; set; }

    public DateTime? FechaContrato { get; set; }

    public bool? Disponible { get; set; }

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    public virtual Constructora? IdConstructoraNavigation { get; set; }

    public virtual Proyecto? IdProyectoNavigation { get; set; }

    public virtual Usuario? IdResidenteNavigation { get; set; }

    public virtual Usuario? IdSuperintendenteNavigation { get; set; }

    public virtual Usuario? IdSupervisorNavigation { get; set; }

    public virtual ICollection<ReporteDiario> ReportesDiarios { get; set; } = new List<ReporteDiario>();

    public virtual ICollection<Concepto> Conceptos { get; set; } = new List<Concepto>();
}
