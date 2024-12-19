using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class SectoresEcon
    {
        public string CodSector { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal? Comision { get; set; }
        public Guid Rowid { get; set; }
    }
}
