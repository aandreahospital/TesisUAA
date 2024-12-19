using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
namespace SistemaBase.Models
{
    public partial class GruposUsuario
    {
        public GruposUsuario()
        {
            AccesosGrupos = new HashSet<AccesosGrupo>();
            Usuarios = new HashSet<Usuario>();
        }
        [Display(Name = "Codigo")]
        public string CodGrupo { get; set; } = null!;
        public string? Descripcion { get; set; }
        [ScaffoldColumn(false)]
        public string? OpFueraHo { get; set; }
        [ScaffoldColumn(false)]
        public Guid Rowid { get; set; }
        public virtual ICollection<AccesosGrupo>? AccesosGrupos { get; set; }
        public virtual ICollection<Usuario>? Usuarios { get; set; }
    }
}
