using System.Text.Json.Serialization;

namespace SistemaBase.ModelsCustom
{
    public partial class BuscadorCustom
    {
        [JsonPropertyName("NomTitular")]
        public String? NomTitular { get; set; }

        [JsonPropertyName("NroDocTitular")]
        public String? NroDocTitular { get; set; }

        [JsonPropertyName("TipoSolicitud")]
        public String? TipoSolicitud { get; set; }

        [JsonPropertyName("NumeroEntrada")]
        public Decimal? NumeroEntrada { get; set; }
        [JsonPropertyName("TituloPropiedad")]
        public string? NroBoleta { get; set; }

        //[JsonPropertyName("Fecha")]
        //public DateTime? Fecha { get; set; }
    }
}
