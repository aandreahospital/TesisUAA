using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class RmConceptosLiquidacione
    {
        public string? IdConcepto { get; set; }
        public string? DescripcionConcepto { get; set; }
        public string? TipoConcepto { get; set; }
        public decimal? MontoConcepto { get; set; }
        public string? TipoSolicitud { get; set; }
    }
}
