using System.ComponentModel.DataAnnotations;

namespace SistemaBase.ModelsCustom
{
    public class AnulacionEntrada
    {
        [Key]
        public decimal NroEntrada { get; set; }
        public DateTime? FechaAnulacion { get; set; }
        public decimal? NroFormulario { get; set; }
        public decimal? CodOficina { get; set; }
        public string? DescOficina { get; set; }
        public decimal? TipoSolicitud { get; set; }
        public string? DescSolicitud { get; set; }
        public decimal? EstadoEntrada { get; set; }
        public string? DescEstado { get; set; }
        public string? NombreTit { get; set; }
        public decimal? IdMotivo { get; set; }
        public string? DescMotivo { get; set; }

    }
}

