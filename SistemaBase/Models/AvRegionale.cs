using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class AvRegionale
    {
        public string? CodRegional { get; set; }
        public string? Descripcion { get; set; }
        public string? CodDepartamento { get; set; }
        public string? CodPais { get; set; }
        public Guid Rowid { get; set; }
    }
}
