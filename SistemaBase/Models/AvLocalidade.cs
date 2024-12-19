using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class AvLocalidade
    {
        public string? Descripcion { get; set; }
        public string CodLocalidad { get; set; } = null!;
        public string CodDistrito { get; set; } = null!;
        public string? UltimoEstablecimiento { get; set; }
        public decimal? Rownumb { get; set; }
        public Guid Rowid { get; set; }
    }
}
