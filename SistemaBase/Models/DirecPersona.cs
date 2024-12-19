using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class DirecPersona
    {
        //public DirecPersona()
        //{
        //    TelefPersonas = new HashSet<TelefPersona>();
        //}
        public string CodPersona { get; set; } = null!;
        public string CodDireccion { get; set; } = null!;
        public string? TipDireccion { get; set; }
        public string? CasillaCorreo { get; set; }
        public string? CodigoPostal { get; set; }
        public string? Detalle { get; set; }
        public string? CodPais { get; set; }
        public string? CodProvincia { get; set; }
        public string? CodCiudad { get; set; }
        public string? CodBarrio { get; set; }
        public string? PorDefecto { get; set; }
        public string? DescripcionRef { get; set; }
        public string? Ciudad { get; set; }
        public string? Barrio { get; set; }
        public Guid Rowid { get; set; }
       // public virtual Barrio? Cod { get; set; }
        //public virtual ICollection<TelefPersona> TelefPersonas { get; set; }
    }
}
