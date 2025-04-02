using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class Comentario
    {
        public int IdComentario { get; set; }
        public string CodUsuario { get; set; } = null!;
        public int IdForoDebate { get; set; }
        public string? Descripcion { get; set; }
        public DateTime? FechaComentario { get; set; } 

        //public virtual Usuario CodUsuarioNavigation { get; set; } = null!;
        /// public virtual ForoDebate IdForoDebateNavigation { get; set; } = null!;
    }
}
