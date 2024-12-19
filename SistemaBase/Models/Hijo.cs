using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class Hijo
    {
        public string? CodPersona { get; set; }
        public string? Nombre { get; set; }
        public DateTime? FecNacimiento { get; set; }
        public string? ColegioUniversidad { get; set; }
        public Guid Rowid { get; set; }
    }
}
