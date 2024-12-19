using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class Oficina
    {
        public Oficina()
        {
            Deptos1s = new HashSet<Deptos1>();
        }
        public double IdOfi { get; set; }
        public string? DescOfi { get; set; }
        public Guid Rowid { get; set; }
        public virtual ICollection<Deptos1> Deptos1s { get; set; }
    }
}
