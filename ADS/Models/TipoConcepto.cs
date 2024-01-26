using System;
using System.Collections.Generic;

namespace ADS.Models;

public partial class TipoConcepto
{
    public int IdTipoConcepto { get; set; }

    public string? TipoConcepto1 { get; set; }

    public virtual ICollection<Concepto> Conceptos { get; set; } = new List<Concepto>();
}
