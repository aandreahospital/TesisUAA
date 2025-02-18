using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class Cargo
    {
        public int IdCargo { get; set; }
        public string CodUsuario { get; set; } = null!;
        public string? Descripcion { get; set; }

        public virtual Usuario CodUsuarioNavigation { get; set; } = null!;
    }
}
