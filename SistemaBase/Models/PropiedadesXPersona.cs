using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class PropiedadesXPersona
    {
        public string CodPersona { get; set; } = null!;
        public string NumFinca { get; set; } = null!;
        public string Distrito { get; set; } = null!;
        public string? Ubicacion { get; set; }
        public string? CodMoneda { get; set; }
        public decimal? ValComercial { get; set; }
        public string? IndHipoteca { get; set; }
        public string? HipotecadoA { get; set; }
        public decimal? ValHipoteca { get; set; }
        public string? NomTitular { get; set; }
        public decimal? Superficie { get; set; }
        public string? CuentaCte { get; set; }
        public Guid Rowid { get; set; }
    }
}
