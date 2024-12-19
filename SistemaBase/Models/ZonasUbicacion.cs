using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class ZonasUbicacion
    {
        public string CodZona { get; set; } = null!;
        public string CodPais { get; set; } = null!;
        public string CodProvincia { get; set; } = null!;
        public string CodCiudad { get; set; } = null!;
        public string CodBarrio { get; set; } = null!;
        public Guid Rowid { get; set; }
        public virtual Barrio Cod { get; set; } = null!;
        public virtual Provincia CodP { get; set; } = null!;
        public virtual Paise CodPaisNavigation { get; set; } = null!;
        public virtual ZonasGeografica CodZonaNavigation { get; set; } = null!;
    }
}
