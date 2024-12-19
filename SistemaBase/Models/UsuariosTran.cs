using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class UsuariosTran
    {
        public string CodUsuario { get; set; } = null!;
        public string CodModulo { get; set; } = null!;
        public decimal TipoTrans { get; set; }
        public Guid Rowid { get; set; }
        public virtual Modulo CodModuloNavigation { get; set; } = null!;
    }
}
