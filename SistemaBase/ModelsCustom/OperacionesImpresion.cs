namespace SistemaBase.ModelsCustom
{
    public class OperacionesImpresion
    {
        public decimal NroEntrada { get; set; }
        public decimal? TipoSolicitud { get; set; }
        public string? DescSolicitud { get; set; }
        public DateTime? FechaAlta { get; set; }
        public string? DescEstado { get; set; }
        public string? Observacion { get; set; }
        public string? NombreOperador { get; set; }
    }
}
