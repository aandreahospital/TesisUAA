using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class Modulo
    {
        public Modulo()
        {
            AccesosGrupos = new HashSet<AccesosGrupo>();
            Formas = new HashSet<Forma>();
        }

        public string CodModulo { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? ManejaCalendario { get; set; }
        public string? ManejaCierre { get; set; }
        public Guid Rowid { get; set; }

        public virtual ICollection<AccesosGrupo> AccesosGrupos { get; set; }
        public virtual ICollection<Forma> Formas { get; set; }
    }
}
