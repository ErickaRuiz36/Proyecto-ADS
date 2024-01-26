using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ADS.Models;

public partial class Constructora
{
    public int IdConstructora { get; set; }
    [Required(ErrorMessage = "El campo Nombre es requerido.")]
    public string? Nombre { get; set; }
    [Required(ErrorMessage = "El campo RFC es requerido.")]
    public string? Rfc { get; set; }
    [Required(ErrorMessage = "El campo Telefono es requerido.")]
    public string? Telefono { get; set; }
    [Required(ErrorMessage = "El campo Correo de contacto es requerido.")]
    public string? CorreoContacto { get; set; }
    public bool? Disponible { get; set; }

    public virtual ICollection<FrenteObra> FrenteObras { get; set; } = new List<FrenteObra>();
}
