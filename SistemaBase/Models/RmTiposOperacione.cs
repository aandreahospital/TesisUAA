using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaBase.Models
{
    public partial class RmTiposOperacione
    {
        public decimal TipoOperacion { get; set; }
        public string? DescripTipoOperacion { get; set; }

        [ScaffoldColumn(false)]
        public Guid Rowid { get; set; }
    }
}
