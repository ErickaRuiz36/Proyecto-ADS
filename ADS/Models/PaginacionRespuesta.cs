namespace ADS.Models
{
    public class PaginacionRespuesta
    {
        public int Pagina { get; set; } = 1;
        public int registrosPagina { get; set; } = 5;
        public int totalRegistros { get; set;}
        public int totalPaginas => (int)Math.Ceiling((double)totalRegistros/registrosPagina);
        public string? baseURL { get; set; }
		public int criterioOrden { get; set; } = 1;
		public int direccionOrden { get; set; } = 1;
        public int criterioBusqueda { get; set; } = 1;
        public string datoBusqueda { get; set; } = "";
        public int id { get; set; }
        public int estado { get; set; } = 2;
    }

    public class PaginacionRespuesta<T>: PaginacionRespuesta
    {
        public IEnumerable<T>? Elementos { get; set; }
    }
}
