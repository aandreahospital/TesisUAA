using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaBase.Models;

namespace SistemaBase.ModelsCustom
{
    public partial class UsuairosCustom
    {
        public string CodUsuario { get; set; } = null!;
        public string? CodPersona { get; set; }
        public string? Clave { get; set; }
        public string? CodGrupo { get; set; }
        public string? CodEmpresa { get; set; }
        public string? CodSucursal { get; set; }
        [ScaffoldColumn(false)]
        public string? Estado { get; set; }
        [ScaffoldColumn(false)]
        public string? CodArea { get; set; }
        public string? CodCustodio { get; set; }
        [ScaffoldColumn(false)]
        public string? CodColorRegistro { get; set; }
        public string? AutorizaStock { get; set; }
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
        //public virtual Sucursale? Cod { get; set; }
        //public virtual Empresa? CodEmpresaNavigation { get; set; }
        public virtual GruposUsuario? CodGrupoNavigation { get; set; }
       // public virtual ICollection<RmLevantamiento> RmLevantamientos { get; set; }

    }

}
