using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class Moneda
    {
        public string CodMoneda { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? Siglas { get; set; }
        public decimal? TipoCambioDia { get; set; }
        public string? CodPais { get; set; }
        public decimal? Decimales { get; set; }
        public DateTime? FecTipoCambio { get; set; }
        public decimal? TipoCambio { get; set; }
        public decimal? TipoCambioCompra { get; set; }
        public decimal? PorcInteres { get; set; }
        public decimal? TipoCambioContado { get; set; }
        public decimal? TipoCambioCredito { get; set; }
        public Guid Rowid { get; set; }
        public virtual Paise? CodPaisNavigation { get; set; }
    }
}
