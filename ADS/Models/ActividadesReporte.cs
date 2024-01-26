using System;
using System.Collections.Generic;

namespace ADS.Models;

public partial class ActividadesReporte
{
    public int IdActividadesReporte { get; set; }

    public int? IdReporte { get; set; }

    public string? Actividad { get; set; }

    public string? Descripcion { get; set; }

    public virtual ReporteDiario? IdReporteNavigation { get; set; }
}
