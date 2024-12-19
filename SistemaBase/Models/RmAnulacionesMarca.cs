using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaBase.Models
{
    public partial class RmAnulacionesMarca
    {
        public decimal? IdMotivoAnulacion { get; set; }
        public string? IdUsuario { get; set; }
        public DateTime? FechaAlta { get; set; }
        public Guid Rowid { get; set; }
        public decimal IdMarca { get; set; }

        public virtual RmMotivosAnulacion? IdMotivoAnulacionNavigation { get; set; }

        //[Display(Name = "ID")]
        //public decimal IdMarca { get; set; }

        //[Display(Name ="Motivo Anulacion")]
        //public decimal? IdMotivoAnulacion { get; set; }

        //[Display(Name ="Usuario")]
        //public string? IdUsuario { get; set; }

        //[Display(Name ="Fecha de Alta")]
        //public DateTime? FechaAlta { get; set; }
        //public Guid Rowid { get; set; }
        //public virtual RmMotivosAnulacion? IdMotivoAnulacionNavigation { get; set; }
        //public virtual RmUsuario? IdUsuarioNavigation { get; set; }
    }
}
