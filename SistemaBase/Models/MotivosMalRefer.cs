using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class MotivosMalRefer
    {
        public string CodMotivo { get; set; } = null!;
        public string? Descripcion { get; set; }
        public Guid Rowid { get; set; }
    }
}
