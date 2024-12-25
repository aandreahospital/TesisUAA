using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class Sucursale
    {
        public Sucursale()
        {
            Usuarios = new HashSet<Usuario>();
        }

        public string CodEmpresa { get; set; } = null!;
        public string CodSucursal { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? EsMatriz { get; set; }
        public string? Estado { get; set; }
        public string? CasillaCorreo { get; set; }
        public string? CodigoPostal { get; set; }
        public string? DetalleDir { get; set; }
        public string? TrabajaDom { get; set; }
        public string? TrabajaSab { get; set; }
        public string? CodPais { get; set; }
        public string? CodProvincia { get; set; }
        public string? CodCiudad { get; set; }
        public string? CodBarrio { get; set; }
        public string? ManejaStock { get; set; }
        public decimal? PlazoEnvio { get; set; }
        public string? CodSucursalCentral { get; set; }
        public string? CodCustodioIni { get; set; }
        public string? NroPatronal { get; set; }
        public string? StockPropio { get; set; }
        public string? IndUmDef { get; set; }
        public string? CodRegional { get; set; }
        public Guid Rowid { get; set; }

        public virtual Barrio? Cod { get; set; }
        public virtual Empresa CodEmpresaNavigation { get; set; } = null!;
        public virtual Provincia? CodP { get; set; }
        public virtual Paise? CodPaisNavigation { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
