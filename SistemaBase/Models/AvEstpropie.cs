using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class AvEstpropie
    {
        public string CodPropietario { get; set; } = null!;
        public string CodEstable { get; set; } = null!;
        public string? ProMayor { get; set; }
        public DateTime? FecVa1era { get; set; }
        public DateTime? FecVaultima { get; set; }
        public string? Estado { get; set; }
        public string? HabilitadoCota { get; set; }
        public string? CodPropietarioViejo { get; set; }
        public string? BloqCria { get; set; }
        public string? BloqEngorde { get; set; }
        public string? BloqInvernada { get; set; }
        public string? BloqReproduccion { get; set; }
        public string? BloqConsumo { get; set; }
        public string? BloqFaena { get; set; }
        public string? BloqExposicion { get; set; }
        public Guid Rowid { get; set; }
    }
}
