using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace SistemaBase.ModelsCustom
{
    public partial class MesaEntradaCustom
    {
        [JsonPropertyName("NumeroEntrada")]
        public Decimal? NumeroEntrada { get; set; }
        [JsonPropertyName("Descripcion")]
        public String? Descripcion { get; set; }
        [JsonPropertyName("Fecha")]
        public DateTime? Fecha { get; set; }
    }
        
}
