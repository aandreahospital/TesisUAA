using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class RmOperacionesSist
    {
       public RmOperacionesSist()
        {
            RmCambiosEstados = new HashSet<RmCambiosEstado>();
        }
        public string CodOperacion { get; set; } = null!;
        public string? DescOperacion { get; set; }
        public Guid Rowid { get; set; }
       public virtual ICollection<RmCambiosEstado> RmCambiosEstados { get; set; }
    }
}
