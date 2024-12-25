using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class Barrio
    {
        public Barrio()
        {
            Sucursales = new HashSet<Sucursale>();
        }

        public string CodPais { get; set; } = null!;
        public string CodProvincia { get; set; } = null!;
        public string CodCiudad { get; set; } = null!;
        public string CodBarrio { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? Abreviatura { get; set; }

        public virtual ICollection<Sucursale> Sucursales { get; set; }
    }
}
