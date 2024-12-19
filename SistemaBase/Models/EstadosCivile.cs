using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaBase.Models
{
    public partial class EstadosCivile
    {
        public string CodEstadoCivil { get; set; } = null!;
        public string? Descripcion { get; set; }
        [ScaffoldColumn(false)]
        public Guid Rowid { get; set; }
    }
}
