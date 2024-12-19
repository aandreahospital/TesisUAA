using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class TiposTalonario
    {
        public string? TipTalonario { get; set; }
        public string? Descripcion { get; set; }
        public Guid Rowid { get; set; }
    }
}
