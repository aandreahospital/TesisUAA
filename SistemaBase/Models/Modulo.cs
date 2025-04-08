using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class Modulo
    {
        public Modulo()
        {
            Formas = new HashSet<Forma>();
        }

        public string CodModulo { get; set; } = null!;
        public string? Descripcion { get; set; }

        public virtual ICollection<Forma> Formas { get; set; }
    }
}
