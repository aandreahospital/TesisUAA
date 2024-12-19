using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaBase.Models
{
    public partial class RmOficinasRegistrale
    {
        //public RmOficinasRegistrale()
        //{
        //    RmMesaEntrada = new HashSet<RmMesaEntradum>();
        //    RmUsuarios = new HashSet<RmUsuario>();
        //}
        [ScaffoldColumn(false)]
        [Display(Name = "Codigo Oficina:")]
        [DisplayFormat(DataFormatString = "{0:F0}", ApplyFormatInEditMode = true)]

        public decimal CodigoOficina { get; set; }

        [Display(Name = "Descripcion:")]
        public string? DescripOficina { get; set; }
        [ScaffoldColumn(false)]
        public Guid Rowid { get; set; }
        //public virtual ICollection<RmMesaEntradum>? RmMesaEntrada { get; set; }
        public virtual ICollection<RmUsuario>? RmUsuarios { get; set; }
    }
}
