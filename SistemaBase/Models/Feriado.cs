using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class Feriado
    {
        public DateTime Feriado1 { get; set; }
        public string? Descripcion { get; set; }
        public Guid Rowid { get; set; }
    }
}
