using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class RmMarcaSenalDistrito
    {
        public decimal IdMarca { get; set; }
        public string? CodPais { get; set; }
        public string? CodProvincia { get; set; }
        public string? CodCiudad { get; set; }
        public string? CodigoSenal { get; set; }
    }
}
