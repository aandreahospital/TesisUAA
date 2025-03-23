using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class Usuario
    {
        //public Usuario()
        //{
           // Cargos = new HashSet<Cargo>();
           // CarreraUsuarios = new HashSet<CarreraUsuario>();
           // Comentarios = new HashSet<Comentario>();
           // DatosAcademicos = new HashSet<DatosAcademico>();
           // DatosLaborales = new HashSet<DatosLaborale>();
            //ForoDebates = new HashSet<ForoDebate>();
            //OfertaAcademicas = new HashSet<OfertaAcademica>();
            //OfertaLaborals = new HashSet<OfertaLaboral>();
       // }

        public string CodUsuario { get; set; } = null!;
        public string? Clave { get; set; }
        public DateTime? FecCreacion { get; set; }
        public string? CodGrupo { get; set; }
        public string? CodPersona { get; set; }

        public virtual GruposUsuario? CodGrupoNavigation { get; set; }
        public virtual Persona? CodPersonaNavigation { get; set; }
        //public virtual ICollection<Cargo> Cargos { get; set; }
        //public virtual ICollection<CarreraUsuario> CarreraUsuarios { get; set; }
       // public virtual ICollection<Comentario> Comentarios { get; set; }
       // public virtual ICollection<DatosAcademico> DatosAcademicos { get; set; }
        //public virtual ICollection<DatosLaborale> DatosLaborales { get; set; }
        //public virtual ICollection<ForoDebate> ForoDebates { get; set; }
        //public virtual ICollection<OfertaAcademica> OfertaAcademicas { get; set; }
        //public virtual ICollection<OfertaLaboral> OfertaLaborals { get; set; }
    }
}
