using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class Talonario
    {
        public string CodEmpresa { get; set; } = null!;
        public string TipTalonario { get; set; } = null!;
        public decimal NroTalonario { get; set; }
        public string? CodSucursal { get; set; }
        public string? Serie { get; set; }
        public decimal? NumeroInicial { get; set; }
        public decimal? NumeroFinal { get; set; }
        public string? Activo { get; set; }
        public string? Imprime { get; set; }
        public string? CodSector { get; set; }
        public string? TipImpresion { get; set; }
        public Guid Rowid { get; set; }
    }
}
