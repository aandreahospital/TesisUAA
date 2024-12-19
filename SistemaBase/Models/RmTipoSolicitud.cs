using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaBase.Models
{
    public partial class RmTipoSolicitud
    {

        [ScaffoldColumn(false)]
        public decimal TipoSolicitud { get; set; }

        [Display(Name = "Descripcion")]
        public string? DescripSolicitud { get; set; }

        [Display(Name = "Bloqueo de Entrada:")]
        public string? BloqueaEntrada { get; set; }

        [ScaffoldColumn(false)]
        public Guid Rowid { get; set; }
        //public virtual ICollection<RmMesaEntradum>? RmMesaEntrada { get; set; }
    }
}
