﻿using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class Paise
    {
        public Paise()
        {
            Moneda = new HashSet<Moneda>();
            Provincia = new HashSet<Provincia>();
            Sucursales = new HashSet<Sucursale>();
            ZonasUbicacions = new HashSet<ZonasUbicacion>();
        }

        public string CodPais { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? Nacionalidad { get; set; }
        public string? CodigoArea { get; set; }
        public string? Abreviatura { get; set; }
        public string? Siglas { get; set; }
        public Guid Rowid { get; set; }

        public virtual ICollection<Moneda> Moneda { get; set; }
        public virtual ICollection<Provincia> Provincia { get; set; }
        public virtual ICollection<Sucursale> Sucursales { get; set; }
        public virtual ICollection<ZonasUbicacion> ZonasUbicacions { get; set; }
    }
}