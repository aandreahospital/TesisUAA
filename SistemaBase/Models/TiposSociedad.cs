using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class TiposSociedad
    {

        public string TipoSociedad { get; set; } = null!;
        public string? Descripcion { get; set; }
        public Guid Rowid { get; set; }
    }
}
