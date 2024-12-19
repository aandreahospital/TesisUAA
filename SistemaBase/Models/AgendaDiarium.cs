using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class AgendaDiarium
    {
        public decimal? NroActividad { get; set; }
        public string? CodUsuario { get; set; }
        public DateTime? FecActividad { get; set; }
        public string? Detalle { get; set; }
        public string? IndRealizado { get; set; }
        public string? CodUsuarioAlta { get; set; }
        public DateTime? FecAlta { get; set; }
        public DateTime? HoraActividad { get; set; }
        public string? IndEnviaMail { get; set; }
        public string? Subject { get; set; }
        public string? CodGrupo { get; set; }
        public DateTime? FecUltimaActualizacion { get; set; }
    }
}
