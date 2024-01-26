using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ADS.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }
    [Required(ErrorMessage ="El campo Correo es requerido.")]
    public string? Correo { get; set; }
    [Required(ErrorMessage ="El campo Clave es requerido.")]
    public string? Clave { get; set; }
    [Required(ErrorMessage = "El campo Nombre es requerido")]
    public string? Nombre { get; set; }
    [Required(ErrorMessage = "El campo Apellido Paterno es requerido")]
    public string? ApellidoP { get; set; }
    [Required(ErrorMessage = "El campo Apellido Materno es requerido")]
    public string? ApellidoM { get; set; }
    [Required(ErrorMessage = "El campo RFC es requerido")]
    public string? Rfc { get; set; }
    
    public long? Telefono { get; set; }
    [Required(ErrorMessage = "El tipo de usuario es requerido")]
    public int? IdTipoUsuario { get; set; }

    public bool? Disponible { get; set; }
    [Required(ErrorMessage = "El campo Nombre usuario es requerido")]
    public string? NombreUsuario { get; set; }

    public virtual ICollection<FrenteObra> FrenteObraIdResidenteNavigations { get; set; } = new List<FrenteObra>();

    public virtual ICollection<FrenteObra> FrenteObraIdSuperintendenteNavigations { get; set; } = new List<FrenteObra>();

    public virtual ICollection<FrenteObra> FrenteObraIdSupervisorNavigations { get; set; } = new List<FrenteObra>();

    public virtual TipoUsuario? IdTipoUsuarioNavigation { get; set; }

    public virtual ICollection<RegistroActividade> RegistroActividades { get; set; } = new List<RegistroActividade>();

    public virtual ICollection<ReporteDiario> ReportesDiarioIdResidenteNavigations { get; set; } = new List<ReporteDiario>();

    public virtual ICollection<ReporteDiario> ReportesDiarioIdSupervisorNavigations { get; set; } = new List<ReporteDiario>();
}
