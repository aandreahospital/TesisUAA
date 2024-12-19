using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class SegurosPersona
    {
        public string? CodPersona { get; set; }
        public string? CompaniaAseguradora { get; set; }
        public string? NomBeneficiario { get; set; }
        public string? RiesgosCubiertos { get; set; }
        public DateTime? FecVencimiento { get; set; }
        public decimal? MontoSeguro { get; set; }
        public Guid Rowid { get; set; }
    }
}
