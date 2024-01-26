using Microsoft.EntityFrameworkCore;
using ADS.Services.ContratoSesion;
using ADS.Models;

namespace ADS.Services.ImplementacionSesion
{
    public class UsuarioService : IUsuarioService
    {
        //referencia a la base de datos
        private readonly BaseObrasContext _obrasContext;

        public UsuarioService(BaseObrasContext obrasContext)
        {
            _obrasContext = obrasContext;
        }



        public async Task<Usuario> GetUsuarios(string correo, string clave)
        {
            Usuario usuario_encontrado = await _obrasContext.Usuarios.Where
                (u => u.Correo == correo && u.Clave == clave).FirstOrDefaultAsync();
            if(usuario_encontrado != null)
            {
                usuario_encontrado.IdTipoUsuarioNavigation = await _obrasContext.TipoUsuarios.Where
                (u => u.IdTipoUsuario == usuario_encontrado.IdTipoUsuario).FirstOrDefaultAsync();
            }
            

            return usuario_encontrado;
        }

        public async Task<int> ObtenerIdFrenteObra(int? idTipoUsuario, int idUsuario)
        {
            int idFrenteObra = 0;

            if(idTipoUsuario == 2)
            {
                idFrenteObra = await _obrasContext.FrenteObras
                    .Where(u => u.IdResidente == idUsuario && u.Disponible == true)
                    .Select(u => u.IdFrenteObra)
                    .FirstOrDefaultAsync();
            }
            else if(idTipoUsuario == 3)
            {
                idFrenteObra = await _obrasContext.FrenteObras
                    .Where(u => u.IdSupervisor == idUsuario && u.Disponible == true)
                    .Select(u => u.IdFrenteObra)
                    .FirstOrDefaultAsync();
            }
            else if (idTipoUsuario == 4)
            {
                idFrenteObra = await _obrasContext.FrenteObras
                    .Where(u => u.IdSuperintendente == idUsuario && u.Disponible == true)
                    .Select(u => u.IdFrenteObra)
                    .FirstOrDefaultAsync();
            }

            return idFrenteObra;
        }
        
        /*/  Usar si falla el registro de eri (Spoiler.. no va a fallar xdnt
        public async Task<Usuario> SaveUsuarios(Usuario modelo)
        {
           _obrasContext.Usuarios.Add(modelo);
           await _obrasContext.SaveChangesAsync();
            
           return modelo;
           throw new NotImplementedException();
        }
        */
    }
}
