using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class StListaPrecio
    {
        public string? DescArticulo { get; set; }
        public string? Precio { get; set; }
        public string? Garantia { get; set; }
        public string? GrupoArt { get; set; }
        public DateTime? Fecha { get; set; }
        public string? CodEmpresa { get; set; }
        public string? CodSucursal { get; set; }
        public string? CodProveedor { get; set; }
        public Guid Rowid { get; set; }
    }
}
