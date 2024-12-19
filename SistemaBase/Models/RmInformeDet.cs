using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class RmInformeDet
    {
        public decimal NroDetalle { get; set; }
        public decimal IdInforme { get; set; }
        public string? IdBeneficiario { get; set; }
        public virtual Persona? IdBeneficiarioNavigation { get; set; }
        public virtual RmInforme? IdInformeNavigation { get; set; } = null!;
    }
}
