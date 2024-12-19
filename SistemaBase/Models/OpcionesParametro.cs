using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class OpcionesParametro
    {
        public string Tipo { get; set; } = null!;
        public string Parametro { get; set; } = null!;
        public string NomForma { get; set; } = null!;
        public string? Descripcion { get; set; }
        public Guid Rowid { get; set; }
    }
}
