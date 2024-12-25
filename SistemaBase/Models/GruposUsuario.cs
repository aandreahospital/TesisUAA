using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class GruposUsuario
    {
        public GruposUsuario()
        {
            AccesosGrupos = new HashSet<AccesosGrupo>();
            Usuarios = new HashSet<Usuario>();
        }

        public string CodGrupo { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? OpFueraHo { get; set; }
        public Guid Rowid { get; set; }

        public virtual ICollection<AccesosGrupo> AccesosGrupos { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
