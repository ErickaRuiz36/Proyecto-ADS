using Microsoft.AspNetCore.Mvc.Rendering;

namespace ADS.Models
{
    public class UsuarioViewModel: Usuario
    {
        public virtual IEnumerable<SelectListItem>? TiposUsuario { get; set; }

        public virtual string? TipoUsuario { get; set; }

    }
}
