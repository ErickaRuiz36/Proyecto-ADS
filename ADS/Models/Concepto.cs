using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace ADS.Models;

public partial class Concepto
{
    public int IdConcepto { get; set; }

    public string? Codigo { get; set; }

    public string? Concepto1 { get; set; }

    public string? Descripcion { get; set; }

    public decimal? Cantidad { get; set; }

    public int? IdTipoConcepto { get; set; }

    public decimal? CostoUnidad { get; set; }

    public int? IdConceptoBase { get; set; }

    public bool? Disponible { get; set; }

    public int? IdEstado { get; set; }

    public string? Unidad { get; set; }

    public int? IdFrenteObra { get; set; }

    public virtual ICollection<ConceptosFactura> ConceptosFacturas { get; set; } = new List<ConceptosFactura>();

    public virtual Estado? IdEstadoNavigation { get; set; }

    public virtual TipoConcepto? IdTipoConceptoNavigation { get; set; }

    public virtual FrenteObra? IdFrenteObraNavigation { get; set; }
    public virtual int? Numero { get; set; }

}
