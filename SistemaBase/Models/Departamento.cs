using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class Departamento
    {
        public string CodEmpresa { get; set; } = null!;
        public string CodDepartamento { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? Activo { get; set; }
        public string? CodSucursal { get; set; }
        public double? CodDepto { get; set; }
        public Guid Rowid { get; set; }
    }
}
