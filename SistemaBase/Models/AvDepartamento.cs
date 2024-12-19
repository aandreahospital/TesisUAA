using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class AvDepartamento
    {
        public string CodDepartamento { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public Guid Rowid { get; set; }
    }
}
