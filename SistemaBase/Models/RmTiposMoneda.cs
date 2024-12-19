using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaBase.Models
{
    public partial class RmTiposMoneda
    {
        public decimal TipoMoneda { get; set; }
        public string? DescripTipoMoneda { get; set; }

        [ScaffoldColumn(false)]
        public Guid Rowid { get; set; }

        //public virtual ICollection<RmLevantamiento>? RmLevantamientos { get; set; }
        public virtual ICollection<RmMedidasPrenda>? RmMedidasPrenda { get; set; }

    }
}
