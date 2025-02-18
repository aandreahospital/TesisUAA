using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class CarreraUsuario
    {
        public int IdCarrera { get; set; }
        public string CodUsuario { get; set; } = null!;
        public string? Promo { get; set; }

        public virtual Usuario CodUsuarioNavigation { get; set; } = null!;
    }
}
