using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class AvTipoEstablecimiento
    {
        public string CodTipoEstab { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? Estado { get; set; }
        public Guid Rowid { get; set; }
    }
}
