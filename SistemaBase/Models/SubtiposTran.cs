using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class SubtiposTran
    {
        public string CodEmpresa { get; set; } = null!;
        public string CodModulo { get; set; } = null!;
        public decimal TipoTrans { get; set; }
        public string SubtipoTrans { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? UsaDinero { get; set; }
        public string? Concepto { get; set; }
        public string? CargaValores { get; set; }
        public string? VerificaValores { get; set; }
        public string? AfectaImponible { get; set; }
        public string? CodAgrupacion { get; set; }
        public string? TipDocumento { get; set; }
        public string? CargaDeposito { get; set; }
        public string? CargaOtros { get; set; }
        public string? AfectaIva { get; set; }
        public string? AfectaRenta { get; set; }
        public string? GastoDespacho { get; set; }
        public string? CodCuenta { get; set; }
        public string? Abreviatura { get; set; }
        public string? AfectaAsiento { get; set; }
        public Guid Rowid { get; set; }
        public virtual Empresa CodEmpresaNavigation { get; set; } = null!;
        public virtual Modulo CodModuloNavigation { get; set; } = null!;
        public virtual TiposTran TiposTran { get; set; } = null!;
    }
}
