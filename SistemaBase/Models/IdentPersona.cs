using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class IdentPersona
    {
        public string CodPersona { get; set; } = null!;
        public string CodIdent { get; set; } = null!;
        public string? Numero { get; set; }
        public DateTime? FecVencimiento { get; set; }
        public Guid Rowid { get; set; }
        public virtual Identificacione CodIdentNavigation { get; set; } = null!;
    }
}
