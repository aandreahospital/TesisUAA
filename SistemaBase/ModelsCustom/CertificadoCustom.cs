using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Web.Mvc;

namespace SistemaBase.ModelsCustom
{
    public class CertificadoCustom
    {
        public decimal NumeroEntrada { get; set; }
        public decimal NroEntradaTrans { get; set; }
        public decimal IdTransaccion { get; set; }
        public decimal? EstadoEntrada { get; set; }
        public string? NroBoleta { get; set; }
        public decimal? TipoSolicitud { get; set; }
        public string? TipoOperacion { get; set; }
        public string? DescripSolicitud { get; set; }
        //public string? TipoOperacion { get; set; }
        public DateTime? FechaActoJuridico { get; set; }
        public DateTime? FechaAlta { get; set; }
        public decimal? IdAutorizante { get; set; }
        public string? NombreAutorizante { get; set; }
        public decimal? MatriculaAutorizante { get; set; }
        public string? DescripAutorizante { get; set; }
        public string? Serie { get; set; }
        public string? FormaConcurre { get; set; }
        public string? NomTitular { get; set; }
        public string? NroDocumentoTitular { get; set; }
        public string? Representante { get; set; }
        public string? NombreRepresentante { get; set; }
        public byte[]? ImgMarca { get; set; }
        public byte[]? ImgSenal { get; set; }
        public string? IdEscribanoJuez { get; set; }
        public string? NroEscrituraDsAi { get; set; }
        public DateTime? FechaEscrituraDsAi { get; set; }
        public string? Observacion { get; set; }




        //Propietario adjudicado
        public DateTime? FechaRegistro { get; set; }
        public decimal? IdTransaccionPropietario { get; set; }
        public string? Comentario { get; set; }
        public string? CodEstable { get; set; }
        public decimal? CantidadGanado { get; set; }
        public string? IdTipoPropiedad { get; set; }
        public string? Nombre { get; set; }
        public decimal? Asiento { get; set; }
        public string? IdUsuario { get; set; }


        public class Distrito
        {
            public string? CodDistrito { get; set; }
            public string? Descripcion { get; set; }
            public string? DescripcionEstable { get; set; }
        }
        public class Establecimiento
        {
            public string? Descripcion { get; set; }
        }
        public class Propietario
        {
            public string? IdPropietario { get; set; }
            public string? DescPropietario { get; set; }
            public string? TipoPropiedad { get; set; }
        }
        public List<SelectListItem>? RmMarcasXEstab { get; set; }
        public List<SelectListItem>? RmTitularesMarca { get; set; }
        public List<SelectListItem>? RmTiposPropiedad { get; set; }
        public List<SelectListItem>? RmMarcasSenale { get; set; }
    }
}
