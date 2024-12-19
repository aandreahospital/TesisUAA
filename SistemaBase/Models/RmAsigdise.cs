using System.Text.Json.Serialization;

namespace SistemaBase.Models
{
    public class RmAsigdise
    {
        [JsonPropertyName("Id")]
        public string? Id { get; set; }
        [JsonPropertyName("NombreUsuario")]
        public string? NombreUsuario { get; set; }
        [JsonPropertyName("DescripcionGrupo")]
        public string? DescripcionGrupo { get; set; }
        [JsonPropertyName("Estado")]
        public string? Estado { get; set; }
        [JsonPropertyName("Cantidad")]
        public int? Cantidad { get; set; }
        [JsonPropertyName("NumeroEntrada")]
        public decimal? NumeroEntrada { get; set; }
    }
}
