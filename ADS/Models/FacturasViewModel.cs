namespace ADS.Models
{
    public class ConceptosViewModel
    {
        public required IList<ConceptoTemporal> ConceptoTemporal { get; set; }
        public List<ConceptoTemporal>? ConceptoTemporals { get; set; } 

        public string? Descripcion { get; set; }
        
    }

    public class RevisarFacturaViewModel
    {
        public required Factura FacturaRev { get; set; }
        
    }

    public class FacturasViewModel : Factura
    {
        public int? IdFactura { get; set; }

        public string Residente { get; set; }

        public DateTime FechaRegistro { get; set; }

        public string? Periodo { get; set; }

        public int IdUsuario { get; set; }

        public int? Total { get; set; }

        public int Estado { get; set; }
    }


}
