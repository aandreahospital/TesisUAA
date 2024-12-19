using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class AreasUsuario
    {
        public string CodEmpresa { get; set; } = null!;
        public string CodArea { get; set; } = null!;
        public string CodUsuario { get; set; } = null!;
        public string? Activo { get; set; }
        public Guid Rowid { get; set; }
    }
}
