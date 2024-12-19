using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class NivelEstudio
    {
        public string CodNivel { get; set; } = null!;
        public string? Descripcion { get; set; }
        public Guid Rowid { get; set; }
    }
}
