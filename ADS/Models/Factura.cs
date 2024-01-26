using System;
using System.Collections.Generic;

namespace ADS.Models;

public partial class Factura
{
    public int IdFactura { get; set; }

    public int? IdFrenteObra { get; set; }

    public int? IdProyecto { get; set; }

    public int? IdEstado { get; set; }

    public string? Periodo { get; set; }

    public DateTime FechaRegistro { get; set; }

    public string? DescripcionServicio { get; set; }

    public decimal? SubTotal { get; set; }

    public decimal? Total { get; set; }

    public bool? Disponible { get; set; }

    public virtual IEnumerable<Concepto> ConceptosFacturas { get; set; } = new List<Concepto>();

    public virtual Estado? IdEstadoNavigation { get; set; }

    public virtual FrenteObra? IdFrenteObraNavigation { get; set; }

    public virtual Proyecto? IdProyectoNavigation { get; set; }

    public virtual String? Residente { get; set; }

}
