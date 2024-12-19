using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class Profesione
    {
        public string CodProfesion { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? Abreviatura { get; set; }
        public string? Siglas { get; set; }
        public Guid Rowid { get; set; }
    }
}
