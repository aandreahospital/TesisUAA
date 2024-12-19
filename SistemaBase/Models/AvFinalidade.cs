using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class AvFinalidade
    {
        public string CodFinalidad { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? Estado { get; set; }
        public string? UsoEstablecimiento { get; set; }
        public string? UsoCota { get; set; }
        public Guid Rowid { get; set; }
    }
}
