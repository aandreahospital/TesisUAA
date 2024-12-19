using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class MalasReferencia
    {
        public string? CodPersona { get; set; }
        public string? CodMotivo { get; set; }
        public DateTime? FecIngreso { get; set; }
        public DateTime? FecVencimi { get; set; }
        public string? CodBanco { get; set; }
        public decimal? Monto { get; set; }
        public Guid Rowid { get; set; }
    }
}
