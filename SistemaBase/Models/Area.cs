using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class Area
    {
        public string CodEmpresa { get; set; } = null!;
        public string CodArea { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? Activo { get; set; }
        public Guid Rowid { get; set; }
    }
}
