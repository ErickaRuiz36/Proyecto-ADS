using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Security.Cryptography;
using System.Text;

namespace ADS.Recursos
{
    public class UtilidadesSeguridad
    {

        public static string EncriptarClave(string clave)
        {
            StringBuilder sBuilder = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] resul = hash.ComputeHash(enc.GetBytes(clave));

                foreach(byte b in resul)
                {
                    sBuilder.Append(b.ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }

    }
}
