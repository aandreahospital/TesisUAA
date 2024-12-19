using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class Barrio
    {
        public string CodPais { get; set; } = null!;
        public string CodProvincia { get; set; } = null!;
        public string CodCiudad { get; set; } = null!;
        public string CodBarrio { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? Abreviatura { get; set; }
       // public virtual ICollection<DirecPersona>? DirecPersonas { get; set; }
        public virtual ICollection<Sucursale>? Sucursales { get; set; }
        public virtual ICollection<ZonasUbicacion>? ZonasUbicacions { get; set; }
    }
}
