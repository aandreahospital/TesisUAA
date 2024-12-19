using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace SistemaBase.ModelsCustom
{
    public partial class ModuloCustom
    {
        [Display(Name = "Codigo Modulo")]
        public string CodModulo { get; set; } = null!;
        [Display(Name = "Descripcion")]
        public string? Descripcion { get; set; }
        [Display(Name = "Maneja Calendario")]
        public string? ManejaCalendario { get; set; }
        [Display(Name = "Maneja Cierre")]
        public string? ManejaCierre { get; set; }
        public Guid Rowid { get; set; }
    }
}