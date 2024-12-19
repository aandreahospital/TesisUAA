using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SistemaBase.ModelsCustom
{
    public partial class RecepcionEntradas
    {
        [Key]

      
        [JsonPropertyName("NroEntrada")]
        public decimal NroEntrada { get; set; }

        [DisplayName("Id Tipo Solicitud")]
        public decimal? TipoSolicitud { get; set; }

        [JsonPropertyName("DescSolicitud")]
        public string? DescSolicitud { get; set; }

        [JsonPropertyName("DescOficina")]
        public string? DescOficina { get; set; }

   
        [JsonPropertyName("FechaEntrada")]
        public DateTime? FechaEntrada { get; set; }
    }
}