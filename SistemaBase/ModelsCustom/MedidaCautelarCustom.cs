using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaBase.ModelsCustom
{
    public partial class MedidaCautelarCustom
    {
        [Key]
        [DisplayName("Id Medida")]
        public decimal IdMedida { get; set; }

        [DisplayName("Id Usuario")]
        public string? CodUsuario { get; set; }

        [DisplayName("Fecha Acto")]
        public DateTime Fecha_Operacion { get; set; }

        [DisplayName("Nro. Entrada")]
        public string? Nro_Entrada { get; set; }

        [DisplayName("Nro. Boleta de Marca")]
        public string? Nro_Boleta { get; set; }
        [DisplayName("Nro. Boleta de Señal")]
        public string? Nro_Boleta_Senal { get; set; }

        [DisplayName("Nro. Oficio")]
        public string? Nro_Oficio { get; set; }

        [DisplayName("Feha Inscripción")]
        public DateTime? Fecha_Inscripcion { get; set; }

        [NotMapped]
        [DisplayName("Instrumento")]
        public List<SelectListItem>? Instrumento { get; set; }

        [DisplayName("Acreedor")]
        public decimal? Acreedor { get; set; }


        [DisplayName("Deudor")]
        public decimal? Deudor { get; set; }


        [DisplayName("Monto")]
        public decimal? Monto_Prenda { get; set; }

        [DisplayName("Tipo Moneda")]
        public decimal? Tipo_Moneda { get; set; }
        public string? Desc_Moneda { get; set; }

        [DisplayName("Autorizante")]
        public decimal? Id_Autorizante { get; set; }
        public string? Desc_Autorizante { get; set; }

        [NotMapped]
        [DisplayName("Tipo de Embargo")]
        public List<SelectListItem>? Tipo_Embargo { get; set; }

        [DisplayName("Juzgado")]
        public string? Juzgado { get; set; }

        [DisplayName("Secretaria")]
        public string? Secretaria { get; set; }

        [DisplayName("Nro. Libro")]
        public string? Libro { get; set; }

        [DisplayName("Nro. Folio")]
        public string? Folio { get; set; }

        [DisplayName("Cantidad Ganado")]
        public decimal? Cant_Ganado { get; set; }

        [DisplayName("Gasto Justicia")]
        public decimal? Monto_De_Justicia { get; set; }
    }
}
