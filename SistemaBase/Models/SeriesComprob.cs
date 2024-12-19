using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class SeriesComprob
    {
        public string CodEmpresa { get; set; } = null!;
        public string TipComprobante { get; set; } = null!;
        public string SerComprobante { get; set; } = null!;
        public string? SerArticulo { get; set; }
        public decimal? NumInicial { get; set; }
        public decimal? NumFinal { get; set; }
        public string? Exento { get; set; }
        public Guid Rowid { get; set; }
    }
}
