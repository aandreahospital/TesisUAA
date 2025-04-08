using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class Forma
    {
        //public Forma()
        //{
        //    AccesosGrupos = new HashSet<AccesosGrupo>();
        //}

        public string CodModulo { get; set; } = null!;
        public string NomForma { get; set; } = null!;
        public string? Descripcion { get; set; }

        public virtual Modulo CodModuloNavigation { get; set; } = null!;
      //  public virtual ICollection<AccesosGrupo> AccesosGrupos { get; set; }
    }
}
