using System;
using System.Collections.Generic;

namespace ADS.Models;

public partial class RegistroActividade
{
    public int IdRegistroActividades { get; set; }

    public string? Tipo { get; set; }

    public string? Tabla { get; set; }

    public int? IdRegistro { get; set; }

    public string? Campo { get; set; }

    public string? DatoAnterior { get; set; }

    public string? DatoNuevo { get; set; }

    public int? IdUsuario { get; set; }

    public DateTime? HoraFecha { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
