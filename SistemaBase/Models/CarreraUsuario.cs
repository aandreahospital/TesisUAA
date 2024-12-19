using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class CarreraUsuario
    {
        public int Idcarrera { get; set; }
        public string CodUsuario { get; set; } = null!;
        public string? Promo { get; set; }
        public Guid Rowid { get; set; }

        public virtual Usuario CodUsuarioNavigation { get; set; } = null!;
    }
}
