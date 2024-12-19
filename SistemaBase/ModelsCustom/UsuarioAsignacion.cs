using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaBase.Models;
using System.Text.Json.Serialization;

namespace SistemaBase.ModelsCustom
{
    public partial class UsuarioAsignacion
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
