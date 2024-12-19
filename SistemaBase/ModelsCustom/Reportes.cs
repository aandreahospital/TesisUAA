using NuGet.Protocol;
using Org.BouncyCastle.Bcpg.OpenPgp;
using SistemaBase.Models;

namespace SistemaBase.ModelsCustom
{
    public class Reportes
    {
        public decimal? CodigoOficina { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public decimal NumeroEntradaDesde { get; set; }
        public decimal NumeroEntradaHasta { get; set; }
        public decimal NumeroEntrada { get; set; }
        public DateTime? FechaEntrada { get; set; }
        public string? NombreTitular { get; set; }
        public string? TipoSolicitud { get; set; }
        public string? Reingreso { get; set; }
        public string? Anulado { get; set; }
        public string? NroLiquidacion { get; set; }
        public string? Usuario { get; set; }
        public string? NombreUsuario { get; set; }
        public DateTime? FechaActual { get; set; }
        public decimal? TotalIngresado { get; set; }
        public DateTime? FechaAsignacion { get; set; }
        public string UsuarioSup { get; set; }
        public string DescripOficina { get; set; }
        public TimeOnly? HoraDesde { get; set; }
        public TimeOnly? HoraHasta { get; set; }

        public DateOnly? FechaDes { get; set; }
        public DateOnly? FechaHas { get; set; }

        #region Ingresados
        public int Inscripcion { get; set; }
        public int Adjudicacion { get; set; }
        public int Transferencia { get; set; }
        public int Reinscripcion { get; set; }
        public int Litis { get; set; }
        public int Duplicado { get; set; }
        public int Copia { get; set; }
        public int Permuta { get; set; }
        public int Donacion { get; set; }
        public int Usufructo { get; set; }
        public int Prenda { get; set; }
        public int Levantamiento { get; set; }
        public int Certificado { get; set; }
        public int Informe { get; set; }
        public int InformeJudicial { get; set; }

        public int AnotacionDeInscripcionPreventiva { get; set; }
        public int EmbargoPreventivo { get; set; }
        public int DacionPago { get; set; }
        public int EmbargoEjecutivo { get; set; }

        public int CertificadoCondominioDominio { get; set; }

        public int CertificadoCondominioDominioR { get; set; }


        public int Cancelacion { get; set; }
        public int CambioDenominacion { get; set; }
        public int Rectificacion { get; set; }
        public int CopiaJudicial { get; set; }
        public int Fianza { get; set; }

        public int ProDeIn_y_Con { get; set; }
        #endregion

        #region Reingresos
        public int InscripcionRe { get; set; }
        public int AdjudicacionRe { get; set; }

        public int TransferenciaRe { get; set; }
        public int PermutaRe { get; set; }

        public int ReinscripcionRe { get; set; }
        public int CopiaRe { get; set; }

        public int UsufructoRe { get; set; }
        public int DuplicadoRe { get; set; }

        public int PrendaRe { get; set; }
        public int LevantamientoRe { get; set; }
        public int CertificadoRe { get; set; }
        public int InformeRe { get; set; }
        public int InformeJudicialRe { get; set; }
        public int LitisRe { get; set; }

        public int AnotacionDeInscripcionPreventivaRe { get; set; }
        public int EmbargoPreventivoRe { get; set; }
        public int DacionPagoRe { get; set; }
        public int EmbargoEjecutivoRe { get; set; }
        public int CancelacionRe { get; set; }
        public int CambioDenominacionRe { get; set; }
        public int DonacionRe { get; set; }

        public int RectificacionRe { get; set; }
        public int CopiaJudicialRe { get; set; }
        public int FianzaRe { get; set; }

        public int ProDeIn_y_ConRe { get; set; }
        #endregion

        #region TRABAJOS FINALIZADOS PROPIEDADES
        public int Inscripcion1 { get; set; }

        public int Adjudicacion1 { get; set; }
        public int Transferencia1 { get; set; }
        public int Reinscripcion1 { get; set; }
        public int Duplicado1 { get; set; }
        public int Copia1 { get; set; }
        public int Permuta1 { get; set; }
        public int Donacion1 { get; set; }

        public int Usufructo1 { get; set; }

        public int Prenda1 { get; set; }

        public int Levantamiento1 { get; set; }

        public int Certificado1 { get; set; }

        public int Informe1 { get; set; }

        public int InformeJudicial1 { get; set; }

        public int AnotacionInscripcionPreventiva1 { get; set; }

        public int EmbargoPreventivo1 { get; set; }

        public int DacionPago1 { get; set; }

        public int CertificadoCondominioDominio1 { get; set; }


        public int Cancelacion1 { get; set; }

        public int Rectificacion1 { get; set; }

        public int CopiaJudicial1 { get; set; }


        public int Fianza1 { get; set; }

        public int ProhibicionInnovarContratar1 { get; set; }


        public int Litis1 { get; set; }
        public int EmbargoEjecutivo1 { get; set; }
        public int CambioDenominacion1 { get; set; }

        public int TotalF { get; set; }

        public int TotalInscriptos { get; set; }

        #endregion

        #region Propiedades para Observado
        public int InscripcionO { get; set; }
        public int AdjudicacionO { get; set; }
        public int TransferenciaO { get; set; }
        public int ReinscripcionO { get; set; }
        public int DuplicadoO { get; set; }
        public int CopiaO { get; set; }
        public int PermutaO { get; set; }
        public int DonacionO { get; set; }
        public int LitisO { get; set; }
        public int EmbargoEO { get; set; }
        public int CambioDO { get; set; }



        public int UsufructoO { get; set; }
        public int PrendaO { get; set; }
        public int levantamientoO { get; set; }
        public int CertificadoO { get; set; }
        public int InformeO { get; set; }
        public int InformeJudicialO { get; set; }
        public int AnotacionInscripcionPreventivoO { get; set; }
        public int EmbargoPreventivoO { get; set; }
        public int DacionPagoO { get; set; }
        public int CancelacionO { get; set; }
        public int RectificacionO { get; set; }
        public int CopiaJudicialO { get; set; }
        public int FianzaO { get; set; }
        public int ProhibicionInnovarContratarO { get; set; }


        public int TotalO { get; set; }
        #endregion

        #region Ingresados Online

        public int InscripcionOnline { get; set; }

        public int DuplicadoOnline { get; set; }

        public int CertificadoOnline { get; set; }

        public int InformeOnline { get; set; }

        public int TotalOnline { get; set; }

        #endregion

        public string? TipoUsuario { get; set; }


        #region JefaRegistral
        public int InscripcionFirma { get; set; }

        public int AdjudicacionFirma { get; set; }
        public int TransferenciaFirma { get; set; }
        public int ReinscripcionFirma { get; set; }
        public int DuplicadoFirma { get; set; }
        public int CopiaFirma { get; set; }
        public int PermutaFirma { get; set; }
        public int DonacionFirma { get; set; }
        public int TotalAprobadosFirma { get; set; }

        public int TotalAprobadosSup { get; set; }

        public int TotalMesaSalida { get; set; }

        #endregion
        public int TotalIn { get; set; }

        public int TotalRe { get; set; }

        public List<ResultadoReporte> Resultados { get; set; }
        public List<Usuario> UsuariosOPREG { get; set; }

        // Clase para almacenar los resultados del conteo de movimientos
        public class ResultadoReporte
        {
            public string Usuario { get; set; }
            public int RecepRegistrador { get; set; }
            public int AprobRegistrador { get; set; }
            public int NNRegistrador { get; set; }
            public int ObsRegistrador { get; set; }
            public string NombreUsuario { get; set; }
        }

        public string? Oficina { get; set; }
        public List<EntradasCargas>? Entradas { get; set; }
        public List<EnvioSigor>? EnvioSigor3 { get; set; }
        public List<ListadoCarga>? Listado { get; set; }
        public List<ListadoJefes>? ListadoParaJefes { get; set; }
        public List<FinalizadoRegistrador>? FinalizadoRegis { get; set; }
        public List<MesaSalida>? Salidas { get; set; }
        public string? ImageDataUri { get; set; }

        public class EntradasCargas
        {
            public decimal? NumeroEntrada { get; set; }
            public DateTime? FechaEntrada { get; set; }
            public string? NombreTitular { get; set; }
            public string? UsuarioEntrada { get; set; }
            public string? Reingreso { get; set; }
            public string? Anulado { get; set; }
            public string? TipoSolicitud { get; set; }
            public string? Usuario { get; set; }
            public string? NroLiquidacion { get; set; }
            public string? Oficina { get; set; }





        }
        public class ListadoCarga
        {
            public decimal? NumeroEntrada { get; set; }
            public DateTime? FechaEntrada { get; set; }
            public DateTime? FechaAsignacion { get; set; }
            public string? TipoSolicitud { get; set; }

        }
        public class FinalizadoRegistrador
        {
            public decimal? NumeroEntrada { get; set; }
            public DateTime? FechaEntrada { get; set; }
            public DateTime? FechaAsignacion { get; set; }
            public string? TipoSolicitud { get; set; }
            public string? Estado { get; set; }
            public DateTime? FechaFin { get; set; }

        }

        public class ListadoJefes
        {
            public decimal? NumeroEntrada { get; set; }
            public DateTime? FechaEntrada { get; set; }
            public DateTime? FechaAlta { get; set; }
            public string? TipoSolicitud { get; set; }
            public string? NomTitular { get; set; }
            public string? NomOperador { get; set; }
            public string? UsuarioSup { get; set; }
            public decimal? Estado { get; set; }

        }

        public class MesaSalida
        {
            public decimal? NumeroEntrada { get; set; }
            public DateTime? FechaEntrada { get; set; }
            public DateTime? FechaAlta { get; set; }
            public string? TipoSolicitud { get; set; }
            public string? Titular { get; set; }
            public string? NroBoleta { get; set; }
            public string? UsuarioSalida { get; set; }

        }
        public class EnvioSigor
        {
            public decimal? NumeroEntrada { get; set; }
            public string? Usuario { get; set; }
            public string? Contrasena { get; set; }
            public string? NomTitular { get; set; }
            public string? ApellTitular { get; set; }
            public string? TipoDocumento { get; set; }
            public string? NroDocumento { get; set; }
            public string? DigitoVerificador { get; set; }
            public string? Nacionalidad { get; set; }
            public string? NroMarca { get; set; }
            public string? MarcaImagen { get; set; }
            public string? TipoImagen { get; set; }
            public string? NroSenal { get; set; }
            public string? SenalImagen { get; set; }

        }

        // DECLARACIÓN DE VARIABLES PARA INFORME DE PRODUCCIÓN REGIONAL

        #region Mesa de Entrada

        public int InscripcionEnt { get; set; }
        public int InscripcionEntRe { get; set; }

        public int AdjudicacionEnt { get; set; }
        public int AdjudicacionEntRe { get; set; }

        public int TransferenciaEnt { get; set; }
        public int TransferenciaEntRe { get; set; }

        public int ReinscripcionEnt { get; set; }
        public int ReinscripcionEntRe { get; set; }

        public int LitisEnt { get; set; }
        public int LitisEntRe { get; set; }

        public int DuplicadoEnt { get; set; }
        public int DuplicadoEntRe { get; set; }

        public int CopiaEnt { get; set; }
        public int CopiaEntRe { get; set; }

        public int PermutaEnt { get; set; }
        public int PermutaEntRe { get; set; }

        public int DonacionEnt { get; set; }
        public int DonacionEntRe { get; set; }

        public int UsufructoEnt { get; set; }
        public int UsufructoEntRe { get; set; }

        public int PrendaEnt { get; set; }
        public int PrendaEntRe { get; set; }

        public int LevantamientoEnt { get; set; }
        public int LevantamientoEntRe { get; set; }

        public int CertificadoEnt { get; set; }
        public int CertificadoEntRe { get; set; }

        public int InformeEnt { get; set; }
        public int InformeEntRe { get; set; }

        public int InformeJudicialEnt { get; set; }
        public int InformeJudicialEntRe { get; set; }

        public int AnotacionDeInscripcionPreventivaEnt { get; set; }
        public int AnotacionDeInscripcionPreventivaEntRe { get; set; }

        public int EmbargoPreventivoEnt { get; set; }
        public int EmbargoPreventivoEntRe { get; set; }

        public int DacionPagoEnt { get; set; }
        public int DacionPagoEntRe { get; set; }

        public int EmbargoEjecutivoEnt { get; set; }
        public int EmbargoEjecutivoEntRe { get; set; }

        public int CertificadoCondominioDominioEnt { get; set; }
        public int CertificadoCondominioDominioEntRe { get; set; }

        public int CancelacionEnt { get; set; }
        public int CancelacionEntRe { get; set; }

        public int CambioDenominacionEnt { get; set; }
        public int CambioDenominacionEntRe { get; set; }

        public int RectificacionEnt { get; set; }
        public int RectificacionEntRe { get; set; }

        public int CopiaJudicialEnt { get; set; }
        public int CopiaJudicialEntRe { get; set; }

        public int FianzaEnt { get; set; }
        public int FianzaEntRe { get; set; }

        public int ProDeIn_y_ConEnt { get; set; }
        public int ProDeIn_y_ConEntRe { get; set; }

        public int TotalEnt { get; set; }
        public int TotalEntRe { get; set; }
        public int TotalMesaEntrada { get; set; }

        #endregion

        #region Envío de Entrada Div Regional

        public int InscripcionEnv { get; set; }
        public int InscripcionEnvRe { get; set; }

        public int AdjudicacionEnv { get; set; }
        public int AdjudicacionEnvRe { get; set; }

        public int TransferenciaEnv { get; set; }
        public int TransferenciaEnvRe { get; set; }

        public int ReinscripcionEnv { get; set; }
        public int ReinscripcionEnvRe { get; set; }

        public int LitisEnv { get; set; }
        public int LitisEnvRe { get; set; }

        public int DuplicadoEnv { get; set; }
        public int DuplicadoEnvRe { get; set; }

        public int CopiaEnv { get; set; }
        public int CopiaEnvRe { get; set; }

        public int PermutaEnv { get; set; }
        public int PermutaEnvRe { get; set; }

        public int DonacionEnv { get; set; }
        public int DonacionEnvRe { get; set; }

        public int UsufructoEnv { get; set; }
        public int UsufructoEnvRe { get; set; }

        public int PrendaEnv { get; set; }
        public int PrendaEnvRe { get; set; }

        public int LevantamientoEnv { get; set; }
        public int LevantamientoEnvRe { get; set; }

        public int CertificadoEnv { get; set; }
        public int CertificadoEnvRe { get; set; }

        public int InformeEnv { get; set; }
        public int InformeEnvRe { get; set; }

        public int InformeJudicialEnv { get; set; }
        public int InformeJudicialEnvRe { get; set; }

        public int AnotacionDeInscripcionPreventivaEnv { get; set; }
        public int AnotacionDeInscripcionPreventivaEnvRe { get; set; }

        public int EmbargoPreventivoEnv { get; set; }
        public int EmbargoPreventivoEnvRe { get; set; }

        public int DacionPagoEnv { get; set; }
        public int DacionPagoEnvRe { get; set; }

        public int EmbargoEjecutivoEnv { get; set; }
        public int EmbargoEjecutivoEnvRe { get; set; }

        public int CertificadoCondominioDominioEnv { get; set; }
        public int CertificadoCondominioDominioEnvRe { get; set; }

        public int CancelacionEnv { get; set; }
        public int CancelacionEnvRe { get; set; }

        public int CambioDenominacionEnv { get; set; }
        public int CambioDenominacionEnvRe { get; set; }

        public int RectificacionEnv { get; set; }
        public int RectificacionEnvRe { get; set; }

        public int CopiaJudicialEnv { get; set; }
        public int CopiaJudicialEnvRe { get; set; }

        public int FianzaEnv { get; set; }
        public int FianzaEnvRe { get; set; }

        public int ProDeIn_y_ConEnv { get; set; }
        public int ProDeIn_y_ConEnvRe { get; set; }

        public int TotalEnv { get; set; }
        public int TotalEnvRe { get; set; }
        public int TotalEnviadoRegional { get; set; }

        #endregion

        #region Recepción de Entrada Div regional

        public int InscripcionRecep { get; set; }
        public int InscripcionRecepRe { get; set; }

        public int AdjudicacionRecep { get; set; }
        public int AdjudicacionRecepRe { get; set; }

        public int TransferenciaRecep { get; set; }
        public int TransferenciaRecepRe { get; set; }

        public int ReinscripcionRecep { get; set; }
        public int ReinscripcionRecepRe { get; set; }

        public int LitisRecep { get; set; }
        public int LitisRecepRe { get; set; }

        public int DuplicadoRecep { get; set; }
        public int DuplicadoRecepRe { get; set; }

        public int CopiaRecep { get; set; }
        public int CopiaRecepRe { get; set; }

        public int PermutaRecep { get; set; }
        public int PermutaRecepRe { get; set; }

        public int DonacionRecep { get; set; }
        public int DonacionRecepRe { get; set; }

        public int UsufructoRecep { get; set; }
        public int UsufructoRecepRe { get; set; }

        public int PrendaRecep { get; set; }
        public int PrendaRecepRe { get; set; }

        public int LevantamientoRecep { get; set; }
        public int LevantamientoRecepRe { get; set; }

        public int CertificadoRecep { get; set; }
        public int CertificadoRecepRe { get; set; }

        public int InformeRecep { get; set; }
        public int InformeRecepRe { get; set; }

        public int InformeJudicialRecep { get; set; }
        public int InformeJudicialRecepRe { get; set; }

        public int AnotacionDeInscripcionPreventivaRecep { get; set; }
        public int AnotacionDeInscripcionPreventivaRecepRe { get; set; }

        public int EmbargoPreventivoRecep { get; set; }
        public int EmbargoPreventivoRecepRe { get; set; }

        public int DacionPagoRecep { get; set; }
        public int DacionPagoRecepRe { get; set; }

        public int EmbargoEjecutivoRecep { get; set; }
        public int EmbargoEjecutivoRecepRe { get; set; }

        public int CertificadoCondominioDominioRecep { get; set; }
        public int CertificadoCondominioDominioRecepRe { get; set; }

        public int CancelacionRecep { get; set; }
        public int CancelacionRecepRe { get; set; }

        public int CambioDenominacionRecep { get; set; }
        public int CambioDenominacionRecepRe { get; set; }

        public int RectificacionRecep { get; set; }
        public int RectificacionRecepRe { get; set; }

        public int CopiaJudicialRecep { get; set; }
        public int CopiaJudicialRecepRe { get; set; }

        public int FianzaRecep { get; set; }
        public int FianzaRecepRe { get; set; }

        public int ProDeIn_y_ConRecep { get; set; }
        public int ProDeIn_y_ConRecepRe { get; set; }

        public int TotalRecep { get; set; }
        public int TotalRecepRe { get; set; }
        public int TotalRecepEntrada { get; set; }

        #endregion

        #region Remisión de Entrada Div Regional Diseño

        public int InscripcionReDise { get; set; }
        public int InscripcionReDiseRe { get; set; }

        public int TotalReDise { get; set; }
        public int TotalReDiseRe { get; set; }
        public int TotalRemisionDiseño { get; set; }

        #endregion

        #region Recepción de Diseños Div Regional

        public int InscripcionDise { get; set; }
        public int InscripcionDiseRe { get; set; }

        public int TotalDise { get; set; }
        public int TotalDiseRe { get; set; }
        public int TotalRecepDiseño { get; set; }

        #endregion

        #region Asignaciones

        public int InscripcionAsig { get; set; }
        public int InscripcionAsigRe { get; set; }

        public int AdjudicacionAsig { get; set; }
        public int AdjudicacionAsigRe { get; set; }

        public int TransferenciaAsig { get; set; }
        public int TransferenciaAsigRe { get; set; }

        public int ReinscripcionAsig { get; set; }
        public int ReinscripcionAsigRe { get; set; }

        public int LitisAsig { get; set; }
        public int LitisAsigRe { get; set; }

        public int DuplicadoAsig { get; set; }
        public int DuplicadoAsigRe { get; set; }

        public int CopiaAsig { get; set; }
        public int CopiaAsigRe { get; set; }

        public int PermutaAsig { get; set; }
        public int PermutaAsigRe { get; set; }

        public int DonacionAsig { get; set; }
        public int DonacionAsigRe { get; set; }

        public int UsufructoAsig { get; set; }
        public int UsufructoAsigRe { get; set; }

        public int PrendaAsig { get; set; }
        public int PrendaAsigRe { get; set; }

        public int LevantamientoAsig { get; set; }
        public int LevantamientoAsigRe { get; set; }

        public int CertificadoAsig { get; set; }
        public int CertificadoAsigRe { get; set; }

        public int InformeAsig { get; set; }
        public int InformeAsigRe { get; set; }

        public int InformeJudicialAsig { get; set; }
        public int InformeJudicialAsigRe { get; set; }

        public int AnotacionDeInscripcionPreventivaAsig { get; set; }
        public int AnotacionDeInscripcionPreventivaAsigRe { get; set; }

        public int EmbargoPreventivoAsig { get; set; }
        public int EmbargoPreventivoAsigRe { get; set; }

        public int DacionPagoAsig { get; set; }
        public int DacionPagoAsigRe { get; set; }

        public int EmbargoEjecutivoAsig { get; set; }
        public int EmbargoEjecutivoAsigRe { get; set; }

        public int CertificadoCondominioDominioAsig { get; set; }
        public int CertificadoCondominioDominioAsigRe { get; set; }

        public int CancelacionAsig { get; set; }
        public int CancelacionAsigRe { get; set; }

        public int CambioDenominacionAsig { get; set; }
        public int CambioDenominacionAsigRe { get; set; }

        public int RectificacionAsig { get; set; }
        public int RectificacionAsigRe { get; set; }

        public int CopiaJudicialAsig { get; set; }
        public int CopiaJudicialAsigRe { get; set; }

        public int FianzaAsig { get; set; }
        public int FianzaAsigRe { get; set; }

        public int ProDeIn_y_ConAsig { get; set; }
        public int ProDeIn_y_ConAsigRe { get; set; }

        public int TotalAsig { get; set; }
        public int TotalAsigRe { get; set; }
        public int TotalAsignados { get; set; }

        #endregion

        #region Reasignaciones

        public int InscripcionReAsig { get; set; }
        public int InscripcionReAsigRe { get; set; }

        public int AdjudicacionReAsig { get; set; }
        public int AdjudicacionReAsigRe { get; set; }

        public int TransferenciaReAsig { get; set; }
        public int TransferenciaReAsigRe { get; set; }

        public int ReinscripcionReAsig { get; set; }
        public int ReinscripcionReAsigRe { get; set; }

        public int LitisReAsig { get; set; }
        public int LitisReAsigRe { get; set; }

        public int DuplicadoReAsig { get; set; }
        public int DuplicadoReAsigRe { get; set; }

        public int CopiaReAsig { get; set; }
        public int CopiaReAsigRe { get; set; }

        public int PermutaReAsig { get; set; }
        public int PermutaReAsigRe { get; set; }

        public int DonacionReAsig { get; set; }
        public int DonacionReAsigRe { get; set; }

        public int UsufructoReAsig { get; set; }
        public int UsufructoReAsigRe { get; set; }

        public int PrendaReAsig { get; set; }
        public int PrendaReAsigRe { get; set; }

        public int LevantamientoReAsig { get; set; }
        public int LevantamientoReAsigRe { get; set; }

        public int CertificadoReAsig { get; set; }
        public int CertificadoReAsigRe { get; set; }

        public int InformeReAsig { get; set; }
        public int InformeReAsigRe { get; set; }

        public int InformeJudicialReAsig { get; set; }
        public int InformeJudicialReAsigRe { get; set; }

        public int AnotacionDeInscripcionPreventivaReAsig { get; set; }
        public int AnotacionDeInscripcionPreventivaReAsigRe { get; set; }

        public int EmbargoPreventivoReAsig { get; set; }
        public int EmbargoPreventivoReAsigRe { get; set; }

        public int DacionPagoReAsig { get; set; }
        public int DacionPagoReAsigRe { get; set; }

        public int EmbargoEjecutivoReAsig { get; set; }
        public int EmbargoEjecutivoReAsigRe { get; set; }

        public int CertificadoCondominioDominioReAsig { get; set; }
        public int CertificadoCondominioDominioReAsigRe { get; set; }

        public int CancelacionReAsig { get; set; }
        public int CancelacionReAsigRe { get; set; }

        public int CambioDenominacionReAsig { get; set; }
        public int CambioDenominacionReAsigRe { get; set; }

        public int RectificacionReAsig { get; set; }
        public int RectificacionReAsigRe { get; set; }

        public int CopiaJudicialReAsig { get; set; }
        public int CopiaJudicialReAsigRe { get; set; }

        public int FianzaReAsig { get; set; }
        public int FianzaReAsigRe { get; set; }

        public int ProDeIn_y_ConReAsig { get; set; }
        public int ProDeIn_y_ConReAsigRe { get; set; }

        public int TotalReAsig { get; set; }
        public int TotalReAsigRe { get; set; }
        public int TotalReasignados { get; set; }

        #endregion

        #region Recepción de Doc Firmados Div Regional

        public int InscripcionFirm { get; set; }
        public int InscripcionFirmRe { get; set; }

        public int AdjudicacionFirm { get; set; }
        public int AdjudicacionFirmRe { get; set; }

        public int TransferenciaFirm { get; set; }
        public int TransferenciaFirmRe { get; set; }

        public int ReinscripcionFirm { get; set; }
        public int ReinscripcionFirmRe { get; set; }

        public int LitisFirm { get; set; }
        public int LitisFirmRe { get; set; }

        public int DuplicadoFirm { get; set; }
        public int DuplicadoFirmRe { get; set; }

        public int CopiaFirm { get; set; }
        public int CopiaFirmRe { get; set; }

        public int PermutaFirm { get; set; }
        public int PermutaFirmRe { get; set; }

        public int DonacionFirm { get; set; }
        public int DonacionFirmRe { get; set; }

        public int UsufructoFirm { get; set; }
        public int UsufructoFirmRe { get; set; }

        public int PrendaFirm { get; set; }
        public int PrendaFirmRe { get; set; }

        public int LevantamientoFirm { get; set; }
        public int LevantamientoFirmRe { get; set; }

        public int CertificadoFirm { get; set; }
        public int CertificadoFirmRe { get; set; }

        public int InformeFirm { get; set; }
        public int InformeFirmRe { get; set; }

        public int InformeJudicialFirm { get; set; }
        public int InformeJudicialFirmRe { get; set; }

        public int AnotacionDeInscripcionPreventivaFirm { get; set; }
        public int AnotacionDeInscripcionPreventivaFirmRe { get; set; }

        public int EmbargoPreventivoFirm { get; set; }
        public int EmbargoPreventivoFirmRe { get; set; }

        public int DacionPagoFirm { get; set; }
        public int DacionPagoFirmRe { get; set; }

        public int EmbargoEjecutivoFirm { get; set; }
        public int EmbargoEjecutivoFirmRe { get; set; }

        public int CertificadoCondominioDominioFirm { get; set; }
        public int CertificadoCondominioDominioFirmRe { get; set; }

        public int CancelacionFirm { get; set; }
        public int CancelacionFirmRe { get; set; }

        public int CambioDenominacionFirm { get; set; }
        public int CambioDenominacionFirmRe { get; set; }

        public int RectificacionFirm { get; set; }
        public int RectificacionFirmRe { get; set; }

        public int CopiaJudicialFirm { get; set; }
        public int CopiaJudicialFirmRe { get; set; }

        public int FianzaFirm { get; set; }
        public int FianzaFirmRe { get; set; }

        public int ProDeIn_y_ConFirm { get; set; }
        public int ProDeIn_y_ConFirmRe { get; set; }

        public int TotalFirm { get; set; }
        public int TotalFirmRe { get; set; }
        public int TotalDocFirmadosReg { get; set; }

        #endregion

        #region Envío a Sección Regional

        public int InscripcionEnvSec { get; set; }
        public int InscripcionEnvSecRe { get; set; }

        public int AdjudicacionEnvSec { get; set; }
        public int AdjudicacionEnvSecRe { get; set; }

        public int TransferenciaEnvSec { get; set; }
        public int TransferenciaEnvSecRe { get; set; }

        public int ReinscripcionEnvSec { get; set; }
        public int ReinscripcionEnvSecRe { get; set; }

        public int LitisEnvSec { get; set; }
        public int LitisEnvSecRe { get; set; }

        public int DuplicadoEnvSec { get; set; }
        public int DuplicadoEnvSecRe { get; set; }

        public int CopiaEnvSec { get; set; }
        public int CopiaEnvSecRe { get; set; }

        public int PermutaEnvSec { get; set; }
        public int PermutaEnvSecRe { get; set; }

        public int DonacionEnvSec { get; set; }
        public int DonacionEnvSecRe { get; set; }

        public int UsufructoEnvSec { get; set; }
        public int UsufructoEnvSecRe { get; set; }

        public int PrendaEnvSec { get; set; }
        public int PrendaEnvSecRe { get; set; }

        public int LevantamientoEnvSec { get; set; }
        public int LevantamientoEnvSecRe { get; set; }

        public int CertificadoEnvSec { get; set; }
        public int CertificadoEnvSecRe { get; set; }

        public int InformeEnvSec { get; set; }
        public int InformeEnvSecRe { get; set; }

        public int InformeJudicialEnvSec { get; set; }
        public int InformeJudicialEnvSecRe { get; set; }

        public int AnotacionDeInscripcionPreventivaEnvSec { get; set; }
        public int AnotacionDeInscripcionPreventivaEnvSecRe { get; set; }

        public int EmbargoPreventivoEnvSec { get; set; }
        public int EmbargoPreventivoEnvSecRe { get; set; }

        public int DacionPagoEnvSec { get; set; }
        public int DacionPagoEnvSecRe { get; set; }

        public int EmbargoEjecutivoEnvSec { get; set; }
        public int EmbargoEjecutivoEnvSecRe { get; set; }

        public int CertificadoCondominioDominioEnvSec { get; set; }
        public int CertificadoCondominioDominioEnvSecRe { get; set; }

        public int CancelacionEnvSec { get; set; }
        public int CancelacionEnvSecRe { get; set; }

        public int CambioDenominacionEnvSec { get; set; }
        public int CambioDenominacionEnvSecRe { get; set; }

        public int RectificacionEnvSec { get; set; }
        public int RectificacionEnvSecRe { get; set; }

        public int CopiaJudicialEnvSec { get; set; }
        public int CopiaJudicialEnvSecRe { get; set; }

        public int FianzaEnvSec { get; set; }
        public int FianzaEnvSecRe { get; set; }

        public int ProDeIn_y_ConEnvSec { get; set; }
        public int ProDeIn_y_ConEnvSecRe { get; set; }

        public int TotalEnvSec { get; set; }
        public int TotalEnvSecRe { get; set; }
        public int TotalEnvSecRegional { get; set; }

        #endregion

        #region Recepción Mesa de Salida Sección Regional

        public int InscripcionSaliReg { get; set; }
        public int InscripcionSaliRegRe { get; set; }

        public int AdjudicacionSaliReg { get; set; }
        public int AdjudicacionSaliRegRe { get; set; }

        public int TransferenciaSaliReg { get; set; }
        public int TransferenciaSaliRegRe { get; set; }

        public int ReinscripcionSaliReg { get; set; }
        public int ReinscripcionSaliRegRe { get; set; }

        public int LitisSaliReg { get; set; }
        public int LitisSaliRegRe { get; set; }

        public int DuplicadoSaliReg { get; set; }
        public int DuplicadoSaliRegRe { get; set; }

        public int CopiaSaliReg { get; set; }
        public int CopiaSaliRegRe { get; set; }

        public int PermutaSaliReg { get; set; }
        public int PermutaSaliRegRe { get; set; }

        public int DonacionSaliReg { get; set; }
        public int DonacionSaliRegRe { get; set; }

        public int UsufructoSaliReg { get; set; }
        public int UsufructoSaliRegRe { get; set; }

        public int PrendaSaliReg { get; set; }
        public int PrendaSaliRegRe { get; set; }

        public int LevantamientoSaliReg { get; set; }
        public int LevantamientoSaliRegRe { get; set; }

        public int CertificadoSaliReg { get; set; }
        public int CertificadoSaliRegRe { get; set; }

        public int InformeSaliReg { get; set; }
        public int InformeSaliRegRe { get; set; }

        public int InformeJudicialSaliReg { get; set; }
        public int InformeJudicialSaliRegRe { get; set; }

        public int AnotacionDeInscripcionPreventivaSaliReg { get; set; }
        public int AnotacionDeInscripcionPreventivaSaliRegRe { get; set; }

        public int EmbargoPreventivoSaliReg { get; set; }
        public int EmbargoPreventivoSaliRegRe { get; set; }

        public int DacionPagoSaliReg { get; set; }
        public int DacionPagoSaliRegRe { get; set; }

        public int EmbargoEjecutivoSaliReg { get; set; }
        public int EmbargoEjecutivoSaliRegRe { get; set; }

        public int CertificadoCondominioDominioSaliReg { get; set; }
        public int CertificadoCondominioDominioSaliRegRe { get; set; }

        public int CancelacionSaliReg { get; set; }
        public int CancelacionSaliRegRe { get; set; }

        public int CambioDenominacionSaliReg { get; set; }
        public int CambioDenominacionSaliRegRe { get; set; }

        public int RectificacionSaliReg { get; set; }
        public int RectificacionSaliRegRe { get; set; }

        public int CopiaJudicialSaliReg { get; set; }
        public int CopiaJudicialSaliRegRe { get; set; }

        public int FianzaSaliReg { get; set; }
        public int FianzaSaliRegRe { get; set; }

        public int ProDeIn_y_ConSaliReg { get; set; }
        public int ProDeIn_y_ConSaliRegRe { get; set; }

        public int TotalSaliReg { get; set; }
        public int TotalSaliRegRe { get; set; }
        public int TotalSalidaRegional { get; set; }

        #endregion

        #region Retirado

        public int InscripcionRetir { get; set; }
        public int InscripcionRetirRe { get; set; }

        public int AdjudicacionRetir { get; set; }
        public int AdjudicacionRetirRe { get; set; }

        public int TransferenciaRetir { get; set; }
        public int TransferenciaRetirRe { get; set; }

        public int ReinscripcionRetir { get; set; }
        public int ReinscripcionRetirRe { get; set; }

        public int LitisRetir { get; set; }
        public int LitisRetirRe { get; set; }

        public int DuplicadoRetir { get; set; }
        public int DuplicadoRetirRe { get; set; }

        public int CopiaRetir { get; set; }
        public int CopiaRetirRe { get; set; }

        public int PermutaRetir { get; set; }
        public int PermutaRetirRe { get; set; }

        public int DonacionRetir { get; set; }
        public int DonacionRetirRe { get; set; }

        public int UsufructoRetir { get; set; }
        public int UsufructoRetirRe { get; set; }

        public int PrendaRetir { get; set; }
        public int PrendaRetirRe { get; set; }

        public int LevantamientoRetir { get; set; }
        public int LevantamientoRetirRe { get; set; }

        public int CertificadoRetir { get; set; }
        public int CertificadoRetirRe { get; set; }

        public int InformeRetir { get; set; }
        public int InformeRetirRe { get; set; }

        public int InformeJudicialRetir { get; set; }
        public int InformeJudicialRetirRe { get; set; }

        public int AnotacionDeInscripcionPreventivaRetir { get; set; }
        public int AnotacionDeInscripcionPreventivaRetirRe { get; set; }

        public int EmbargoPreventivoRetir { get; set; }
        public int EmbargoPreventivoRetirRe { get; set; }

        public int DacionPagoRetir { get; set; }
        public int DacionPagoRetirRe { get; set; }

        public int EmbargoEjecutivoRetir { get; set; }
        public int EmbargoEjecutivoRetirRe { get; set; }

        public int CertificadoCondominioDominioRetir { get; set; }
        public int CertificadoCondominioDominioRetirRe { get; set; }

        public int CancelacionRetir { get; set; }
        public int CancelacionRetirRe { get; set; }

        public int CambioDenominacionRetir { get; set; }
        public int CambioDenominacionRetirRe { get; set; }

        public int RectificacionRetir { get; set; }
        public int RectificacionRetirRe { get; set; }

        public int CopiaJudicialRetir { get; set; }
        public int CopiaJudicialRetirRe { get; set; }

        public int FianzaRetir { get; set; }
        public int FianzaRetirRe { get; set; }

        public int ProDeIn_y_ConRetir { get; set; }
        public int ProDeIn_y_ConRetirRe { get; set; }

        public int TotalRetir { get; set; }
        public int TotalRetirRe { get; set; }
        public int TotalRetirado { get; set; }

        #endregion

        #region Envío Triplicado

        public int InscripcionTrip { get; set; }
        public int InscripcionTripRe { get; set; }

        public int AdjudicacionTrip { get; set; }
        public int AdjudicacionTripRe { get; set; }

        public int TransferenciaTrip { get; set; }
        public int TransferenciaTripRe { get; set; }

        public int ReinscripcionTrip { get; set; }
        public int ReinscripcionTripRe { get; set; }

        public int LitisTrip { get; set; }
        public int LitisTripRe { get; set; }

        public int DuplicadoTrip { get; set; }
        public int DuplicadoTripRe { get; set; }

        public int CopiaTrip { get; set; }
        public int CopiaTripRe { get; set; }

        public int PermutaTrip { get; set; }
        public int PermutaTripRe { get; set; }

        public int DonacionTrip { get; set; }
        public int DonacionTripRe { get; set; }

        public int UsufructoTrip { get; set; }
        public int UsufructoTripRe { get; set; }

        public int PrendaTrip { get; set; }
        public int PrendaTripRe { get; set; }

        public int LevantamientoTrip { get; set; }
        public int LevantamientoTripRe { get; set; }

        public int CertificadoTrip { get; set; }
        public int CertificadoTripRe { get; set; }

        public int InformeTrip { get; set; }
        public int InformeTripRe { get; set; }

        public int InformeJudicialTrip { get; set; }
        public int InformeJudicialTripRe { get; set; }

        public int AnotacionDeInscripcionPreventivaTrip { get; set; }
        public int AnotacionDeInscripcionPreventivaTripRe { get; set; }

        public int EmbargoPreventivoTrip { get; set; }
        public int EmbargoPreventivoTripRe { get; set; }

        public int DacionPagoTrip { get; set; }
        public int DacionPagoTripRe { get; set; }

        public int EmbargoEjecutivoTrip { get; set; }
        public int EmbargoEjecutivoTripRe { get; set; }

        public int CertificadoCondominioDominioTrip { get; set; }
        public int CertificadoCondominioDominioTripRe { get; set; }

        public int CancelacionTrip { get; set; }
        public int CancelacionTripRe { get; set; }

        public int CambioDenominacionTrip { get; set; }
        public int CambioDenominacionTripRe { get; set; }

        public int RectificacionTrip { get; set; }
        public int RectificacionTripRe { get; set; }

        public int CopiaJudicialTrip { get; set; }
        public int CopiaJudicialTripRe { get; set; }

        public int FianzaTrip { get; set; }
        public int FianzaTripRe { get; set; }

        public int ProDeIn_y_ConTrip { get; set; }
        public int ProDeIn_y_ConTripRe { get; set; }

        public int TotalTrip { get; set; }
        public int TotalTripRe { get; set; }
        public int TotalTriplicado { get; set; }

        #endregion

        // DECLARACIÓN DE VARIABLES PARA INFORME DE PRODUCCIÓN MENSUAL

        #region INFORME REGISTRAL

        public int AprobRegistrador { get; set; }
        public int NNRegistrador { get; set; }
        public int ObsRegistrador { get; set; }

        #endregion

        #region SOLICITUDES REGISTRAL

        public int InscripcionFinal { get; set; }
        public int InscripcionFinalRe { get; set; }

        public int AdjudicacionFinal { get; set; }
        public int AdjudicacionFinalRe { get; set; }

        public int TransferenciaFinal { get; set; }
        public int TransferenciaFinalRe { get; set; }

        public int ReinscripcionFinal { get; set; }
        public int ReinscripcionFinalRe { get; set; }

        public int LitisFinal { get; set; }
        public int LitisFinalRe { get; set; }

        public int DuplicadoFinal { get; set; }
        public int DuplicadoFinalRe { get; set; }

        public int CopiaFinal { get; set; }
        public int CopiaFinalRe { get; set; }

        public int PermutaFinal { get; set; }
        public int PermutaFinalRe { get; set; }

        public int DonacionFinal { get; set; }
        public int DonacionFinalRe { get; set; }

        public int UsufructoFinal { get; set; }
        public int UsufructoFinalRe { get; set; }

        public int PrendaFinal { get; set; }
        public int PrendaFinalRe { get; set; }

        public int LevantamientoFinal { get; set; }
        public int LevantamientoFinalRe { get; set; }

        public int CertificadoFinal { get; set; }
        public int CertificadoFinalRe { get; set; }

        public int InformeFinal { get; set; }
        public int InformeFinalRe { get; set; }

        public int InformeJudicialFinal { get; set; }
        public int InformeJudicialFinalRe { get; set; }

        public int AnotacionDeInscripcionPreventivaFinal { get; set; }
        public int AnotacionDeInscripcionPreventivaFinalRe { get; set; }

        public int EmbargoPreventivoFinal { get; set; }
        public int EmbargoPreventivoFinalRe { get; set; }

        public int DacionPagoFinal { get; set; }
        public int DacionPagoFinalRe { get; set; }

        public int EmbargoEjecutivoFinal { get; set; }
        public int EmbargoEjecutivoFinalRe { get; set; }

        public int CertificadoCondominioDominioFinal { get; set; }
        public int CertificadoCondominioDominioFinalRe { get; set; }

        public int CancelacionFinal { get; set; }
        public int CancelacionFinalRe { get; set; }

        public int CambioDenominacionFinal { get; set; }
        public int CambioDenominacionFinalRe { get; set; }

        public int RectificacionFinal { get; set; }
        public int RectificacionFinalRe { get; set; }

        public int CopiaJudicialFinal { get; set; }
        public int CopiaJudicialFinalRe { get; set; }

        public int FianzaFinal { get; set; }
        public int FianzaFinalRe { get; set; }

        public int ProDeIn_y_ConFinal { get; set; }
        public int ProDeIn_y_ConFinalRe { get; set; }

        public int TotalFinal { get; set; }
        public int TotalFinalRe { get; set; }

        #endregion

        #region INFORME DE DISEÑO
        public int TotalDisenho { get; set; }
        public int TotalAsigDis { get; set; }
        public int TotalDeAsigDis { get; set; }
        public int TotalDisenhador { get; set; }
        public int TotalFinDise { get; set; }
        public int TotalAproDis { get; set; }
        #endregion

        #region INFORME MESA DE ENTRADA
        public int TotalAsignado { get; set; }
        public int TotalRecibidoMesaSalida { get; set; }
        public int TotalRecepcionDisenho { get; set; }
        public int TotalEnviadoDisenho { get; set; }
        public int TotalEnviadoArchivo { get; set; }
        public int TotalReAsignado { get; set; }
        public int TotalEntradaOnline { get; set; }
        #endregion

        #region INFORME DE SUPERVISIÓN

        public int TotalRecJefSup { get; set; }
        public int InscripcionJS { get; set; }
        public int TotalAsigJefSup { get; set; }
        public int TotalDesAsigJefSup { get; set; }
        public int InscripcionDesAsig { get; set; }
        public int TotalRecSup { get; set; }
        public int InscripcionRecSup { get; set; }
        public int TotalAproSup { get; set; }
        public int InscripcionAproSup { get; set; }
        public int TotalRechaSup { get; set; }
        public int InscripcionRechaSup { get; set; }
        public int TotalAproJefSup { get; set; }
        public int InscripcionAproJefSup { get; set; }
        public int TotalRechaJefSup { get; set; }
        public int InscripcionRechaJefSup { get; set; }

        #endregion
    }

}
