using System;
using System.Collections.Generic;
using SistemaBase.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SistemaBase.ModelsCustom
{
	public class GruposUsuarioCustom
	{
        [Display(Name = "Codigo")]
        public string CodGrupo { get; set; } = null!;
        public string? Descripcion { get; set; }
        [ScaffoldColumn(false)]
        public string? OpFueraHo { get; set; }
        [ScaffoldColumn(false)]
        public Guid Rowid { get; set; }
    }
}