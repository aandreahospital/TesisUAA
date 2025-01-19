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
        public string? Estado { get; set; }
        public string? CasillaCorreo { get; set; }
        public string? CodigoPostal { get; set; }
        public string? DetalleDir { get; set; }
        public decimal? PlazoEnvio { get; set; }
        public string? NroPatronal { get; set; }
        public Guid Rowid { get; set; }

        public virtual Empresa CodEmpresaNavigation { get; set; } = null!;
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
