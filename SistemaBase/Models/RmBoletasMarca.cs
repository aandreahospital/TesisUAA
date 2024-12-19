using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaBase.Models
{
    public partial class RmBoletasMarca
    {
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal NroBoleta { get; set; }
        public string Descripcion { get; set; } = null!;
        public string? Asignado { get; set; }
    }
}
