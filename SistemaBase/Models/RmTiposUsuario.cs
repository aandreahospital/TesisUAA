using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaBase.Models
{
    public partial class RmTiposUsuario
    {
        [ScaffoldColumn(false)]
        public decimal IdTipoUsuario { get; set; }
        public string? DescripcionTipoUsuario { get; set; }
        public string? CodigoTipoUsuario { get; set; }

        [ScaffoldColumn(false)]
        public Guid Rowid { get; set; }
    }
}
