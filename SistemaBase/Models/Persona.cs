using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class Persona
    {
        public string CodPersona { get; set; } = null!;
        public string? Nombre { get; set; }
        public string? DireccionParticular { get; set; }
        public string? Sexo { get; set; }
        public DateTime? FecNacimiento { get; set; }
        public DateTime? FecAlta { get; set; }
        public string? AltaPor { get; set; }
        public DateTime? FecActualizacion { get; set; }
        public string? ActualizadoPor { get; set; }
       
        public Guid Rowid { get; set; }
        public string? Email { get; set; }
        public string? SitioWeb { get; set; }
    }
}
