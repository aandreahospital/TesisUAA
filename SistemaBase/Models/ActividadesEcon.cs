using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class ActividadesEcon
    {
        public ActividadesEcon()
        {
            ActivPersonas = new HashSet<ActivPersona>();
        }
        public string CodActividad { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? Abreviatura { get; set; }
        public Guid Rowid { get; set; }
        public virtual ICollection<ActivPersona> ActivPersonas { get; set; }
    }
}
