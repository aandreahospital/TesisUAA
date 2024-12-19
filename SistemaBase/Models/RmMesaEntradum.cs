using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaBase.Models
{
    public partial class RmMesaEntradum
    {

        //public RmMesaEntradum()
        //{
        //    RmInformes = new HashSet<RmInforme>();
        //    RmMedidasPrenda = new HashSet<RmMedidasPrenda>();
        //}
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal NumeroEntrada { get; set; }
        public DateTime? FechaEntrada { get; set; }
        public decimal? CodigoOficina { get; set; }
        public decimal? CodOficinaRetiro { get; set; }
        public string? NumeroLiquidacion { get; set; }
        public decimal? TipoSolicitud { get; set; }
        public decimal? NroFormulario { get; set; }
        public decimal? EstadoEntrada { get; set; }
        public string? NombrePresentador { get; set; }
        public decimal? TipoDocumentoPresentador { get; set; }
        public string? NroDocumentoPresentador { get; set; }
        public DateTime? FechaSalida { get; set; }
        public string? NombreRetirador { get; set; }
        public decimal? TipoDocumentoRetirador { get; set; }
        public string? NroDocumentoRetirador { get; set; }
        public string? UsuarioEntrada { get; set; }
        public string? UsuarioSalida { get; set; }
        public string? NomTitular { get; set; }
        public decimal? IdAutorizante { get; set; }
        public decimal? NroEntradaOriginal { get; set; }
        public string? Reingreso { get; set; }
        public string? NombreReingresante { get; set; }
        public string? DocReingresante { get; set; }
        public decimal? TipoDocReingresante { get; set; }
        public string? NroDocumentoTitular { get; set; }
        public string? TipoDocumentoTitular { get; set; }
        public string? NroBoleta { get; set; }
        public string? Impreso { get; set; }
        public string? NroOficio { get; set; }
        public string? TipoDocumento { get; set; }
        public decimal? MontoLiquidacion { get; set; }
        public string? NroSenal { get; set; }
        public DateTime? FechaAntReingreso { get; set; }
        public DateTime? FechaAntSalReing { get; set; }
        public string? Anulado { get; set; }
        public decimal? IdMotivoAnulacion { get; set; }
        public DateTime? FechaAnulacion { get; set; }
        public string? UsuarioAnulacion { get; set; }
        public string? NumeroLiquidacionLetras { get; set; }
        public DateTime? FechaRecSalida { get; set; }
        public string? NroBoletaAnterior { get; set; }
        public string? NroBoletaAntSenhal { get; set; }
        public decimal? CantidadReingreso { get; set; }
        public Guid Rowid { get; set; }
        public byte[]? ArchivoPDF { get; set; }
        public byte[]? AnexoPDF { get; set; }
        //public string? RutaPdf { get; set; }
        //public virtual RmOficinasRegistrale? CodigoOficinaNavigation { get; set; }
        //public virtual RmAutorizante? IdAutorizanteNavigation { get; set; }
        //public virtual RmMotivosAnulacion? IdMotivoAnulacionNavigation { get; set; }
        //public virtual RmTipoSolicitud? TipoSolicitudNavigation { get; set; }
        //public virtual ICollection<RmCertificacione>? RmCertificacione { get; set; }
        //public virtual ICollection<RmInforme>? RmInformes { get; set; }
        public virtual ICollection<RmMedidasPrenda>? RmMedidasPrenda { get; set; }

    }
}
