using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class CmProvee
    {
        public string? CodProveedor { get; set; }
        public string? Descripcion { get; set; }
        public string? CodEmpresa { get; set; }
        public Guid Rowid { get; set; }
    }
}
