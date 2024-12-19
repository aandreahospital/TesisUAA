using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class Auditorium
    {
        public string? CodUsuario { get; set; }
        public string? CodModulo { get; set; }
        public string? Operacion { get; set; }
        public DateTime? FecOcurrencia { get; set; }
        public string? DatosViejos { get; set; }
        public string? DatosNuevos { get; set; }
        public string? NomTabla { get; set; }
        public decimal? NroOperacion { get; set; }
        public string? CodEmpresa { get; set; }
        public string? Columnas { get; set; }
        public string? Transferido { get; set; }
        public DateTime? FecTransferido { get; set; }
        public string? CodUsuarioTransf { get; set; }
    }
}
