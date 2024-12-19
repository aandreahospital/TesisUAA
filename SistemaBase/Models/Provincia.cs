using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaBase.Models
{
    public partial class Provincia
    {
        /*public Provincia()
        {
            Sucursales = new HashSet<Sucursale>();
            ZonasUbicacions = new HashSet<ZonasUbicacion>();
        }*/
        public string CodPais { get; set; } = null!;
        public string CodProvincia { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? Abreviatura { get; set; }
        [ScaffoldColumn(false)]
        public Guid Rowid { get; set; }
        public virtual Paise CodPaisNavigation { get; set; } = null!;
        public virtual ICollection<Sucursale>? Sucursales { get; set; }
        public virtual ICollection<ZonasUbicacion>? ZonasUbicacions { get; set; }
    }
}
