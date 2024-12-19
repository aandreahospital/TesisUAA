using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class Calendario
    {
        public string CodEmpresa { get; set; } = null!;
        public string CodModulo { get; set; } = null!;
        public DateTime? FecActual { get; set; }
        public string? AltaPor { get; set; }
        public DateTime? FecAlta { get; set; }
        public string? ActualizadoPor { get; set; }
        public DateTime? FecActualizado { get; set; }
        public DateTime? FecInicial { get; set; }
        public DateTime? FecFinal { get; set; }
        public string? UsaCalendario { get; set; }
        public Guid Rowid { get; set; }
        public virtual Empresa CodEmpresaNavigation { get; set; } = null!;
        public virtual Modulo CodModuloNavigation { get; set; } = null!;
    }
}
