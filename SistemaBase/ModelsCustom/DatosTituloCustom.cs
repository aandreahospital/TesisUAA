using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SistemaBase.ModelsCustom
{
    public class DatosTituloCustom
    {
        public decimal NumeroEntrada { get; set; } //Mesa Entrada
        public string? FechaActual { get; set; }
        public DateTime? FechaEntrada { get; set; }
        public string? NroBoleta { get; set; }
        public decimal? CodigoOficina { get; set; }
        public List<SelectListItem>? Oficinas { get; set; }
        public string? DescripOficina { get; set; } //Oficinas Registrales
        public byte[]? ImgMarca { get; set; }
        public byte[]? ImgSenal { get; set; }
        public string? NomTitular { get; set; }
        public string? NroDocumentoTitular { get; set; }
        public string? DireccionTitular { get; set; }
        public string? CiudadTitular { get; set; }
        public List<SelectListItem>? CiudadesTitu { get; set; }
        public string? CodPersona { get; set; } //Persona
        public string? CodPais { get; set; }
        public List<SelectListItem>? Nacionalidades { get; set; } //Paises
        public string? Descripcion { get; set; }
        public string? CodProfesion { get; set; }
        public List<SelectListItem>? Profesiones { get; set; } //Profesiones
        public string? Nacionalidad { get; set; }
        public DateTime? FecNacimiento { get; set; }
        public int? Edad { get; set; }
        public string? CodEstadoCivil { get; set; }
        public List<SelectListItem>? EstadoCivil { get; set; } //Estados Civiles
        public string? DistritoEstable { get; set; }
        public List<SelectListItem>? Distritos { get; set; }
        public string? DepartamentoEstable { get; set; }
        public string? Representante { get; set; }
        public string? NombreRepresentante { get; set; } //Rm Transacciones
        public string? CodPaisRep { get; set; }
        public List<SelectListItem>? NacionalidadesRep { get; set; } //Paises
        public string? CodEstadoCivilRep { get; set; }
        public List<SelectListItem>? EstadoCivilRep { get; set; } //Estados Civiles
        public string? CodProfesionRep { get; set; }
        public List<SelectListItem>? ProfesionesRep { get; set; } //Profesiones
        public string? DirecRep { get; set; }
        public string? CiudadRep { get; set; }
        public List<SelectListItem>? CiudadesRep { get; set; }
        public decimal? IdAutorizante { get; set; }
        public List<SelectListItem>? MatriculaRegistro { get; set; } //Rm Autorizantes
        public List<SelectListItem>? Autorizantes { get; set; }
        public List<SelectListItem>? CodCiudadAuto { get; set; }
        public List<SelectListItem>? DescripCiudadAuto { get; set; }
        public DateOnly? FechaActoJuridico { get; set; }
        public DateTime? FechaReingreso { get; set; } //Reingreso
        public DateOnly? FechaEntradaFecha { get; set; } // Para almacenar la fecha de FechaEntrada
        public TimeSpan? FechaEntradaHora { get; set; }
        public string? TipoOperacion { get; set; }
        public List<SelectListItem>? Operaciones { get; set; }
        public string? NombreAutorizante { get; set; }
        public decimal? MatriculaAutorizante { get; set; }
        public string? CiudadAutorizante { get; set; }
        public string? NroBoletaSenal { get; set; }
        public string? Comentario { get; set; }
        public string? ImageDataUri { get; set; }
        public string? ImageDataUri2 { get; set; }
        public string? ImageDataUri3 { get; set; }
        public string? ImagenMarca { get; set; }
        public string? ImagenSenhal { get; set; }
        public string? Barcode { get; set; }
        public decimal? IdTransaccion { get; set; }
        public DateTime? FechaAlta { get; set; }

        public List<TitularesCarga>? Titulares { get; set; }

        public class TitularesCarga
        {
            public string? IdPropietario { get; set; }
            public string? Nombre { get; set; }
        }

    }
}
