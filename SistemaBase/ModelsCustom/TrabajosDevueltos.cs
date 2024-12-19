namespace SistemaBase.ModelsCustom
{
    public class TrabajosDevueltos
    {
        public int NroEntrada { get; set; }
        public decimal TipoSolicitud { get; set; }
        public string? DescSolicitud { get; set; }
        public DateTime? FechaEntrada { get; set; }
        public decimal? EstadoEntrada { get; set; }
        public string? DescEstado { get; set; }
        public string? NombreSup { get; set; }
        public string? ObservacionSup { get; set; }
    }
}
