using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class TarjetasDebito
    {
        public string? CodPersona { get; set; }
        public string? NomEntidadEmisora { get; set; }
        public string? MarcaTarjetaDebito { get; set; }
        public string? ClaseTarjetaDebito { get; set; }
        public string? NroTarjetaDebito { get; set; }
        public Guid Rowid { get; set; }
    }
}
