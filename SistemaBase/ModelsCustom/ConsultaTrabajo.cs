using System.ComponentModel.DataAnnotations;

namespace SistemaBase.ModelsCustom
{
    public partial class ConsultaTrabajo
    {
        //Datos de Movimientos
        public decimal? NroMovimiento { get; set; }
        public decimal? NroEntrada { get; set; }
        public string? CodUsuario { get; set; }
        public DateTime? FechaOperacion { get; set; }
        public string? CodOperacion { get; set; }
        public string? NroMovimientoRef { get; set; }
        public decimal? EstadoEntradaMov { get; set; }


        //Datos de la Entrada
        public DateTime? FechaEntrada { get; set; }
        public decimal? CodigoOficina { get; set; }
        public decimal? CodOficinaRetiro { get; set; }
        public string? ObserCambio { get; set; }
        public string? NumeroLiquidacion { get; set; }
        public decimal? TipoSolicitud { get; set; }
        public string? DescSolicitud { get; set; }
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
        public decimal NumeroEntrada { get; set; }

        //Datos Transaccion
        public decimal? IdTransaccion { get; set; }
        public string? UsuarioSup { get; set; }
        public DateTime? FechaAlta { get; set; }
        public string? ObservacionSup { get; set; }

        //Datos NotaNegativa
        public decimal IdEntrada { get; set; }
        public string? IdUsuario { get; set; }
        public DateTime? FechaAltaNT { get; set; }

        public class Movimientos
        {
            public string? CodUsuario { get; set; }
            public string? Nombre { get; set; }
            public DateTime? FechaOperacion { get; set; }
            public string? Estado { get; set; }

        }
        public class Titulares
        {
            public string? CodPersona { get; set; }
            public string? Nombre { get; set; }
            public DateTime? FechaRegistro { get; set; }

        }
        public class Asignaciones
        {

            public DateTime? FechaAsignacion { get; set; }
            public string? Nombre { get; set; }
            public string? Desasignado { get; set; }
            public string? Reingreso { get; set; }

        }

        public class Observacion
        {
            public string? UsuarioSup { get; set; }
            public DateTime? FechaAlta { get; set; }
            public string? ObservacionSup { get; set; }

        }

        public class NotaNegativa
        {
            public string? DescripNotaNegativa { get; set; }
            public string? NomPersona { get; set; }
            public DateTime? FechaAlta { get; set; }

        }
    }
}
