using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class Deptos1
    {
        public double IdDepto1 { get; set; }
        public string? DescDepto1 { get; set; }
        public double? IdOficina { get; set; }
        public Guid Rowid { get; set; }
        public virtual Oficina? IdOficinaNavigation { get; set; }
    }
}
