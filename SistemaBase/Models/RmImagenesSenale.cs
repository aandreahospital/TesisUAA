using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class RmImagenesSenale
    {
        public double CodImgSenal { get; set; }
        public string UrlImg { get; set; } = null!;
        public string? CodTipoCorte { get; set; }
    }
}
