
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SistemaBase.ModelsCustom
{
    public partial class TransaccionCustom
    {
        //Datos de la entrada
        public decimal NumeroEntrada { get; set; }
        public decimal? EstadoEntrada{ get; set; }
        public decimal? IdTransaccion { get; set; }
        public string? TipoOperacion { get; set; }
        public DateTime? FechaActoJuridico { get; set; }
        public string? IdEscribanoJuez { get; set; }
        public string? NomEscribano { get; set; }
        public string? FormaConcurre { get; set; }
        public string? Serie { get; set; }
        public decimal? IdAutorizante { get; set; }
        public string? NombreAutorizante { get; set; }
        public decimal? MatriculaAutorizante { get; set; }
        public string? Representante { get; set; }
        public string? NombreRepresentante { get; set; }
        public string? NroBoleta { get; set; }

        public byte[]? ImgMarca { get; set; }
        public byte[]? ImgSenal { get; set; }

        //Propietario adjudicado
        public string? IdPropietario { get; set; }
        public string? DescTitular { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]

        public DateTime? FechaRegistro { get; set; }
        public decimal? IdTransaccionPropietario { get; set; }
        public string? Comentario { get; set; }
        public string? IdUsuario { get; set; }
        public decimal? NroEntradaTrans { get; set; }
        public string? Observacion { get; set; }



        public class HistoricoPropietarios
        {
            public string? IdPropietario { get; set; }
            public string? Nombre { get; set; }
            public DateTime? FechaRegistro { get; set; }
            public decimal? IdTransaccion { get; set; }
        }
        public List<TitularesMarcasCarga>? Titulares { get; set; }

        public class TitularesMarcasCarga
        {
            public string? IdPropietario { get; set; }
            public string? Nombre { get; set; }
        }
        public class TitularesMarcas
        {
            public string? Operacion { get; set; } = string.Empty;
            public string? IdPropietario { get; set; }
            public string? Nombre { get; set; }
        }
        public class TitularesImpresion
        {
            public string? Texto { get; set; } = string.Empty;
            public string? IdPropietario { get; set; }
            public string? Texto2 { get; set; } = string.Empty;
            public string? Nombre { get; set; }
        }

    }
}
