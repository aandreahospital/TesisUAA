using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaBase.Models
{
    public partial class Ciudade
    {
        public string CodPais { get; set; } = null!;
        public string CodProvincia { get; set; } = null!;
        public string CodCiudad { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? Abreviatura { get; set; }
        public string? CodZona { get; set; }
        public string? CodRegional { get; set; }
    }
}
