using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class GruposUsuario
    {
        public GruposUsuario()
        {
            Usuarios = new HashSet<Usuario>();
        }

        public string CodGrupo { get; set; } = null!;
        public string? Descripcion { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
