using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class RmCertificacionesDet
    {
        public decimal NroDetalle { get; set; }
        public decimal IdCertificacion { get; set; }
        public string? IdBeneficiario { get; set; }
        public virtual RmCertificacione? IdCertificacionNavigation { get; set; }
        
        public virtual Persona? IdBeneficiarioNavigation { get; set; }
    }
}
