using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class TarjetasCredito
    {
        public string? CodPersona { get; set; }
        public string? NomEntidadEmisora { get; set; }
        public string? MarcaTarjetaCredito { get; set; }
        public string? ClaseTarjetaCredito { get; set; }
        public string? NroTarjetaCredito { get; set; }
        public Guid Rowid { get; set; }
    }
}
