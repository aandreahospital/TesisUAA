using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class RmReingreso
    {
        public decimal? NroEntrada { get; set; }
        public string? Observacion { get; set; }
        public DateTime? FechaSalida { get; set; }
        public string? UsuarioSalida { get; set; }
        public DateTime? FechaReingreso { get; set; }
        public string? UsuarioReingreso { get; set; }
        public string? UsuarioInscriptor { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string? NombreReingresante { get; set; }
        public string? DocReingresante { get; set; }
        public decimal? TipoDocReingresante { get; set; }
        public decimal IdReingreso { get; set; }
    }
}
