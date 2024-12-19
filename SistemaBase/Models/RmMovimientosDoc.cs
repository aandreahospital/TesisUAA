using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class RmMovimientosDoc
    {
        public decimal? NroEntrada { get; set; }
        public string? CodUsuario { get; set; }
        public DateTime? FechaOperacion { get; set; }
        public string? CodOperacion { get; set; }
        public string? NroMovimientoRef { get; set; }
        public decimal? EstadoEntrada { get; set; }
        public decimal NroMovimiento { get; set; }
    }
}
