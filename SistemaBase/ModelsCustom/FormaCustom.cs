
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaBase.ModelsCustom
{
    public partial class FormaCustom
    {
        [Display(Name = "Cod.Módulo")]
        public string CodModulo { get; set; } = null!;
        [Display(Name = "Descripción Módulo")]
        public string? DescripcionModulo { get; set; } = null!;
        [Display(Name = "Forma")]
        public string NomForma { get; set; } = null!;
        [Display(Name = "Título de la forma")]
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public Guid Rowid { get; set; }

    }
    public partial class FormaCreateCustom
    {
        [Display(Name = "Cod.Módulo:")]
        public string CodModulo { get; set; } = null!;
        [Display(Name = "Descripción Módulo:")]
        public string? DescripcionModulo { get; set; } = null!;
        [Display(Name = "Forma:")]
        public string NomForma { get; set; } = null!;
        [Display(Name = "Título de la forma:")]
        public string? Titulo { get; set; }
        [Display(Name = "Descripción:")]
        public string? Descripcion { get; set; }
        public Guid Rowid { get; set; }
        public List<SelectListItem> ModuloCreateCustom { get; set; }
        public List<SelectListItem> FormasCreateCustom { get; set; }

    }
}
