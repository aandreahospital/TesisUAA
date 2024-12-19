using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class RmEstadosEntradum
    {
        public decimal CodigoEstado { get; set; }
        public string? DescripEstado { get; set; }
        public Guid Rowid { get; set; }
    }
}
