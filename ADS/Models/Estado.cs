using System;
using System.Collections.Generic;

namespace ADS.Models;

public partial class Estado
{
    public int IdEstado { get; set; }

    public string? Estado1 { get; set; }

    public virtual ICollection<Concepto> Conceptos { get; set; } = new List<Concepto>();

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    public virtual ICollection<ReporteDiario> ReportesDiarios { get; set; } = new List<ReporteDiario>();
}
