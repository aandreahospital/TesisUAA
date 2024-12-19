using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class Vestab
    {
        public string CodEstable { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string CodPropietario { get; set; } = null!;
        public string? Nombre { get; set; }
        public string? Distrito { get; set; }
    }
}
