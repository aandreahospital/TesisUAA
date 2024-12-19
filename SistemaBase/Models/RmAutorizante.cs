using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaBase.Models
{
    public partial class RmAutorizante
    {
       
        public decimal? MatriculaRegistro { get; set; }
        public string? DescripAutorizante { get; set; }
        public string? TipoAutorizante { get; set; }
        public string? CodCiudad { get; set; }
        [ScaffoldColumn(false)]
        public Guid Rowid { get; set; }
        public decimal IdAutorizante { get; set; }
        //public virtual ICollection<RmMesaEntradum>? RmMesaEntrada { get; set; }
        public virtual ICollection<RmMedidasPrenda>? RmMedidasPrenda { get; set; }
    }
}
