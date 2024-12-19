namespace SistemaBase.ModelsCustom
{
    public class PaginacionViewModel<T>
    {
        public List<T>? Datos { get; set; }
        public int PaginaActual { get; set; }
        public int TamanoPagina { get; set; }
        public int TotalItems { get; set; }
        public int NumeroPaginas => (int)Math.Ceiling((double)TotalItems / TamanoPagina);
    }
}
