using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class Carrera
    {
        public int Idcarrera { get; set; }
        public string? Descripcion { get; set; }
        public Guid Rowid { get; set; }
    }
}
