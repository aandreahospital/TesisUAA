using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class AccesosRrhh
    {
        public string? CodGrupo { get; set; }
        public string? CodModulo { get; set; }
        public string? NomForma { get; set; }
        public string? PuedeInsertar { get; set; }
        public string? PuedeBorrar { get; set; }
        public string? PuedeActualizar { get; set; }
        public string? PuedeConsultar { get; set; }
        public string? ItemMenu { get; set; }
        public Guid Rowid { get; set; }
    }
}
