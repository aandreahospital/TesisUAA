using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class NumeroLetra
    {
        public decimal Numero { get; set; }
        public string? Letras { get; set; }
        public Guid Rowid { get; set; }
    }
}
