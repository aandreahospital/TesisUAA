using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaBase.Models
{
    public partial class RmMotivosAnulacion
    {
        //public RmMotivosAnulacion()
        //{
        //    RmAnulacionesMarcas = new HashSet<RmAnulacionesMarca>();
        //    //RmMesaEntrada = new HashSet<RmMesaEntradum>();
        //}

        [Display(Name ="ID")]
        public decimal IdMotivoAnulacion { get; set; }

        [Display(Name = "Descripcion")]
        public string? DescripMotivo { get; set; }

        [ScaffoldColumn(false)]
        public Guid Rowid { get; set; }
        public virtual ICollection<RmAnulacionesMarca>? RmAnulacionesMarcas { get; set; }
        //public virtual ICollection<RmMesaEntradum>? RmMesaEntrada { get; set; }
    }
}
