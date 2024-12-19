using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class Dpto
    {
        public double CodDpto { get; set; }
        public string? NombreDpto { get; set; }
        public string CodPais { get; set; } = null!;
        public Guid Rowid { get; set; }
    }
}
