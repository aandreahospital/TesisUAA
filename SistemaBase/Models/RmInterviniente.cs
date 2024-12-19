using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class RmInterviniente
    {
        //public RmInterviniente()
        //{
        //    RmTransacciones = new HashSet<RmTransaccione>();
        //}
        public decimal IdProfesional { get; set; }
        public string? Nombre { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? TipoInterviniente { get; set; }
        //public virtual ICollection<RmTransaccione> RmTransacciones { get; set; }
    }
}
