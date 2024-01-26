using System;
using System.Collections.Generic;

namespace ADS.Models;

public partial class EstadoProblema
{
    public int IdEstadoProblema { get; set; }

    public string? Estado { get; set; }

    public virtual ICollection<ProblemasReporte> ProblemasReportes { get; set; } = new List<ProblemasReporte>();
}
