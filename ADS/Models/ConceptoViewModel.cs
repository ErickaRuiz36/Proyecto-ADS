using Microsoft.AspNetCore.Mvc.Rendering;

namespace ADS.Models
{
    public class ConceptoViewModel: Concepto
    {
        public IEnumerable<SelectListItem>? TiposConcepto { get; set; }

        public IEnumerable<SelectListItem>? Estados { get; set; }
    }
}
