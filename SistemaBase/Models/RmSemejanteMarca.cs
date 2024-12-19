using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class RmSemejanteMarca
    {
        public decimal IdMarca { get; set; }
        public DateTime? FecProceso { get; set; }
        public decimal? PorcSemejanza { get; set; }
        public decimal IdMarcaSem { get; set; }
    }
}
