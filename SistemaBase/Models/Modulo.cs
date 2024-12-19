using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaBase.Models
{
    public partial class Modulo
    {
        public Modulo()
        {
            AccesosGrupos = new HashSet<AccesosGrupo>();
            Calendarios = new HashSet<Calendario>();
            Formas = new HashSet<Forma>();
            ParametrosGenerales = new HashSet<ParametrosGenerale>();
            SubtiposTrans = new HashSet<SubtiposTran>();
            TiposTrans = new HashSet<TiposTran>();
            UsuariosTrans = new HashSet<UsuariosTran>();
        }
        [Display(Name= "Codigo Modulo")]
        public string CodModulo { get; set; } = null!;
        [Display(Name = "Descripcion")]
        public string? Descripcion { get; set; }

        [Display(Name = "Maneja Calendario:")]
        public string? ManejaCalendario { get; set; }
        [Display(Name = "Maneja Cierre:")]
        public string? ManejaCierre { get; set; }
       [ScaffoldColumn(false)]
        public Guid Rowid { get; set; }
        public virtual ICollection<AccesosGrupo> AccesosGrupos { get; set; }
        public virtual ICollection<Calendario> Calendarios { get; set; }
        public virtual ICollection<Forma> Formas { get; set; }
        public virtual ICollection<ParametrosGenerale> ParametrosGenerales { get; set; }
        public virtual ICollection<SubtiposTran> SubtiposTrans { get; set; }
        public virtual ICollection<TiposTran> TiposTrans { get; set; }
        public virtual ICollection<UsuariosTran> UsuariosTrans { get; set; }
    }
}
