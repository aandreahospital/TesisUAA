using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaBase.Models
{
    public partial class Identificacione
    {
        /*public Identificacione()
        {
            IdentPersonas = new HashSet<IdentPersona>();
        }*/
        public string CodIdent { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? Mascara { get; set; }
        public string? PersonaFisica { get; set; }
        public string? Abreviatura { get; set; }
        [ScaffoldColumn(false)]
        public Guid Rowid { get; set; }
        public virtual ICollection<IdentPersona>? IdentPersonas { get; set; }
    }
}
