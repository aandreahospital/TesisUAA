using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class RmFormasTableCab
    {
        public RmFormasTableCab()
        {
            RmFormasTableDets = new HashSet<RmFormasTableDet>();
        }
        public string NombForma { get; set; } = null!;
        public string? Descripcion { get; set; }
        public virtual ICollection<RmFormasTableDet> RmFormasTableDets { get; set; }
    }
}
