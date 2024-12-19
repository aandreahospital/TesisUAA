using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaBase.Models
{
    public partial class RmTiposDocumento
    {
        [ScaffoldColumn(false)]
        [Display(Name = "ID")]
        public decimal TipoDocumento { get; set; }

        [Display(Name = "Descripcion")]
        public string? DescripTipoDocumento { get; set; }

        [ScaffoldColumn(false)]
        public Guid Rowid { get; set; }
    }
}
