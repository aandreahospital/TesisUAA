using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class Pai
    {
        public string CodPais { get; set; } = null!;
        public string? NombrePais { get; set; }
        public Guid Rowid { get; set; }
    }
}
