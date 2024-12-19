using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class TelefPersona
    {
        public string CodPersona { get; set; } = null!;
        public string CodigoArea { get; set; } = null!;
        public string NumTelefono { get; set; } = null!;
        public string? TipTelefono { get; set; }
        public string? TelUbicacion { get; set; }
        public decimal? Interno { get; set; }
        public string? Nota { get; set; }
        public string? PorDefecto { get; set; }
        public string? CodDireccion { get; set; }
        public Guid Rowid { get; set; }
       // public virtual DirecPersona? Cod { get; set; }
    }
}
