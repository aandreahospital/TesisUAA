using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaBase.Models
{
    public partial class RmCambiosEstado
    {
        public decimal NroMovimiento { get; set; }
        public string? NroEntrada { get; set; }
        public string? CodUsuario { get; set; }
        public string? EstadoAnterior { get; set; }
        public DateTime? FechaOperacion { get; set; }
        public string? Observacion { get; set; }
        public string? EstadoNuevo { get; set; }
        public string? CodOperacion { get; set; }
        [ScaffoldColumn(false)]
        public Guid Rowid { get; set; }
       public virtual RmOperacionesSist? CodOperacionNavigation { get; set; }
        
    }
}
