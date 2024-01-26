namespace ADS.Models
{
    public class PaginacionViewModel
    {
        public int Pagina { get; set; } = 1;
        private int registrosPagina = 5;
        private readonly int registrosMaximos = 30;
        public int criterioOrden { get; set; } = 1;
        public int direccionOrden { get; set; } = 1;
        public int criterioBusqueda { get; set; } = 1;
        public string datoBusqueda { get; set; } = "";
        public int id { get; set; }

        public int estado { get; set; } = 2;

        public int RegistrosPagina
        {
            get{
                return registrosPagina;
            }
            set {
                registrosPagina = (value > registrosMaximos) ?
                    registrosMaximos : value;
            }
        }

        public int registrosSaltar => registrosPagina * (Pagina - 1);
    }
}
