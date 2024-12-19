using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class Funcionario
    {
        public string CodEmpresa { get; set; } = null!;
        public string? CodSucursal { get; set; }
        public string CodFuncionario { get; set; } = null!;
        public string? CodPersona { get; set; }
        public string? CodDepartamento { get; set; }
        public string? RecibeDocumentos { get; set; }
        public string? Activo { get; set; }
        public Guid Rowid { get; set; }
    }
}
