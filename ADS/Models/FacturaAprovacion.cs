namespace ADS.Models
{
    public class FacturaAprovacion
    {
        public int? idFacAprovacionResidente {  get; set; }
        public int? IdUsuario { get; set; }
        
        public int IdFactura { get; set; }

        public int IdEstado { get; set; }
    }
}
