using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SistemaBase.ModelsCustom
{

    public class EntregaTrabajoArchivoCustom
    {
        [Key]
        [DisplayName("Titulo de Propieda Nro")]
        [JsonPropertyName("NUMERO_BOLETA")]
        public string? NUMERO_BOLETA { get; set; }
        [DisplayName("Numero Entrada")]
        [JsonPropertyName("NUMERO_ENTRADA")]
        public int NUMERO_ENTRADA { get; set; }
        [DisplayName("Tipo de Solicitud")]
        [JsonPropertyName("TIPO_SOLICITUD")]
        public string? TIPO_SOLICITUD { get; set; }
        [DisplayName("Fecha Asignacion")]
        [JsonPropertyName("FECHA_ASIGNACION")]
        public DateTime? FECHA_ASIGNACION { get; set; }
        [JsonPropertyName("NOM_TITULAR")]
        public string? NOM_TITULAR { get; set; }
    }
}