using System;
using System.Collections.Generic;

namespace ADS.Models;

public partial class ConceptosFactura
{
    public int IdConceptosFactura { get; set; }

    public int? IdFactura { get; set; }

    public int? IdConcepto { get; set; }

    public int? Cantidad { get; set; }

    public virtual Concepto? IdConceptoNavigation { get; set; }

    
}
