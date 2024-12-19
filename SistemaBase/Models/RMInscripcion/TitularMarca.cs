using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SistemaBase.Models.RMInscripcion
{
    public class TitularMarca
    {
        // Properties
        [JsonPropertyName("CodPersona")]
        public string CodPersona { get; set; }
        [JsonPropertyName("Nombre")]
        public String Nombre { get; set; }
        [JsonPropertyName("EsPropietario")]
        public bool EsPropietario { get; set; }
    }
}
