using Microsoft.EntityFrameworkCore;
using ADS.Models;

namespace ADS.Services.ContratoSesion
{
    public interface IUsuarioService
    {
        //Devuelve usuario a partir de los parametros establecidos
        Task<Usuario> GetUsuarios(string correo, string clave);
        Task<int> ObtenerIdFrenteObra(int? idTipoUsuario, int idUsuario);
        //Guarda usuario dentro del modelo creado
        //Task<Usuario> SaveUsuarios(Usuario modelo);
    }
}
