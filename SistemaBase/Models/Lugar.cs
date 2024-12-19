using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class Lugar
    {
        public double IdLugar { get; set; }
        public string? Nombre { get; set; }
        public Guid Rowid { get; set; }
    }
}
