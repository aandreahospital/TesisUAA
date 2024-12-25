using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class RmAutorizante
    {
        public decimal? MatriculaRegistro { get; set; }
        public string? DescripAutorizante { get; set; }
        public string? TipoAutorizante { get; set; }
        public string? CodCiudad { get; set; }
        public Guid Rowid { get; set; }
        public decimal IdAutorizante { get; set; }
    }
}
