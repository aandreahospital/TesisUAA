using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace SistemaBase.Models
{
    public partial class Usuario
    {
        public string CodUsuario { get; set; } = null!;
        public string? CodPersona { get; set; }
        public string? Clave { get; set; }
        public string? CodGrupo { get; set; }
        public string? CodEmpresa { get; set; }
        public string? CodSucursal { get; set; }
        // [Range(typeof(string), "A", "I", ErrorMessage = "error de Autorizacion")]
     
        public string? Estado { get; set; }
        [ScaffoldColumn(false)]
        public string? CodArea { get; set; }
        public string? CodCustodio { get; set; }
        [ScaffoldColumn(false)]
        public string? CodColorRegistro { get; set; }
    //    [Range(typeof(bool), "S", "N", ErrorMessage = "error de Autorizacion")]
        public string? AutorizaStock { get; set; }
       // [Range(typeof(bool), "S", "N", ErrorMessage = "error de Autorizacion")]
        public string? AutorizaCtacte { get; set; }
        [ScaffoldColumn(false)]
        public string? EMail { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? FechaAlta { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? FechaBaja { get; set; }
        [ScaffoldColumn(false)]
        public decimal? IdTipoUsuario { get; set; }
        [ScaffoldColumn(false)]
        public decimal? CodOficina { get; set; }
        [ScaffoldColumn(false)]
        public Guid Rowid { get; set; }
        public virtual Sucursale? Cod { get; set; }
        public virtual Empresa? CodEmpresaNavigation { get; set; }
        public virtual GruposUsuario? CodGrupoNavigation { get; set; }
        public virtual ICollection<RmLevantamiento>? RmLevantamientos { get; set; }
        public virtual ICollection<Comentario>? Comentarios { get; set; }
        public virtual ICollection<CarreraUsuario>? CarreraUsuarios { get; set; }
        public virtual ICollection<Datosacademico>? Datosacademicos { get; set; }
        public virtual ICollection<Datoslaborale>? Datoslaborales { get; set; }
        public virtual ICollection<Forodebate>? Forodebates { get; set; }

    }
}
