using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaBase.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SistemaBase.ModelsCustom
{
    public class RmAnulacionesMarcaCustom
    {
        [Display(Name = "ID")]
        public decimal IdMarca { get; set; }

        [Display(Name = "Motivo Anulacion")]
        public decimal? IdMotivoAnulacion { get; set; }
        public String? MotivoAnulacion { get; set; }

        [Display(Name = "Usuario")]
        public string? IdUsuario { get; set; }
        public string? Usuario { get; set; }

        [Display(Name = "Fecha de Alta")]
        public DateTime? FechaAlta { get; set; }
        public Guid Rowid { get; set; }
    }
    public partial class AnulacionesMarcaCreate
    {
        [Display(Name = "ID")]
        public decimal IdMarca { get; set; }

        [Display(Name = "Motivo Anulacion")]
        public decimal? IdMotivoAnulacion { get; set; }

        [Display(Name = "Usuario")]
        public string? IdUsuario { get; set; }

        [Display(Name = "Fecha de Alta")]
        public DateTime? FechaAlta { get; set; }
        public Guid Rowid { get; set; }
        public List<SelectListItem>? Motivos { get; set; }
        public List<SelectListItem>? Usuarios { get; set; }
    }


     

}
