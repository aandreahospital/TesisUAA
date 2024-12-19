using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class ActivPersona
    {
        public string CodActividad { get; set; } = null!;
        public string CodPersona { get; set; } = null!;
        public Guid Rowid { get; set; }
        public virtual ActividadesEcon CodActividadNavigation { get; set; } = null!;
    }
}
