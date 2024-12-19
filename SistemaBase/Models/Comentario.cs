using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class Comentario
    {
        public int Idcomentario { get; set; }
        public string CodUsuario { get; set; } = null!;
        public int ForodebateIdforodebate { get; set; }
        public string? Descripcion { get; set; }
        public DateTime? Fechacomentario { get; set; }
        public Guid Rowid { get; set; }

        public virtual Usuario CodUsuarioNavigation { get; set; } = null!;
        public virtual Forodebate ForodebateIdforodebateNavigation { get; set; } = null!;
    }
}
