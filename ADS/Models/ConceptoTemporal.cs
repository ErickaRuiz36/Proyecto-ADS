namespace ADS.Models
{
    public class ConceptoTemporal 
    {

        public bool Seleccion { get; set; }

        public required Concepto Concepto { get; set; }

        public int Cantidad { get; set; }

    }

    public class ConceptoTemporalFactura 
    {
        public int IdConcepto { get; set; }

        public int Cantidad { get; set; }

        public int IdFactura { get; set; }
    }

    public class ConceptoRevisionFactura
    {
        public Concepto? Concepto { get; set; }

        public int Cantidad { get; set; }

    }
}
