using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class Forodebate
    {
        //public Forodebate()
        //{
        //    Comentarios = new HashSet<Comentario>();
        //}

        public int Idforodebate { get; set; }
        public string CodUsuario { get; set; } = null!;
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public string? Estado { get; set; }
        public Guid Rowid { get; set; }

        public virtual Usuario CodUsuarioNavigation { get; set; } = null!;
        ///public virtual ICollection<Comentario> Comentarios { get; set; }
    }
}
