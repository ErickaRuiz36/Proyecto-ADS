using System;
using System.Collections.Generic;

namespace ADS.Models;

public partial class ProblemasReporte
{
    public int IdProblemasReporte { get; set; }

    public string? Problema { get; set; }

    public string? ActividadesCorrectivas { get; set; }

    public int? IdEstadoProblema { get; set; }

    public virtual EstadoProblema? IdEstadoProblemaNavigation { get; set; }
}
