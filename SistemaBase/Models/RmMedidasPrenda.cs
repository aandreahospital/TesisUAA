﻿using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class RmMedidasPrenda
    {
        public decimal? IdMarca { get; set; }
        public decimal? NroEntrada { get; set; }
        public decimal? NroOficio { get; set; }
        public DateTime? FechaInscripcion { get; set; }
        public DateTime? FechaOperacion { get; set; }
        public string? Acreedor { get; set; }
        public string? Deudor { get; set; }
        public string? Instrumento { get; set; }
        public decimal? MontoPrenda { get; set; }
        public decimal? MontoDeJusticia { get; set; }
        public decimal? TipoMoneda { get; set; }
        public string? Folio { get; set; }
        public string? Libro { get; set; }
        public string? CodUsuario { get; set; }
        public decimal? IdAutorizante { get; set; }
        public string? Estado { get; set; }
        public string? EstadoTransaccion { get; set; }
        public string? Observacion { get; set; }
        public string? Entregado { get; set; }
        public string? UsuarioSup { get; set; }
        public string? NroBoleta { get; set; }
        public string? ObservacionSup { get; set; }
        public string? TipoOperacion { get; set; }
        public decimal? RepId { get; set; }
        public decimal? EstadoRegistral { get; set; }
        public string? Juzgado { get; set; }
        public string? Secretaria { get; set; }
        public string? Juicio { get; set; }
        public decimal? CantGanado { get; set; }
        public decimal? IdTransaccion { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public DateTime? FechaAutorizacion { get; set; }
        public string? CodDistrito { get; set; }
        public string? NroBoletaSenal { get; set; }
        public string? TipoEmbargo { get; set; }
        public Guid Rowid { get; set; }
        public decimal IdMedida { get; set; }

        public virtual RmAutorizante? IdAutorizanteNavigation { get; set; }
        public virtual RmMesaEntradum? NroEntradaNavigation { get; set; }
        public virtual RmTiposMoneda? TipoMonedaNavigation { get; set; }
    }
}