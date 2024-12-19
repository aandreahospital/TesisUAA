using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaBase.Models
{
    public partial class Mese
    {

        [DisplayFormat(DataFormatString = "{0:F0}", ApplyFormatInEditMode = true)]
        public decimal Mes { get; set; }
        public string? Descripcion { get; set; }
        public string? Abreviatura { get; set; }
        public Guid Rowid { get; set; }

    }
}
