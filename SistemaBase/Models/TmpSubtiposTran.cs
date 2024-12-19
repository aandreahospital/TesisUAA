using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class TmpSubtiposTran
    {
        public string? CodEmpresa { get; set; }
        public string? CodModulo { get; set; }
        public decimal? TipoTrans { get; set; }
        public string? SubtipoTrans { get; set; }
        public string? Descripcion { get; set; }
        public string? UsaDinero { get; set; }
        public string? Concepto { get; set; }
        public string? CargaValores { get; set; }
        public string? VerificaValores { get; set; }
        public Guid Rowid { get; set; }
    }
}
