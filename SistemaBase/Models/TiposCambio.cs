using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class TiposCambio
    {
        public string CodMoneda { get; set; } = null!;
        public string TipoCambio { get; set; } = null!;
        public DateTime FecTipoCambio { get; set; }
        public decimal? ValVenta { get; set; }
        public decimal? ValCompra { get; set; }
        public string? AltaPor { get; set; }
        public DateTime? FecAlta { get; set; }
        public string? ActualizadoPor { get; set; }
        public DateTime? FecActualizado { get; set; }
        public decimal? TipoCambioContado { get; set; }
        public decimal? TipoCambioCredito { get; set; }
        public Guid Rowid { get; set; }
    }
}
