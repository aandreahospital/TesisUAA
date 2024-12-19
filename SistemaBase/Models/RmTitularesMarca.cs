using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class RmTitularesMarca
    {
        public string? IdTitular { get; set; }
        public string? IdPropietario { get; set; }
        public decimal? IdMarca { get; set; }
        public decimal? IdTransaccion { get; set; }
        public decimal? PorcentajeTitularidad { get; set; }
        public decimal? EstadoTitularidad { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string? EsPropietario { get; set; }
        public string? CodUsuario { get; set; }
        public string? IdUsuario { get; set; }
        public decimal? IdTipoPropiedad { get; set; }
        public Guid Rowid { get; set; }
        public decimal IdTm { get; set; }
    }
}
