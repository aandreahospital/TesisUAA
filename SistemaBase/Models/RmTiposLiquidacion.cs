using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class RmTiposLiquidacion
    {
        public decimal IdTipoLiquidacion { get; set; }
        public string DescripcionTipoLiquidacion { get; set; } = null!;
        public decimal IdConceptoLiquidacion { get; set; }
    }
}
