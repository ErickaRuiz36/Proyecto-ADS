using System;
using System.Collections.Generic;

namespace ADS.Models;

public partial class ReporteDiario
{
    public int IdReporte { get; set; }

    public DateTime? Fecha { get; set; }

    public int? IdSupervisor { get; set; }

    public int? IdFrenteObra { get; set; }

    public string? Descripcion { get; set; }

    public int? IdEstado { get; set; }

    public int? IdResidente { get; set; }

    public string? ComentariosResidente { get; set; }

    public int? HorasLluvia { get; set; }

    public int? Progreso { get; set; }

    public bool? Disponible { get; set; }
    public string? Actividad { get; set; }
    public string? Problema{ get; set; }
    public string? ActividadesCorrectivas { get; set; }
    public required Byte[] Src { get; set; }
    
    public  ActividadesReporte? ActividadNavigation { get; set; }
    public virtual Estado? IdEstadoNavigation { get; set; }

    public virtual FrenteObra? IdFrenteObraNavigation { get; set; }

    public virtual Usuario? IdResidenteNavigation { get; set; }

    public virtual Usuario? IdSupervisorNavigation { get; set; }

    public virtual ProblemasReporte? ProblemasReporteNavigation { get; set; }

    public virtual ProblemasReporte? ActividadesCorrectivasNavigation{ get; set;}

    public virtual ProblemasReporte? IdEstadoProblemaNavigation { get; set; }

    public virtual EstadoProblema? EstadoNavigation { get; set; }
    
    public virtual ImagenesReporte? SrcNavigation{ get; set; }

    public virtual ICollection<Estado> Estados { get; set; } = new List<Estado>();
    public virtual ICollection<ActividadesReporte> ActividadesReportes { get; set; } = new List<ActividadesReporte>();


    public virtual ICollection<ProblemasReporte> ProblemasReportes { get; set; } = new List<ProblemasReporte>();

    public virtual ICollection<ImagenesReporte> ImagenesReportes { get; set; } = new List<ImagenesReporte>();

    public virtual ICollection<EstadoProblema> EstadoProblemas { get; set; } = new List<EstadoProblema>();

    internal static void FindAsync(int id)
    {
        throw new NotImplementedException();
    }

    internal static IEnumerable<ReporteDiario> VerActividades()
    {
        throw new NotImplementedException();
    }

    internal static IEnumerable<ReporteDiario> ImagenesReporte()
    {
        throw new NotImplementedException();
    }

    internal static IEnumerable<ReporteDiario> VerProblemas()
    {
        throw new NotImplementedException();
    }

    internal static IEnumerable<ReporteDiario> VerReportes()
    {
        throw new NotImplementedException();
    }

    internal static IEnumerable<ReporteDiario> Problemas()
    {
        throw new NotImplementedException();
    }

    internal static IEnumerable<ReporteDiario> Actividades()
    {
        throw new NotImplementedException();
    }
}
