using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class AvZona
    {
        public string CodZona { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string CodDepartamento { get; set; } = null!;
        public string CodPais { get; set; } = null!;
        public Guid Rowid { get; set; }
    }
}
