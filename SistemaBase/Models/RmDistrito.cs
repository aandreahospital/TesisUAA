using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaBase.Models
{
    public partial class RmDistrito
    {
        [DisplayFormat(DataFormatString = "{0:F0}", ApplyFormatInEditMode = true)]
        public decimal CodigoDepto { get; set; }
        [DisplayFormat(DataFormatString = "{0:F0}", ApplyFormatInEditMode = true)]
        public decimal CodigoDistrito { get; set; }
        public string? Nomenclador { get; set; }
        public string? DescripDistrito { get; set; }

        [ScaffoldColumn(false)]
        public Guid Rowid { get; set; }
    }
}
