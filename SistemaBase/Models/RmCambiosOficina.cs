using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class RmCambiosOficina
    {
        public decimal NroMovimiento { get; set; }
        public string? NroEntrada { get; set; }
        public string? CodUsuario { get; set; }
        public decimal? OficRetiroAnt { get; set; }
        public DateTime? FechaOperacion { get; set; }
        public string? Observacion { get; set; }
        public decimal? OficRetiroNuev { get; set; }
        public string? CodOperacion { get; set; }
        public Guid Rowid { get; set; }

       // public virtual RmOperacionesSist? CodOperacionNavigation { get; set; }
    }
}
