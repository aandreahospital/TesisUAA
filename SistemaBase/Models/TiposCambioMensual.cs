using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class TiposCambioMensual
    {
        public string Año { get; set; } = null!;
        public string Mes { get; set; } = null!;
        public string CodMoneda { get; set; } = null!;
        public decimal? TipCambio { get; set; }
        public Guid Rowid { get; set; }
    }
}
