using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class Provincia
    {
        public Provincia()
        {
            Sucursales = new HashSet<Sucursale>();
        }

        public string CodPais { get; set; } = null!;
        public string CodProvincia { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? Abreviatura { get; set; }
        public Guid Rowid { get; set; }

        public virtual Paise CodPaisNavigation { get; set; } = null!;
        public virtual ICollection<Sucursale> Sucursales { get; set; }
    }
}
