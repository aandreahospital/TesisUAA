using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class RmLiquidacionesMarca
    {
        public decimal IdLiquidacion { get; set; }
        public DateTime? FechaUtilizacion { get; set; }
        public decimal? Actualizado { get; set; }
        public decimal? Utilizado { get; set; }
    }
}
