using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class Cargo
    {
        public int Idcargo { get; set; }
        public string CodUsuario { get; set; } = null!;
        public string? Lugartrabajo { get; set; }
        public Guid Rowid { get; set; }

        public virtual Usuario CodUsuarioNavigation { get; set; } = null!;
    }
}
