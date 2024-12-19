using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class ProvisorioCab
    {
        public string? CodEmpresa { get; set; }
        public string? CodEjercicio { get; set; }
        public decimal? NumAsiento { get; set; }
        public string? CodSucursal { get; set; }
        public string? CodModulo { get; set; }
        public DateTime? FecAsiento { get; set; }
        public string? Descripcion { get; set; }
        public string? EjercicioCerrado { get; set; }
        public string? IndMayorizado { get; set; }
        public string? TipAsiento { get; set; }
        public Guid Rowid { get; set; }
    }
}
