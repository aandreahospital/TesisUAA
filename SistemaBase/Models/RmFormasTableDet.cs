using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class RmFormasTableDet
    {
        public string NombForma { get; set; } = null!;
        public decimal NroDetalle { get; set; }
        public string? NombTable { get; set; }
        public virtual RmFormasTableCab NombFormaNavigation { get; set; } = null!;
    }
}
