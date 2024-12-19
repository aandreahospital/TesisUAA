using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class HistDocumento
    {
        public string? CodEmpresa { get; set; }
        public string? CodSucursal { get; set; }
        public string? TipDocumento { get; set; }
        public string? SerDocumento { get; set; }
        public decimal? NroDocumento { get; set; }
        public DateTime? FecDocumento { get; set; }
        public string? CodUsuario { get; set; }
        public DateTime? FecCarga { get; set; }
        public string? EstadoAnt { get; set; }
        public string? EstadoAct { get; set; }
        public string? CodDepartamentoEnt { get; set; }
        public string? CodFuncionarioEnt { get; set; }
        public string? CodDepartamentoSal { get; set; }
        public string? CodFuncionarioSal { get; set; }
        public string? Borrado { get; set; }
        public string? BorroUsuario { get; set; }
        public DateTime? FecBorrado { get; set; }
        public Guid Rowid { get; set; }
    }
}
