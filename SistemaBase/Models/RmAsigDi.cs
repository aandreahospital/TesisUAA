using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SistemaBase.Models
{
    public partial class RmAsigDi
    {
        [JsonPropertyName("NroAsignacion")]
        public decimal NroAsignacion { get; set; }
        [JsonPropertyName("NroEntrada")]
        public decimal NroEntrada { get; set; }
        [JsonPropertyName("FechaAsignada")]
        public DateTime? FechaAsignada { get; set; }
        [JsonPropertyName("IdUsuarioAsignado")]
        public string? IdUsuarioAsignado { get; set; }
        [JsonPropertyName("Recibido")]
        public string? Recibido { get; set; }
        [JsonPropertyName("IdUsuarioAlta")]
        public string? IdUsuarioAlta { get; set; }
        [JsonPropertyName("NroAsignacionNueva")]
        public decimal? NroAsignacionNueva { get; set; }
        [JsonPropertyName("RepId")]
        public decimal? RepId { get; set; }
        [JsonPropertyName("Desasignado")]
        public string? Desasignado { get; set; }
        [JsonPropertyName("FechaDesasignacion")]
        public DateTime? FechaDesasignacion { get; set; }
        [JsonPropertyName("UsuarioDesasignacion")]
        public string? UsuarioDesasignacion { get; set; }
        [JsonPropertyName("Rowid")]
        public Guid? Rowid { get; set; }
    }
}
