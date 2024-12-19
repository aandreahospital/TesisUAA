using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class RmNotasNegativa
    {
        public decimal IdEntrada { get; set; }
        public string? DescripNotaNegativa { get; set; }
        public string? IdUsuario { get; set; }
        public DateTime? FechaAlta { get; set; }
        public Guid Rowid { get; set; }
    }
}
