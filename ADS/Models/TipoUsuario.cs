using System;
using System.Collections.Generic;

namespace ADS.Models;

public partial class TipoUsuario
{
    public int IdTipoUsuario { get; set; }

    public string? TipoUsuario1 { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
