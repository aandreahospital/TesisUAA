using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using X.PagedList;

namespace SistemaBase.ModelsCustom
{
    public partial class  InscripcionCustom
    {
        public decimal NroEntrada { get; set; }
        public decimal? EstadoEntrada { get; set; }
        public decimal? IdTransaccion { get; set; }
        public string? NroBoleta { get; set; }
        public string? NroBolMarcaAnt { get; set; }
        public string? NroBolSenhalAnt { get; set; }

        public List<SelectListItem>? Operacion { get; set; }
        public string? TipoOperacion { get; set; }
        public DateTime? FechaActoJuridico { get; set; }

        public List<SelectListItem>? Autorizante { get; set; }
        public decimal? IdAutorizante { get; set; }
        public byte[]? ImgMarca { get; set; }

        public byte[]? ImgSenal { get; set; }
        public string? Serie { get; set; }
        public string? FormaConcurre { get; set; }
        public decimal? CantidadGanado { get; set; }
        public string? Representante { get; set; }
        public string? NombreRepresentante { get; set; }
        public string? FirmanteRuego { get; set; }

        public List<SelectListItem>? Distrito { get; set; }
        public string? CodDistrito { get; set; }
        public string? Departamento { get; set; }
        public string? Descripcion { get; set; }
        public string? GpsH { get; set; }
        public string? GpsV { get; set; }
        public string? GpsSc { get; set; }
        public string? GpsS { get; set; }
        public string? GpsW { get; set; }

        public List<TitularMarcaViewModel>? Titulares { get; set; }
        //public decimal? IdPropietario { get; set; }
        //public string? Nombre { get; set; }
        //public string? EsPropietario { get; set; }
        public string? Comentario { get; set; }
        public decimal? Asiento { get; set; }
        public string? IdUsuario { get; set; }
        public string? NombreAutorizante { get; set; }
        public decimal? MatriculaAutorizante { get; set; }
        public string? NombreFirmanteRuego { get; set; }
        public string? Observacion { get; set; }


        public class TitularMarcaViewModel
        {
            public string? IdPropietario { get; set; }
            public string? Nombre { get; set; }
            public string? EsPropietario { get; set; }
        }

    }

    public class PersonaTitular
    {
        public string? IdPropietario { get; set; }
        public string? Nombre { get; set; }
        public decimal? TipoPropiedad { get; set; }
       
    }

    public class PersonaDireccion
    {
        public string? Operacion { get; set; } = string.Empty;
        public string? IdPropietario { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string? TipoPropiedad { get; set; }

    }
}
