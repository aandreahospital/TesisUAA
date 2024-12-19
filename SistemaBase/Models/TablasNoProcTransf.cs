using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class TablasNoProcTransf
    {
        public string? NomTabla { get; set; }
        public string? Operacion { get; set; }
        public Guid Rowid { get; set; }
    }
}
