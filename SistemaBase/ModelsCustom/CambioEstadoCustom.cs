using SistemaBase.Controllers;

namespace SistemaBase.ModelsCustom
{
    public partial class CambioEstadoCustom
    {
        public decimal? NumeroEntrada { get; set; }
        public decimal? EstadoEntrada { get; set; }
        public decimal? NroMovimiento { get; set; }
        public DateTime? FechaOperacion { get; set; }
        public string? NroDocPresentador { get; set; }
        public string? NomPresentador { get; set; }
        public string? NroDocRetirador { get; set; }
        public string? NomRetirador { get; set; }
        public string? NroLiquidacion { get; set; }
        public decimal? NroFormulario { get; set; }
        public decimal? NuevoEstado { get; set; }
        public string? NroDocTitular { get; set; }
        public string? NomTitular { get; set; }
        public decimal? TipoSolicitud { get; set; }
        public string? Observacion { get; set; }
        public decimal? CodigoOficina { get; set; }
        public decimal? CodOficinaRetiro { get; set; }

    }
}
