using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaBase.ModelsCustom
{
    public partial class AccesosGrupoCustom
    {
        [Display(Name = "Grupo")]
        public string CodGrupo { get; set; } = null!;
        [Display(Name = "Modulo")]
        public string CodModulo { get; set; } = null!;
        [Display(Name = "Formulario")]
        public string NomForma { get; set; } = null!;
        [Display(Name = "Descripcion")]
        public string? Descripcion { get; set; } = null!;

    }
    public partial class AccesosGrupoCreateCustom {
        [Display(Name = "Grupo:")]
        public string CodGrupo { get; set; } = null!;
        [Display(Name = "Modulo:")]
        public string CodModulo { get; set; } = null!;
        [Display(Name = "Formulario:")]
        public string NomForma { get; set; } = null!;
        [Display(Name = "Puede Insertar:")]
        public string? PuedeInsertar { get; set; }
        [Display(Name = "Puede Borrar:")]
        public string? PuedeBorrar { get; set; }
        [Display(Name = "Puede Actualizar:")]
        public string? PuedeActualizar { get; set; }
        [Display(Name = "Puede Consultar:")]
        public string? PuedeConsultar { get; set; }
        [ScaffoldColumn(false)]
        public string? ItemMenu { get; set; }
        [ScaffoldColumn(false)]
        public Guid Rowid { get; set; }
        public List<SelectListItem> ?GruposUsuarios { get; set;}
        public List<SelectListItem> ?Modulos { get; set; }
        public List<SelectListItem> ?FormasCustom { get; set; }
    }
    //public partial class AccesosGruposCreate
    //{
    //    [Display(Name = "Grupo:")]
    //    public string CodGrupo { get; set; } = null!;
    //    [Display(Name = "Modulo:")]
    //    public string CodModulo { get; set; } = null!;
    //    [Display(Name = "Formulario:")]
    //    public string NomForma { get; set; } = null!;
    //    [Display(Name = "Puede Insertar:")]
    //    public string? PuedeInsertar { get; set; }
    //    [Display(Name = "Puede Borrar:")]
    //    public string? PuedeBorrar { get; set; }
    //    [Display(Name = "Puede Actualizar:")]
    //    public string? PuedeActualizar { get; set; }
    //    [Display(Name = "Puede Consultar:")]
    //    public string? PuedeConsultar { get; set; }
    //    [ScaffoldColumn(false)]
    //    public string? ItemMenu { get; set; }
    //    [ScaffoldColumn(false)]
    //    public Guid Rowid { get; set; }
    //}
    //public partial class GruposUsuariosCustom
    //{
    //    [Display(Name = "Codigo:")]
    //    public string CodGrupo { get; set; } = null!;
    //    [Display(Name = "Descripcion:")]
    //    public string Descripcion { get; set; } = null!;
    //}
    //public partial class ModulosCustom
    //{
    //    [Display(Name = "Codigo:")]
    //    public string CodModulo { get; set; } = null!;
    //    [Display(Name = "Descripcion:")]
    //    public string Descripcion { get; set; } = null!;
    //}
    //public partial class FormasCustom
    //{
    //    [Display(Name = "Codigo:")]
    //    public string NomForma { get; set; } = null!;
    //    [Display(Name = "Descripcion:")]
    //    public string Descripcion { get; set; } = null!;
    //}
}
