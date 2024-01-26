using System;
using System.Collections.Generic;

namespace ADS.Models;

public partial class ImagenesReporte
{
    public int IdImagenesReporte { get; set; }

    public int? IdReporte { get; set; }

    public required byte[] Src { get; set; }

    public virtual ReporteDiario? IdReporteNavigation { get; set; }
}
