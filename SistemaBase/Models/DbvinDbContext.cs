using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SistemaBase.ModelsCustom;
using SistemaBase.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection.Emit;

namespace SistemaBase.Models
{
    public partial class DbvinDbContext : DbContext
    {
        public DbvinDbContext()
        {
        }
        public DbvinDbContext(DbContextOptions<DbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccesosGrupo> AccesosGrupos { get; set; } = null!;
        public virtual DbSet<AccesosRrhh> AccesosRrhhs { get; set; } =null!;
        public virtual DbSet<ActivPersona> ActivPersonas { get; set; } = null!;
        public virtual DbSet<ActividadesEcon> ActividadesEcons { get; set; } = null!;
        public virtual DbSet<AgendaDiarium> AgendaDiaria { get; set; } = null!;
        public virtual DbSet<Area> Areas { get; set; } = null!;
        public virtual DbSet<AreasUsuario> AreasUsuarios { get; set; } = null!;
        public virtual DbSet<AuditoriaRep> AuditoriaReps { get; set; } = null!;
        public virtual DbSet<Auditorium> Auditoria { get; set; } = null!;
        public virtual DbSet<AutorizadosParaPedido> AutorizadosParaPedidos { get; set; } = null!;
        public virtual DbSet<AvDepartamento> AvDepartamentos { get; set; } = null!;
        public virtual DbSet<AvEstablecimiento> AvEstablecimientos { get; set; } = null!;
        public virtual DbSet<AvEstpropie> AvEstpropies { get; set; } = null!;
        public virtual DbSet<AvFinalidade> AvFinalidades { get; set; } = null!;
        public virtual DbSet<AvLocalidade> AvLocalidades { get; set; } = null!;
        public virtual DbSet<AvRegionale> AvRegionales { get; set; } = null!;
        public virtual DbSet<AvTipoEstablecimiento> AvTipoEstablecimientos { get; set; } = null!;
        public virtual DbSet<AvZona> AvZonas { get; set; } = null!;
        public virtual DbSet<Barrio> Barrios { get; set; } = null!;
        public virtual DbSet<Calendario> Calendarios { get; set; } = null!;
        public virtual DbSet<Ciudade> Ciudades { get; set; } = null!;
        public virtual DbSet<CmProvee> CmProvees { get; set; } = null!;
        public virtual DbSet<Departamento> Departamentos { get; set; } = null!;
        public virtual DbSet<Deptos1> Deptos1s { get; set; } = null!;
        public virtual DbSet<DirecPersona> DirecPersonas { get; set; } = null!;
        public virtual DbSet<Dpto> Dptos { get; set; } = null!;
        public virtual DbSet<Empresa> Empresas { get; set; } = null!;
        public virtual DbSet<EstadosCivile> EstadosCiviles { get; set; } = null!;
        public virtual DbSet<Feriado> Feriados { get; set; } = null!;
        public virtual DbSet<Forma> Formas { get; set; } = null!;
        public virtual DbSet<Funcionario> Funcionarios { get; set; } = null!;
        public virtual DbSet<GruposEmail> GruposEmails { get; set; } = null!;
        public virtual DbSet<GruposUsuario> GruposUsuarios { get; set; } = null!;
        public virtual DbSet<Hijo> Hijos { get; set; } = null!;
        public virtual DbSet<HistComprobante> HistComprobantes { get; set; } = null!;
        public virtual DbSet<HistDocumento> HistDocumentos { get; set; } = null!;
        public virtual DbSet<IdentPersona> IdentPersonas { get; set; } = null!;
        public virtual DbSet<Identificacione> Identificaciones { get; set; } = null!;
        public virtual DbSet<Inscripcion> Inscripcions { get; set; } = null!;
        public virtual DbSet<Lugar> Lugars { get; set; } = null!;
        public virtual DbSet<MalasReferencia> MalasReferencias { get; set; } = null!;
        public virtual DbSet<Materium> Materia { get; set; } = null!;
        public virtual DbSet<Memito> Memitos { get; set; } = null!;
        public virtual DbSet<Mensaje> Mensajes { get; set; } = null!;
        public virtual DbSet<MesaEntradaViej> MesaEntradaViejs { get; set; } = null!;
        public virtual DbSet<Mese> Meses { get; set; } = null!;
        public virtual DbSet<Modulo> Modulos { get; set; } = null!;
        public virtual DbSet<Moneda> Monedas { get; set; } = null!;
        public virtual DbSet<MotivosMalRefer> MotivosMalRefers { get; set; } = null!;
        public virtual DbSet<MsssmaDmNckeyColumn> MsssmaDmNckeyColumns { get; set; } = null!;
        public virtual DbSet<MsssmaDmTable> MsssmaDmTables { get; set; } = null!;
        public virtual DbSet<Nextbook> Nextbooks { get; set; } = null!;
        public virtual DbSet<NivelEstudio> NivelEstudios { get; set; } = null!;
        public virtual DbSet<NumeroLetra> NumeroLetras { get; set; } = null!;
        public virtual DbSet<OeClCoejecutor> OeClCoejecutors { get; set; } = null!;
        public virtual DbSet<Oficina> Oficinas { get; set; } = null!;
        public virtual DbSet<OpcionesParametro> OpcionesParametros { get; set; } = null!;
        public virtual DbSet<Pai> Pais { get; set; } = null!;
        public virtual DbSet<Paise> Paises { get; set; } = null!;
        public virtual DbSet<ParametrosGenerale> ParametrosGenerales { get; set; } = null!;
        public virtual DbSet<Pasatiempo> Pasatiempos { get; set; } = null!;
        public virtual DbSet<PermisosOpcione> PermisosOpciones { get; set; } = null!;
        public virtual DbSet<Persona> Personas { get; set; } = null!;
        public virtual DbSet<PersonasDup> PersonasDups { get; set; } = null!;
        public virtual DbSet<PjMarca> PjMarcas { get; set; } = null!;
        public virtual DbSet<PlanTable> PlanTables { get; set; } = null!;
        public virtual DbSet<PlsqlProfilerDatum> PlsqlProfilerData { get; set; } = null!;
        public virtual DbSet<PlsqlProfilerRun> PlsqlProfilerRuns { get; set; } = null!;
        public virtual DbSet<PlsqlProfilerUnit> PlsqlProfilerUnits { get; set; } = null!;
        public virtual DbSet<PreMaPrestamo> PreMaPrestamos { get; set; } = null!;
        public virtual DbSet<Profesione> Profesiones { get; set; } = null!;
        public virtual DbSet<PropiedadesXPersona> PropiedadesXPersonas { get; set; } = null!;
        public virtual DbSet<Propietario> Propietarios { get; set; } = null!;
        public virtual DbSet<Provincia> Provincias { get; set; } = null!;
        public virtual DbSet<ProvisorioCab> ProvisorioCabs { get; set; } = null!;
        public virtual DbSet<RmAnulacionesMarca> RmAnulacionesMarcas { get; set; } = null!;
        public virtual DbSet<RmAsigDi> RmAsigDis { get; set; } = null!;
        public virtual DbSet<RmAsignacione> RmAsignaciones { get; set; } = null!;
        public virtual DbSet<RmAutorizante> RmAutorizantes { get; set; } = null!;
        public virtual DbSet<RmBoletasMarca> RmBoletasMarcas { get; set; } = null!;
        public virtual DbSet<RmCambiosEstado> RmCambiosEstados { get; set; } = null!;
        public virtual DbSet<RmCambiosOficina> RmCambiosOficinas { get; set; } = null!;
        public virtual DbSet<RmCaracSimilare> RmCaracSimilares { get; set; } = null!;
        public virtual DbSet<RmCertificacione> RmCertificaciones { get; set; } = null!;
        public virtual DbSet<RmCertificacionesDet> RmCertificacionesDets { get; set; } = null!;
        public virtual DbSet<RmConceptosLiquidacione> RmConceptosLiquidaciones { get; set; } = null!;
        public virtual DbSet<RmDistrito> RmDistritos { get; set; } = null!;
        public virtual DbSet<RmEntradaBoletum> RmEntradaBoleta { get; set; } = null!;
        public virtual DbSet<RmEstadosEntradum> RmEstadosEntrada { get; set; } = null!;
        public virtual DbSet<RmFormasConcurrencium> RmFormasConcurrencia { get; set; } = null!;
        public virtual DbSet<RmFormasGruposMenu> RmFormasGruposMenus { get; set; } = null!;
        public virtual DbSet<RmFormasMenu> RmFormasMenus { get; set; } = null!;
        public virtual DbSet<RmFormasTableCab> RmFormasTableCabs { get; set; } = null!;
        public virtual DbSet<RmFormasTableDet> RmFormasTableDets { get; set; } = null!;
        public virtual DbSet<RmGruposMenu> RmGruposMenus { get; set; } = null!;
        public virtual DbSet<RmImagen2> RmImagen2s { get; set; } = null!;
        public virtual DbSet<RmImagenTemp> RmImagenTemps { get; set; } = null!;
        public virtual DbSet<RmImagenesSenale> RmImagenesSenales { get; set; } = null!;
        public virtual DbSet<RmImgSenalArea> RmImgSenalAreas { get; set; } = null!;
        public virtual DbSet<RmInforme> RmInformes { get; set; } = null!;
        public virtual DbSet<RmInformeDet> RmInformeDets { get; set; } = null!;
        public virtual DbSet<RmInterviniente> RmIntervinientes { get; set; } = null!;
        public virtual DbSet<RmLevantamiento> RmLevantamientos { get; set; } = null!;
        public virtual DbSet<RmLiquidacionesMarca> RmLiquidacionesMarcas { get; set; } = null!;
        public virtual DbSet<RmMarcaSenalDistrito> RmMarcaSenalDistritos { get; set; } = null!;
        public virtual DbSet<RmMarcasSenale> RmMarcasSenales { get; set; } = null!;
        public virtual DbSet<RmMarcasXEstab> RmMarcasXEstabs { get; set; } = null!;
        public virtual DbSet<RmMedidasPrenda> RmMedidasPrendas { get; set; } = null!;
        public virtual DbSet<RmMesaEntradaDup> RmMesaEntradaDups { get; set; } = null!;
        public virtual DbSet<RmMesaEntradum> RmMesaEntrada { get; set; } = null!;
        public virtual DbSet<RmMotivosAnulacion> RmMotivosAnulacions { get; set; } = null!;
        public virtual DbSet<RmMovimientosDoc> RmMovimientosDocs { get; set; } = null!;
        public virtual DbSet<RmNotasNegativa> RmNotasNegativas { get; set; } = null!;
        public virtual DbSet<RmOficinasRegistrale> RmOficinasRegistrales { get; set; } = null!;
        public virtual DbSet<RmOperacionesSist> RmOperacionesSists { get; set; } = null!;
        public virtual DbSet<RmParametrosNotificado> RmParametrosNotificados { get; set; } = null!;
        public virtual DbSet<RmReingreso> RmReingresos { get; set; } = null!;
        public virtual DbSet<RmSemejanteMarca> RmSemejanteMarcas { get; set; } = null!;
        public virtual DbSet<RmSemejanteSenal> RmSemejanteSenals { get; set; } = null!;
        public virtual DbSet<RmTipoSolicitud> RmTipoSolicituds { get; set; } = null!;
        public virtual DbSet<RmTiposDocumento> RmTiposDocumentos { get; set; } = null!;
        public virtual DbSet<RmTiposLiquidacion> RmTiposLiquidacions { get; set; } = null!;
        public virtual DbSet<RmTiposMoneda> RmTiposMonedas { get; set; } = null!;
        public virtual DbSet<RmTiposOperacione> RmTiposOperaciones { get; set; } = null!;
        public virtual DbSet<RmTiposPropiedad> RmTiposPropiedads { get; set; } = null!;
        public virtual DbSet<RmTiposUsuario> RmTiposUsuarios { get; set; } = null!;
        public virtual DbSet<RmTitularesMarca> RmTitularesMarcas { get; set; } = null!;
        public virtual DbSet<RmTransaccione> RmTransacciones { get; set; } = null!;
        public virtual DbSet<RmUsuario> RmUsuarios { get; set; } = null!;
        public virtual DbSet<Sectore> Sectores { get; set; } = null!;
        public virtual DbSet<SectoresEcon> SectoresEcons { get; set; } = null!;
        public virtual DbSet<SegurosPersona> SegurosPersonas { get; set; } = null!;
        public virtual DbSet<SeriesComprob> SeriesComprobs { get; set; } = null!;
        public virtual DbSet<SocioClube> SocioClubes { get; set; } = null!;
        public virtual DbSet<SrsImg> SrsImgs { get; set; } = null!;
        public virtual DbSet<SrsImgDatum> SrsImgData { get; set; } = null!;
        public virtual DbSet<SrsTmpImg> SrsTmpImgs { get; set; } = null!;
        public virtual DbSet<SrsTmpImgDatum> SrsTmpImgData { get; set; } = null!;
        public virtual DbSet<SrsTmpRaw> SrsTmpRaws { get; set; } = null!;
        public virtual DbSet<SrsTmpRe> SrsTmpRes { get; set; } = null!;
        public virtual DbSet<StListaPrecio> StListaPrecios { get; set; } = null!;
        public virtual DbSet<SubtiposTran> SubtiposTrans { get; set; } = null!;
        public virtual DbSet<Sucursale> Sucursales { get; set; } = null!;
        public virtual DbSet<TablasNoProcTransf> TablasNoProcTransfs { get; set; } = null!;
        public virtual DbSet<Talonario> Talonarios { get; set; } = null!;
        public virtual DbSet<TarjetasCredito> TarjetasCreditos { get; set; } = null!;
        public virtual DbSet<TarjetasDebito> TarjetasDebitos { get; set; } = null!;
        public virtual DbSet<TelefPersona> TelefPersonas { get; set; } = null!;
        public virtual DbSet<TiposCambio> TiposCambios { get; set; } = null!;
        public virtual DbSet<TiposCambioMensual> TiposCambioMensuals { get; set; } = null!;
        public virtual DbSet<TiposSociedad> TiposSociedads { get; set; } = null!;
        public virtual DbSet<TiposTalonario> TiposTalonarios { get; set; } = null!;
        public virtual DbSet<TiposTran> TiposTrans { get; set; } = null!;
        public virtual DbSet<TmpSubtiposTran> TmpSubtiposTrans { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;
        public virtual DbSet<UsuariosBkp> UsuariosBkps { get; set; } = null!;
        public virtual DbSet<UsuariosCustodio> UsuariosCustodios { get; set; } = null!;
        public virtual DbSet<UsuariosGruposEmail> UsuariosGruposEmails { get; set; } = null!;
        public virtual DbSet<UsuariosTran> UsuariosTrans { get; set; } = null!;
        public virtual DbSet<Vestab> Vestabs { get; set; } = null!;
        public virtual DbSet<ZonasGeografica> ZonasGeograficas { get; set; } = null!;
        public virtual DbSet<ZonasUbicacion> ZonasUbicacions { get; set; } = null!;
        public virtual DbSet<Carrera> Carreras { get; set; } = null!;
        public virtual DbSet<CarreraUsuario> CarreraUsuarios { get; set; } = null!;
        public virtual DbSet<Centroestudio> Centroestudios { get; set; } = null!;
        public virtual DbSet<Comentario> Comentarios { get; set; } = null!;
        public virtual DbSet<Datosacademico> Datosacademicos { get; set; } = null!;
        public virtual DbSet<Datoslaborale> Datoslaborales { get; set; } = null!;
        public virtual DbSet<Forodebate> Forodebates { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder
                       // .UseSqlServer("Server=172.30.150.13,1433; Database=dbinv;Persist Security Info=False;User ID=sa;Password=sA2023;Encrypt=False;Trusted_Connection=False; MultipleActiveResultSets=true;Connect Timeout=180");

                       .UseSqlServer(@"Data Source=DESKTOP-KP48E0B\SQLEXPRESS; Integrated Security=SSPI; Initial Catalog=UAAconecta;Connect Timeout=180") // Timeout de 30 segundos
                        .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
            }
        }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {




            modelBuilder.Entity<AccesosGrupo>(entity =>
            {
                entity.HasKey(e => new { e.CodGrupo, e.CodModulo, e.NomForma });
                entity.ToTable("ACCESOS_GRUPOS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodGrupo)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("COD_GRUPO");
                entity.Property(e => e.CodModulo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("COD_MODULO");
                entity.Property(e => e.NomForma)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("NOM_FORMA");
                entity.Property(e => e.ItemMenu)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ITEM_MENU");
                entity.Property(e => e.PuedeActualizar)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("PUEDE_ACTUALIZAR");
                entity.Property(e => e.PuedeBorrar)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("PUEDE_BORRAR");
                entity.Property(e => e.PuedeConsultar)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("PUEDE_CONSULTAR");
                entity.Property(e => e.PuedeInsertar)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("PUEDE_INSERTAR");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("newid()");
                entity.HasOne(d => d.CodGrupoNavigation)
                    .WithMany(p => p.AccesosGrupos)
                    .HasForeignKey(d => d.CodGrupo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ACC_GRUPOS_GRP");
                entity.HasOne(d => d.CodModuloNavigation)
                    .WithMany(p => p.AccesosGrupos)
                    .HasForeignKey(d => d.CodModulo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ACC_GRU_MOD");
            });
            modelBuilder.Entity<AccesosRrhh>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("ACCESOS_RRHH", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodGrupo)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("COD_GRUPO");
                entity.Property(e => e.CodModulo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("COD_MODULO");
                entity.Property(e => e.ItemMenu)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ITEM_MENU");
                entity.Property(e => e.NomForma)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("NOM_FORMA");
                entity.Property(e => e.PuedeActualizar)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("PUEDE_ACTUALIZAR");
                entity.Property(e => e.PuedeBorrar)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("PUEDE_BORRAR");
                entity.Property(e => e.PuedeConsultar)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("PUEDE_CONSULTAR");
                entity.Property(e => e.PuedeInsertar)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("PUEDE_INSERTAR");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<ActivPersona>(entity =>
            {
                entity.HasKey(e => new { e.CodActividad, e.CodPersona });
                entity.ToTable("ACTIV_PERSONAS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodActividad)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_ACTIVIDAD");
                entity.Property(e => e.CodPersona)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_PERSONA");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
                entity.HasOne(d => d.CodActividadNavigation)
                    .WithMany(p => p.ActivPersonas)
                    .HasForeignKey(d => d.CodActividad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ACT_PERSONA_ACT");
            });
            modelBuilder.Entity<ActividadesEcon>(entity =>
            {
                entity.HasKey(e => e.CodActividad);
                entity.ToTable("ACTIVIDADES_ECON", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodActividad)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_ACTIVIDAD");
                entity.Property(e => e.Abreviatura)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("ABREVIATURA");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<AgendaDiarium>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("AGENDA_DIARIA", "INV");
                entity.Property(e => e.CodGrupo)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_GRUPO");
                entity.Property(e => e.CodUsuario)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO");
                entity.Property(e => e.CodUsuarioAlta)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO_ALTA");
                entity.Property(e => e.Detalle)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("DETALLE");
                entity.Property(e => e.FecActividad)
                    .HasPrecision(0)
                    .HasColumnName("FEC_ACTIVIDAD");
                entity.Property(e => e.FecAlta)
                    .HasPrecision(0)
                    .HasColumnName("FEC_ALTA");
                entity.Property(e => e.FecUltimaActualizacion)
                    .HasPrecision(0)
                    .HasColumnName("FEC_ULTIMA_ACTUALIZACION");
                entity.Property(e => e.HoraActividad)
                    .HasPrecision(0)
                    .HasColumnName("HORA_ACTIVIDAD");
                entity.Property(e => e.IndEnviaMail)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IND_ENVIA_MAIL");
                entity.Property(e => e.IndRealizado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IND_REALIZADO");
                entity.Property(e => e.NroActividad)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("NRO_ACTIVIDAD");
                entity.Property(e => e.Subject)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SUBJECT");
            });
            modelBuilder.Entity<Area>(entity =>
            {
                entity.HasKey(e => new { e.CodEmpresa, e.CodArea });
                entity.ToTable("AREAS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodEmpresa)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_EMPRESA");
                entity.Property(e => e.CodArea)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_AREA");
                entity.Property(e => e.Activo)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ACTIVO");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(65)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<AreasUsuario>(entity =>
            {
                entity.HasKey(e => new { e.CodEmpresa, e.CodArea, e.CodUsuario });
                entity.ToTable("AREAS_USUARIOS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodEmpresa)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_EMPRESA");
                entity.Property(e => e.CodArea)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_AREA");
                entity.Property(e => e.CodUsuario)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO");
                entity.Property(e => e.Activo)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ACTIVO");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<AuditoriaRep>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("AUDITORIA_REP", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodEmpresa)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_EMPRESA");
                entity.Property(e => e.CodUsuario)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO");
                entity.Property(e => e.Comentario)
                    .HasMaxLength(1500)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");
                entity.Property(e => e.Fecha)
                    .HasPrecision(0)
                    .HasColumnName("FECHA");
                entity.Property(e => e.Maquina)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MAQUINA");
                entity.Property(e => e.NomReporte)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("NOM_REPORTE");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<Auditorium>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("AUDITORIA", "INV");
                entity.HasIndex(e => e.NroOperacion, "IND_NRO_OPERACION");
                entity.Property(e => e.CodEmpresa)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_EMPRESA");
                entity.Property(e => e.CodModulo)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("COD_MODULO");
                entity.Property(e => e.CodUsuario)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO");
                entity.Property(e => e.CodUsuarioTransf)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO_TRANSF");
                entity.Property(e => e.Columnas)
                    .HasMaxLength(3000)
                    .IsUnicode(false)
                    .HasColumnName("COLUMNAS");
                entity.Property(e => e.DatosNuevos)
                    .HasMaxLength(4000)
                    .IsUnicode(false)
                    .HasColumnName("DATOS_NUEVOS");
                entity.Property(e => e.DatosViejos)
                    .HasMaxLength(4000)
                    .IsUnicode(false)
                    .HasColumnName("DATOS_VIEJOS");
                entity.Property(e => e.FecOcurrencia)
                    .HasPrecision(0)
                    .HasColumnName("FEC_OCURRENCIA");
                entity.Property(e => e.FecTransferido)
                    .HasPrecision(0)
                    .HasColumnName("FEC_TRANSFERIDO");
                entity.Property(e => e.NomTabla)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("NOM_TABLA");
                entity.Property(e => e.NroOperacion)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("NRO_OPERACION");
                entity.Property(e => e.Operacion)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("OPERACION");
                entity.Property(e => e.Transferido)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("TRANSFERIDO");
            });
            modelBuilder.Entity<AutorizadosParaPedido>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("AUTORIZADOS_PARA_PEDIDOS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CedulaIdentidad)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("CEDULA_IDENTIDAD");
                entity.Property(e => e.CodPersona)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_PERSONA");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<AvDepartamento>(entity =>
            {
                entity.HasKey(e => e.CodDepartamento);
                entity.ToTable("AV_DEPARTAMENTOS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodDepartamento)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("COD_DEPARTAMENTO");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<AvEstablecimiento>(entity =>
            {
                entity.HasKey(e => new { e.CodEstable, e.CodEstablePj });
                entity.ToTable("AV_ESTABLECIMIENTOS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodEstable)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("COD_ESTABLE");
                entity.Property(e => e.CodEstablePj)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("COD_ESTABLE_PJ");
                entity.Property(e => e.Banio)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("BANIO");
                entity.Property(e => e.Brete)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("BRETE");
                entity.Property(e => e.Cepo)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("CEPO");
                entity.Property(e => e.CodDepartamento)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("COD_DEPARTAMENTO");
                entity.Property(e => e.CodDistrito)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("COD_DISTRITO");
                entity.Property(e => e.CodEstableOld)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("COD_ESTABLE_OLD");
                entity.Property(e => e.CodFinalidad)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_FINALIDAD");
                entity.Property(e => e.CodLocalidad)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_LOCALIDAD");
                entity.Property(e => e.CodPropietario)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("COD_PROPIETARIO");
                entity.Property(e => e.CodRegional)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("COD_REGIONAL");
                entity.Property(e => e.CodTipoEstab)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_TIPO_ESTAB");
                entity.Property(e => e.CodUsuarioAlta)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO_ALTA");
                entity.Property(e => e.CodZona)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("COD_ZONA");
                entity.Property(e => e.Corral)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("CORRAL");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.Domicilio)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DOMICILIO");
                entity.Property(e => e.Embarcadero)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("EMBARCADERO");
                entity.Property(e => e.FecUltMod)
                    .HasPrecision(0)
                    .HasColumnName("FEC_ULT_MOD");
                entity.Property(e => e.FechaAlta)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_ALTA");
                entity.Property(e => e.FrecuenciaRadio)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("FRECUENCIA_RADIO");
                entity.Property(e => e.Galpon)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("GALPON");
                entity.Property(e => e.GpsH)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("GPS_H");
                entity.Property(e => e.GpsS)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("GPS_S");
                entity.Property(e => e.GpsSc)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("GPS_SC");
                entity.Property(e => e.GpsV)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("GPS_V");
                entity.Property(e => e.GpsW)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("GPS_W");
                entity.Property(e => e.LinderoEste)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("LINDERO_ESTE");
                entity.Property(e => e.LinderoNorte)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("LINDERO_NORTE");
                entity.Property(e => e.LinderoOeste)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("LINDERO_OESTE");
                entity.Property(e => e.LinderoSur)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("LINDERO_SUR");
                entity.Property(e => e.NroPotero)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("NRO_POTERO");
                entity.Property(e => e.PasturaCultivada)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("PASTURA_CULTIVADA");
                entity.Property(e => e.PasturaMonte)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("PASTURA_MONTE");
                entity.Property(e => e.PasturaNatural)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("PASTURA_NATURAL");
                entity.Property(e => e.PasturaOtro)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("PASTURA_OTRO");
                entity.Property(e => e.PistaAterrisaje)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("PISTA_ATERRISAJE");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("newid()");
                entity.Property(e => e.Telefono)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TELEFONO");
                entity.Property(e => e.TotalHectarea)
                    .HasColumnType("numeric(12, 0)")
                    .HasColumnName("TOTAL_HECTAREA");
                entity.Property(e => e.UsuarioUltMod)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("USUARIO_ULT_MOD");
            });
            modelBuilder.Entity<AvEstpropie>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("AV_ESTPROPIE", "INV");
                entity.HasIndex(e => e.CodEstable, "COD_ESTABLE");
                entity.HasIndex(e => e.CodPropietario, "COD_PROPIETARIO");
                entity.HasIndex(e => new { e.CodEstable, e.CodPropietario }, "PK_AV_ESTPROPIE")
                    .IsUnique();
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.BloqConsumo)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("BLOQ_CONSUMO");
                entity.Property(e => e.BloqCria)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("BLOQ_CRIA");
                entity.Property(e => e.BloqEngorde)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("BLOQ_ENGORDE");
                entity.Property(e => e.BloqExposicion)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("BLOQ_EXPOSICION");
                entity.Property(e => e.BloqFaena)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("BLOQ_FAENA");
                entity.Property(e => e.BloqInvernada)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("BLOQ_INVERNADA");
                entity.Property(e => e.BloqReproduccion)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("BLOQ_REPRODUCCION");
                entity.Property(e => e.CodEstable)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_ESTABLE");
                entity.Property(e => e.CodPropietario)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("COD_PROPIETARIO");
                entity.Property(e => e.CodPropietarioViejo)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("COD_PROPIETARIO_VIEJO");
                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO");
                entity.Property(e => e.FecVa1era)
                    .HasPrecision(0)
                    .HasColumnName("FEC_VA1ERA");
                entity.Property(e => e.FecVaultima)
                    .HasPrecision(0)
                    .HasColumnName("FEC_VAULTIMA");
                entity.Property(e => e.HabilitadoCota)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("HABILITADO_COTA");
                entity.Property(e => e.ProMayor)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("PRO_MAYOR");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<AvFinalidade>(entity =>
            {
                entity.HasKey(e => e.CodFinalidad)
                    .HasName("PK_AV_FINALIDAD");
                entity.ToTable("AV_FINALIDADES", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodFinalidad)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_FINALIDAD");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
                entity.Property(e => e.UsoCota)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("USO_COTA");
                entity.Property(e => e.UsoEstablecimiento)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("USO_ESTABLECIMIENTO");
            });
            modelBuilder.Entity<AvLocalidade>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("AV_LOCALIDADES", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodDistrito)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_DISTRITO");
                entity.Property(e => e.CodLocalidad)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_LOCALIDAD");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
                entity.Property(e => e.Rownumb)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ROWNUMB");
                entity.Property(e => e.UltimoEstablecimiento)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("ULTIMO_ESTABLECIMIENTO");
            });
            modelBuilder.Entity<AvRegionale>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("AV_REGIONALES", "INV");
                entity.HasIndex(e => e.CodRegional, "PK_AV_REGIONALES")
                    .IsUnique()
                    .HasFilter("([COD_REGIONAL] IS NOT NULL)");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodDepartamento)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_DEPARTAMENTO");
                entity.Property(e => e.CodPais)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_PAIS");
                entity.Property(e => e.CodRegional)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_REGIONAL");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<AvTipoEstablecimiento>(entity =>
            {
                entity.HasKey(e => e.CodTipoEstab);
                entity.ToTable("AV_TIPO_ESTABLECIMIENTOS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodTipoEstab)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_TIPO_ESTAB");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<AvZona>(entity =>
            {
                entity.HasKey(e => new { e.CodPais, e.CodDepartamento, e.CodZona });
                entity.ToTable("AV_ZONAS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodPais)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_PAIS");
                entity.Property(e => e.CodDepartamento)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("COD_DEPARTAMENTO");
                entity.Property(e => e.CodZona)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("COD_ZONA");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<Barrio>(entity =>
            {
                entity.HasKey(e => new { e.CodPais, e.CodProvincia, e.CodCiudad, e.CodBarrio });
                entity.ToTable("BARRIOS", "INV");
                entity.Property(e => e.CodPais)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_PAIS");
                entity.Property(e => e.CodProvincia)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_PROVINCIA");
                entity.Property(e => e.CodCiudad)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_CIUDAD");
                entity.Property(e => e.CodBarrio)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_BARRIO");
                entity.Property(e => e.Abreviatura)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ABREVIATURA");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
            });
            modelBuilder.Entity<Calendario>(entity =>
            {
                entity.HasKey(e => new { e.CodEmpresa, e.CodModulo });
                entity.ToTable("CALENDARIOS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodEmpresa)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_EMPRESA");
                entity.Property(e => e.CodModulo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("COD_MODULO");
                entity.Property(e => e.ActualizadoPor)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ACTUALIZADO_POR");
                entity.Property(e => e.AltaPor)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ALTA_POR");
                entity.Property(e => e.FecActual)
                    .HasPrecision(0)
                    .HasColumnName("FEC_ACTUAL");
                entity.Property(e => e.FecActualizado)
                    .HasPrecision(0)
                    .HasColumnName("FEC_ACTUALIZADO");
                entity.Property(e => e.FecAlta)
                    .HasPrecision(0)
                    .HasColumnName("FEC_ALTA");
                entity.Property(e => e.FecFinal)
                    .HasPrecision(0)
                    .HasColumnName("FEC_FINAL");
                entity.Property(e => e.FecInicial)
                    .HasPrecision(0)
                    .HasColumnName("FEC_INICIAL");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("newid()");
                entity.Property(e => e.UsaCalendario)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("USA_CALENDARIO");
                entity.HasOne(d => d.CodEmpresaNavigation)
                    .WithMany(p => p.Calendarios)
                    .HasForeignKey(d => d.CodEmpresa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CALEND_EMP");
               entity.HasOne(d => d.CodModuloNavigation)
                    .WithMany(p => p.Calendarios)
                    .HasForeignKey(d => d.CodModulo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CALEND_MOD");
            });
            modelBuilder.Entity<Ciudade>(entity =>
            {
                entity.HasKey(e => e.CodCiudad);
                entity.ToTable("CIUDADES", "INV");
                entity.HasIndex(e => e.CodCiudad, "COD_CIUDAD");
                entity.HasIndex(e => new { e.CodCiudad, e.CodProvincia, e.CodPais, e.CodZona }, "INDX_CIUDADES_COD_CIUDAD");
                entity.Property(e => e.Abreviatura)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ABREVIATURA");
                entity.Property(e => e.CodCiudad)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_CIUDAD");
                entity.Property(e => e.CodPais)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_PAIS");
                entity.Property(e => e.CodProvincia)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_PROVINCIA");
                entity.Property(e => e.CodRegional)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_REGIONAL");
                entity.Property(e => e.CodZona)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_ZONA");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
            });
            modelBuilder.Entity<CmProvee>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("CM_PROVEE", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodEmpresa)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_EMPRESA");
                entity.Property(e => e.CodProveedor)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_PROVEEDOR");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<Departamento>(entity =>
            {
                entity.HasKey(e => new { e.CodEmpresa, e.CodDepartamento });
                entity.ToTable("DEPARTAMENTOS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodEmpresa)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_EMPRESA");
                entity.Property(e => e.CodDepartamento)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_DEPARTAMENTO");
                entity.Property(e => e.Activo)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ACTIVO");
                entity.Property(e => e.CodDepto).HasColumnName("COD_DEPTO");
                entity.Property(e => e.CodSucursal)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_SUCURSAL");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(65)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<Deptos1>(entity =>
            {
                entity.HasKey(e => e.IdDepto1)
                    .HasName("CONSTR$DEPTOS1");
                entity.ToTable("DEPTOS1", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.IdDepto1).HasColumnName("ID_DEPTO1");
                entity.Property(e => e.DescDepto1)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("DESC_DEPTO1");
                entity.Property(e => e.IdOficina).HasColumnName("ID_OFICINA");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
                entity.HasOne(d => d.IdOficinaNavigation)
                    .WithMany(p => p.Deptos1s)
                    .HasForeignKey(d => d.IdOficina)
                    .HasConstraintName("ID_OFICINA");
            });
            modelBuilder.Entity<DirecPersona>(entity =>
            {
                entity.HasKey(e => new { e.CodPersona, e.CodDireccion });
                entity.ToTable("DIREC_PERSONAS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodPersona)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_PERSONA");
                entity.Property(e => e.CodDireccion)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("COD_DIRECCION");
                entity.Property(e => e.Barrio)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("BARRIO");
                entity.Property(e => e.CasillaCorreo)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("CASILLA_CORREO");
                entity.Property(e => e.Ciudad)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CIUDAD");
                entity.Property(e => e.CodBarrio)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_BARRIO");
                entity.Property(e => e.CodCiudad)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_CIUDAD");
                entity.Property(e => e.CodPais)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_PAIS");
                entity.Property(e => e.CodProvincia)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_PROVINCIA");
                entity.Property(e => e.CodigoPostal)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CODIGO_POSTAL");
                entity.Property(e => e.DescripcionRef)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION_REF");
                entity.Property(e => e.Detalle)
                    .HasMaxLength(160)
                    .IsUnicode(false)
                    .HasColumnName("DETALLE");
                entity.Property(e => e.PorDefecto)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("POR_DEFECTO");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
                entity.Property(e => e.TipDireccion)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("TIP_DIRECCION");
             
            });
            modelBuilder.Entity<Dpto>(entity =>
            {
                entity.HasKey(e => e.CodDpto)
                    .HasName("COD_DPTO");
                entity.ToTable("DPTOS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodDpto).HasColumnName("COD_DPTO");
                entity.Property(e => e.CodPais)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_PAIS");
                entity.Property(e => e.NombreDpto)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_DPTO");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<Empresa>(entity =>
            {
                entity.HasKey(e => e.CodEmpresa);
                entity.ToTable("EMPRESAS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodEmpresa)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_EMPRESA");
                entity.Property(e => e.Actividad)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ACTIVIDAD");
                entity.Property(e => e.CodMonedaOrigen)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("COD_MONEDA_ORIGEN");
                entity.Property(e => e.CodPerJuridica)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_PER_JURIDICA");
                entity.Property(e => e.Departamento)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("DEPARTAMENTO");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.Direccion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DIRECCION");
                entity.Property(e => e.Localidad)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("LOCALIDAD");
                entity.Property(e => e.NroPatronal)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NRO_PATRONAL");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
                entity.Property(e => e.RucEmpresa)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("RUC_EMPRESA");
                entity.Property(e => e.TituloReportes)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("TITULO_REPORTES");
                entity.Property(e => e.UsaCalendario)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("USA_CALENDARIO");
            });
            modelBuilder.Entity<EstadosCivile>(entity =>
            {
                entity.HasKey(e => e.CodEstadoCivil);
                entity.ToTable("ESTADOS_CIVILES", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodEstadoCivil)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("COD_ESTADO_CIVIL");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("newid()");
            });
            modelBuilder.Entity<Feriado>(entity =>
            {
                entity.HasKey(e => e.Feriado1);
                entity.ToTable("FERIADOS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.Feriado1)
                    .HasPrecision(0)
                    .HasColumnName("FERIADO");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<Forma>(entity =>
            {
                entity.HasKey(e => new { e.CodModulo, e.NomForma });
                entity.ToTable("FORMAS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodModulo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("COD_MODULO");
                entity.Property(e => e.NomForma)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("NOM_FORMA");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("newid()");
                entity.Property(e => e.Titulo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TITULO");
                entity.HasOne(d => d.CodModuloNavigation)
                    .WithMany(p => p.Formas)
                    .HasForeignKey(d => d.CodModulo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FORMAS_MOD");
            });
            modelBuilder.Entity<Funcionario>(entity =>
            {
                entity.HasKey(e => new { e.CodEmpresa, e.CodFuncionario });
                entity.ToTable("FUNCIONARIOS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodEmpresa)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_EMPRESA");
                entity.Property(e => e.CodFuncionario)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_FUNCIONARIO");
                entity.Property(e => e.Activo)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ACTIVO");
                entity.Property(e => e.CodDepartamento)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_DEPARTAMENTO");
                entity.Property(e => e.CodPersona)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_PERSONA");
                entity.Property(e => e.CodSucursal)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_SUCURSAL");
                entity.Property(e => e.RecibeDocumentos)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("RECIBE_DOCUMENTOS");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<GruposEmail>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("GRUPOS_EMAIL", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodGrupo)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_GRUPO");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<GruposUsuario>(entity =>
            {
                entity.HasKey(e => e.CodGrupo)
                    .HasName("PK_GRUPOS_USUAROS");
                entity.ToTable("GRUPOS_USUARIOS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodGrupo)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("COD_GRUPO");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.OpFueraHo)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("OP_FUERA_HO");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("newid()");
            });
            modelBuilder.Entity<Hijo>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("HIJOS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodPersona)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_PERSONA");
                entity.Property(e => e.ColegioUniversidad)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("COLEGIO_UNIVERSIDAD");
                entity.Property(e => e.FecNacimiento)
                    .HasPrecision(0)
                    .HasColumnName("FEC_NACIMIENTO");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<HistComprobante>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("HIST_COMPROBANTES", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodAreaEnt)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_AREA_ENT");
                entity.Property(e => e.CodAreaEnv)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_AREA_ENV");
                entity.Property(e => e.CodAreaSal)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_AREA_SAL");
                entity.Property(e => e.CodEmpresa)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_EMPRESA");
                entity.Property(e => e.CodSucursalSal)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_SUCURSAL_SAL");
                entity.Property(e => e.CodUsuarioEnt)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO_ENT");
                entity.Property(e => e.CodUsuarioSal)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO_SAL");
                entity.Property(e => e.Copia)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("COPIA");
                entity.Property(e => e.Enviado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ENVIADO");
                entity.Property(e => e.FecEnviado)
                    .HasPrecision(0)
                    .HasColumnName("FEC_ENVIADO");
                entity.Property(e => e.FecEnvio)
                    .HasPrecision(0)
                    .HasColumnName("FEC_ENVIO");
                entity.Property(e => e.FecRecibido)
                    .HasPrecision(0)
                    .HasColumnName("FEC_RECIBIDO");
                entity.Property(e => e.NroComprobante)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("NRO_COMPROBANTE");
                entity.Property(e => e.Observacion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("OBSERVACION");
                entity.Property(e => e.Paso)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("PASO");
                entity.Property(e => e.Recibido)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("RECIBIDO");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
                entity.Property(e => e.SerComprobante)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("SER_COMPROBANTE");
                entity.Property(e => e.TipComprobante)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIP_COMPROBANTE");
            });
            modelBuilder.Entity<HistDocumento>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("HIST_DOCUMENTOS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.Borrado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("BORRADO");
                entity.Property(e => e.BorroUsuario)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("BORRO_USUARIO");
                entity.Property(e => e.CodDepartamentoEnt)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_DEPARTAMENTO_ENT");
                entity.Property(e => e.CodDepartamentoSal)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_DEPARTAMENTO_SAL");
                entity.Property(e => e.CodEmpresa)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_EMPRESA");
                entity.Property(e => e.CodFuncionarioEnt)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("COD_FUNCIONARIO_ENT");
                entity.Property(e => e.CodFuncionarioSal)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("COD_FUNCIONARIO_SAL");
                entity.Property(e => e.CodSucursal)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_SUCURSAL");
                entity.Property(e => e.CodUsuario)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO");
                entity.Property(e => e.EstadoAct)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO_ACT");
                entity.Property(e => e.EstadoAnt)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO_ANT");
                entity.Property(e => e.FecBorrado)
                    .HasPrecision(0)
                    .HasColumnName("FEC_BORRADO");
                entity.Property(e => e.FecCarga)
                    .HasPrecision(0)
                    .HasColumnName("FEC_CARGA");
                entity.Property(e => e.FecDocumento)
                    .HasPrecision(0)
                    .HasColumnName("FEC_DOCUMENTO");
                entity.Property(e => e.NroDocumento)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("NRO_DOCUMENTO");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
                entity.Property(e => e.SerDocumento)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("SER_DOCUMENTO");
                entity.Property(e => e.TipDocumento)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIP_DOCUMENTO");
            });
            modelBuilder.Entity<IdentPersona>(entity =>
            {
                entity.HasKey(e => new { e.CodPersona, e.CodIdent });
                entity.ToTable("IDENT_PERSONAS", "INV");
                entity.HasIndex(e => e.CodPersona, "IND_IDENT_PERSONA");
                entity.HasIndex(e => e.Numero, "IND_NUMERO_DOC");
                entity.HasIndex(e => new { e.CodIdent, e.Numero }, "IND_TIP_NUMERO_DOC");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodPersona)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_PERSONA");
                entity.Property(e => e.CodIdent)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_IDENT");
                entity.Property(e => e.FecVencimiento)
                    .HasPrecision(0)
                    .HasColumnName("FEC_VENCIMIENTO");
                entity.Property(e => e.Numero)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("NUMERO");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
                entity.HasOne(d => d.CodIdentNavigation)
                    .WithMany(p => p.IdentPersonas)
                    .HasForeignKey(d => d.CodIdent)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IDEN_PERSONAS_IND");
            });
            modelBuilder.Entity<Identificacione>(entity =>
            {
                entity.HasKey(e => e.CodIdent);
                entity.ToTable("IDENTIFICACIONES", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodIdent)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_IDENT");
                entity.Property(e => e.Abreviatura)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ABREVIATURA");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.Mascara)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("MASCARA");
                entity.Property(e => e.PersonaFisica)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("PERSONA_FISICA");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("newid()");
            });
            modelBuilder.Entity<Inscripcion>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("INSCRIPCION", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.IdAlumno).HasColumnName("ID_ALUMNO");
                entity.Property(e => e.IdMateria).HasColumnName("ID_MATERIA");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
                entity.HasOne(d => d.IdMateriaNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdMateria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("COD_MATERIA");
            });
            modelBuilder.Entity<Lugar>(entity =>
            {
                entity.HasKey(e => e.IdLugar)
                    .HasName("CODIGO_LUGAR");
                entity.ToTable("LUGAR", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.IdLugar).HasColumnName("ID_LUGAR");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<MalasReferencia>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("MALAS_REFERENCIAS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodBanco)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_BANCO");
                entity.Property(e => e.CodMotivo)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("COD_MOTIVO");
                entity.Property(e => e.CodPersona)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_PERSONA");
                entity.Property(e => e.FecIngreso)
                    .HasPrecision(0)
                    .HasColumnName("FEC_INGRESO");
                entity.Property(e => e.FecVencimi)
                    .HasPrecision(0)
                    .HasColumnName("FEC_VENCIMI");
                entity.Property(e => e.Monto)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("MONTO");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<Materium>(entity =>
            {
                entity.HasKey(e => e.IdMateria)
                    .HasName("CODIGO_MAT");
                entity.ToTable("MATERIA", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.IdMateria).HasColumnName("ID_MATERIA");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<Memito>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("MEMITO", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.Memo1)
                    .HasMaxLength(254)
                    .IsUnicode(false)
                    .HasColumnName("MEMO1")
                    .IsFixedLength();
                entity.Property(e => e.Memo2)
                    .HasMaxLength(254)
                    .IsUnicode(false)
                    .HasColumnName("MEMO2")
                    .IsFixedLength();
                entity.Property(e => e.OtNumero)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("OT_NUMERO");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<Mensaje>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("MENSAJES", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.M)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<MesaEntradaViej>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("MESA_ENTRADA_VIEJ", "INV");
                entity.Property(e => e.CodigoOficina)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("CODIGO_OFICINA");
                entity.Property(e => e.EstadoEntrada)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ESTADO_ENTRADA");
                entity.Property(e => e.FechaEntrada)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_ENTRADA");
                entity.Property(e => e.FechaSalida)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_SALIDA");
                entity.Property(e => e.NombrePresentador)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_PRESENTADOR");
                entity.Property(e => e.NombreRetirador)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_RETIRADOR");
                entity.Property(e => e.NroDocumentoPresentador)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("NRO_DOCUMENTO_PRESENTADOR");
                entity.Property(e => e.NroDocumentoRetirador)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("NRO_DOCUMENTO_RETIRADOR");
                entity.Property(e => e.NroFormulario)
                    .HasColumnType("numeric(30, 0)")
                    .HasColumnName("NRO_FORMULARIO");
                entity.Property(e => e.NumeroEntrada)
                    .HasColumnType("numeric(20, 0)")
                    .HasColumnName("NUMERO_ENTRADA");
                entity.Property(e => e.NumeroLiquidacion)
                    .HasColumnType("numeric(30, 0)")
                    .HasColumnName("NUMERO_LIQUIDACION");
                entity.Property(e => e.TipoDocumentoPresentador)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("TIPO_DOCUMENTO_PRESENTADOR");
                entity.Property(e => e.TipoDocumentoRetirador)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("TIPO_DOCUMENTO_RETIRADOR");
                entity.Property(e => e.TipoSolicitud)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("TIPO_SOLICITUD");
                entity.Property(e => e.UsuarioEntrada)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("USUARIO_ENTRADA");
                entity.Property(e => e.UsuarioSalida)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("USUARIO_SALIDA");
            });
            modelBuilder.Entity<Mese>(entity =>
            {
                entity.HasKey(e => e.Mes);
                entity.ToTable("MESES", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.Mes)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("MES");
                entity.Property(e => e.Abreviatura)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("ABREVIATURA");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<Modulo>(entity =>
            {
                entity.HasKey(e => e.CodModulo)
                    .HasName("PKM_MODULOS");

                entity.ToTable("MODULOS", "INV");

                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();

                entity.Property(e => e.CodModulo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("COD_MODULO");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.ManejaCalendario)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("MANEJA_CALENDARIO");

                entity.Property(e => e.ManejaCierre)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("MANEJA_CIERRE");

                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("newid()");
            });
            modelBuilder.Entity<Moneda>(entity =>
            {
                entity.HasKey(e => e.CodMoneda);
                entity.ToTable("MONEDAS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodMoneda)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("COD_MONEDA");
                entity.Property(e => e.CodPais)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_PAIS");
                entity.Property(e => e.Decimales)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("DECIMALES");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.FecTipoCambio)
                    .HasPrecision(0)
                    .HasColumnName("FEC_TIPO_CAMBIO");
                entity.Property(e => e.PorcInteres)
                    .HasColumnType("numeric(18, 3)")
                    .HasColumnName("PORC_INTERES");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
                entity.Property(e => e.Siglas)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("SIGLAS");
                entity.Property(e => e.TipoCambio)
                    .HasColumnType("numeric(20, 6)")
                    .HasColumnName("TIPO_CAMBIO");
                entity.Property(e => e.TipoCambioCompra)
                    .HasColumnType("numeric(20, 6)")
                    .HasColumnName("TIPO_CAMBIO_COMPRA");
                entity.Property(e => e.TipoCambioContado)
                    .HasColumnType("numeric(12, 6)")
                    .HasColumnName("TIPO_CAMBIO_CONTADO");
                entity.Property(e => e.TipoCambioCredito)
                    .HasColumnType("numeric(12, 6)")
                    .HasColumnName("TIPO_CAMBIO_CREDITO");
                entity.Property(e => e.TipoCambioDia)
                    .HasColumnType("numeric(20, 6)")
                    .HasColumnName("TIPO_CAMBIO_DIA");
                entity.HasOne(d => d.CodPaisNavigation)
                    .WithMany(p => p.Moneda)
                    .HasForeignKey(d => d.CodPais)
                    .HasConstraintName("FK_MONEDA_PAI");
            });
            modelBuilder.Entity<MotivosMalRefer>(entity =>
            {
                entity.HasKey(e => e.CodMotivo);
                entity.ToTable("MOTIVOS_MAL_REFER", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodMotivo)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("COD_MOTIVO");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<MsssmaDmNckeyColumn>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.KeyName, e.KeyColumnId, e.KeyType })
                    .HasName("PK__MSSsmaDm__BC5021FB42EBD60F");
                entity.ToTable("MSSsmaDmNCKeyColumns", "o2ss");
                entity.Property(e => e.ObjectId).HasColumnName("object_id");
                entity.Property(e => e.KeyName)
                    .HasMaxLength(128)
                    .HasColumnName("key_name");
                entity.Property(e => e.KeyColumnId).HasColumnName("key_column_id");
                entity.Property(e => e.KeyType)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("key_type")
                    .IsFixedLength();
                entity.Property(e => e.DeleteReferentialAction).HasColumnName("delete_referential_action");
                entity.Property(e => e.IndexColumnId).HasColumnName("index_column_id");
                entity.Property(e => e.IsDisabled).HasColumnName("is_disabled");
                entity.Property(e => e.IsNotForReplication).HasColumnName("is_not_for_replication");
                entity.Property(e => e.IsNotTrusted).HasColumnName("is_not_trusted");
                entity.Property(e => e.ReferencedColumnId).HasColumnName("referenced_column_id");
                entity.Property(e => e.ReferencedObjectId).HasColumnName("referenced_object_id");
                entity.Property(e => e.UpdateReferentialAction).HasColumnName("update_referential_action");
            });
            modelBuilder.Entity<MsssmaDmTable>(entity =>
            {
                entity.HasKey(e => e.ObjectId)
                    .HasName("PK__MSSsmaDm__3DC088B5DFF1AD23");
                entity.ToTable("MSSsmaDmTables", "o2ss");
                entity.Property(e => e.ObjectId)
                    .ValueGeneratedNever()
                    .HasColumnName("object_id");
                entity.Property(e => e.DmStartTime)
                    .HasColumnType("datetime")
                    .HasColumnName("dm_start_time");
                entity.Property(e => e.SchemaId).HasColumnName("schema_id");
                entity.Property(e => e.Status).HasColumnName("status");
            });
            modelBuilder.Entity<Nextbook>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("NEXTBOOK", "INV");
                entity.Property(e => e.Text)
                    .IsUnicode(false)
                    .HasColumnName("TEXT");
                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TITLE");
            });
            modelBuilder.Entity<NivelEstudio>(entity =>
            {
                entity.HasKey(e => e.CodNivel);
                entity.ToTable("NIVEL_ESTUDIOS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodNivel)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_NIVEL");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<NumeroLetra>(entity =>
            {
                entity.HasKey(e => e.Numero)
                    .HasName("PKM_NUMERO_LETRAS");
                entity.ToTable("NUMERO_LETRAS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.Numero)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("NUMERO");
                entity.Property(e => e.Letras)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LETRAS");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<OeClCoejecutor>(entity =>
            {
                entity.HasKey(e => e.CoeCodigo)
                    .HasName("PK_COE_CL_COEJECUTOR");
                entity.ToTable("OE_CL_COEJECUTOR", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CoeCodigo)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("COE_CODIGO");
                entity.Property(e => e.CoeDescripcion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("COE_DESCRIPCION");
                entity.Property(e => e.PreCodigo)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("PRE_CODIGO");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
                entity.HasOne(d => d.PreCodigoNavigation)
                    .WithMany(p => p.OeClCoejecutors)
                    .HasForeignKey(d => d.PreCodigo)
                    .HasConstraintName("FK");
            });
            modelBuilder.Entity<Oficina>(entity =>
            {
                entity.HasKey(e => e.IdOfi)
                    .HasName("ID_OFI");
                entity.ToTable("OFICINAS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.IdOfi).HasColumnName("ID_OFI");
                entity.Property(e => e.DescOfi)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("DESC_OFI");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<OpcionesParametro>(entity =>
            {
                entity.HasKey(e => new { e.Tipo, e.Parametro, e.NomForma });
                entity.ToTable("OPCIONES_PARAMETROS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.Tipo)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("TIPO");
                entity.Property(e => e.Parametro)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("PARAMETRO");
                entity.Property(e => e.NomForma)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("NOM_FORMA");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<Pai>(entity =>
            {
                entity.HasKey(e => e.CodPais)
                    .HasName("COD_PAIS");
                entity.ToTable("PAIS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodPais)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_PAIS");
                entity.Property(e => e.NombrePais)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_PAIS");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<Paise>(entity =>
            {
                entity.HasKey(e => e.CodPais);
                entity.ToTable("PAISES", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodPais)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_PAIS");
                entity.Property(e => e.Abreviatura)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ABREVIATURA");
                entity.Property(e => e.CodigoArea)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CODIGO_AREA");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.Nacionalidad)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("NACIONALIDAD");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("newid()");
                entity.Property(e => e.Siglas)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("SIGLAS");
            });
            modelBuilder.Entity<ParametrosGenerale>(entity =>
            {
                entity.HasKey(e => new { e.Parametro, e.CodModulo });
                entity.ToTable("PARAMETROS_GENERALES", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.Parametro)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PARAMETRO");
                entity.Property(e => e.CodModulo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("COD_MODULO");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(240)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.Notificado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("NOTIFICADO");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
                entity.Property(e => e.Valor)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("VALOR");
                entity.HasOne(d => d.CodModuloNavigation)
                    .WithMany(p => p.ParametrosGenerales)
                    .HasForeignKey(d => d.CodModulo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PARAME_GRAL_MOD");
            });
            modelBuilder.Entity<Pasatiempo>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("PASATIEMPOS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodPersona)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("COD_PERSONA");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<PermisosOpcione>(entity =>
            {
                entity.HasKey(e => new { e.CodEmpresa, e.CodUsuario, e.CodModulo, e.Parametro, e.NomForma });
                entity.ToTable("PERMISOS_OPCIONES", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodEmpresa)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_EMPRESA");
                entity.Property(e => e.CodUsuario)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO");
                entity.Property(e => e.CodModulo)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_MODULO");
                entity.Property(e => e.Parametro)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PARAMETRO");
                entity.Property(e => e.NomForma)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NOM_FORMA");
                entity.Property(e => e.Permiso)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("PERMISO");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<Persona>(entity =>
            {
                entity.HasKey(e => e.CodPersona);

                entity.ToTable("PERSONAS", "INV");

                entity.HasIndex(e => new { e.CodSector, e.CodPersona }, "IND_SECTOR")
                    .IsUnique()
                    .HasFilter("([COD_SECTOR] IS NOT NULL)");

                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();

                entity.Property(e => e.CodPersona)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("COD_PERSONA");

                entity.Property(e => e.Email)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.ActualizadoPor)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ACTUALIZADO_POR");

                entity.Property(e => e.AltaPor)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ALTA_POR");

                entity.Property(e => e.CodEstadoCivil)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_ESTADO_CIVIL");

                entity.Property(e => e.CodIdent)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_IDENT");

                entity.Property(e => e.CodPais)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_PAIS");

                entity.Property(e => e.CodPerFisica)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("COD_PER_FISICA");

                entity.Property(e => e.CodPerJuridica)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("COD_PER_JURIDICA");

                entity.Property(e => e.CodPropietarioOld)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("COD_PROPIETARIO_OLD");


                entity.Property(e => e.CodSector)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_SECTOR");

                entity.Property(e => e.Conyugue)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("CONYUGUE");

                entity.Property(e => e.DirecElectronica)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DIREC_ELECTRONICA");

                entity.Property(e => e.EsCoordinador)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ES_COORDINADOR");

                entity.Property(e => e.EsFiscalizador)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ES_FISCALIZADOR");

                entity.Property(e => e.EsFisica)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ES_FISICA");

                entity.Property(e => e.EsFuncionarioSenacsa)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ES_FUNCIONARIO_SENACSA");

                entity.Property(e => e.EsMalDeudor)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ES_MAL_DEUDOR");

                entity.Property(e => e.EsPropietario)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ES_PROPIETARIO");

                entity.Property(e => e.EsVacunador)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ES_VACUNADOR");

                entity.Property(e => e.EsVeterinario)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ES_VETERINARIO");

                entity.Property(e => e.Estatal)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ESTATAL");

                entity.Property(e => e.FecActualizacion)
                    .HasPrecision(0)
                    .HasColumnName("FEC_ACTUALIZACION");

                entity.Property(e => e.FecAlta)
                    .HasPrecision(0)
                    .HasColumnName("FEC_ALTA");

                entity.Property(e => e.FecNacimiento)
                    .HasPrecision(0)
                    .HasColumnName("FEC_NACIMIENTO");

                entity.Property(e => e.Lucrativa)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("LUCRATIVA");

                entity.Property(e => e.NivelEstudios)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("NIVEL_ESTUDIOS");

                entity.Property(e => e.NombFantasia)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("NOMB_FANTASIA");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");

                entity.Property(e => e.NroRegistroProf)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("NRO_REGISTRO_PROF");

                entity.Property(e => e.NroRegistroSenacsa)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("NRO_REGISTRO_SENACSA");

                entity.Property(e => e.PaginaWeb)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("PAGINA_WEB");

                entity.Property(e => e.Profesion)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("PROFESION");

                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("newid()");

                entity.Property(e => e.Sexo)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SEXO");

                entity.Property(e => e.TipoSociedad)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_SOCIEDAD");

                entity.Property(e => e.TotalIngresos)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("TOTAL_INGRESOS");

            });
            modelBuilder.Entity<PersonasDup>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("PERSONAS_DUP", "INV");
                entity.Property(e => e.ActualizadoPor)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ACTUALIZADO_POR");
                entity.Property(e => e.AltaPor)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ALTA_POR");
                entity.Property(e => e.CodEstadoCivil)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_ESTADO_CIVIL");
                entity.Property(e => e.CodIdent)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_IDENT");
                entity.Property(e => e.CodPais)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_PAIS");
                entity.Property(e => e.CodPerFisica)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("COD_PER_FISICA");
                entity.Property(e => e.CodPerJuridica)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("COD_PER_JURIDICA");
                entity.Property(e => e.CodPersona)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("COD_PERSONA");
                entity.Property(e => e.CodPropietarioOld)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("COD_PROPIETARIO_OLD");
                entity.Property(e => e.CodSector)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_SECTOR");
                entity.Property(e => e.Conyugue)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("CONYUGUE");
                entity.Property(e => e.DirecElectronica)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DIREC_ELECTRONICA");
                entity.Property(e => e.EsCoordinador)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ES_COORDINADOR");
                entity.Property(e => e.EsFiscalizador)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ES_FISCALIZADOR");
                entity.Property(e => e.EsFisica)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ES_FISICA");
                entity.Property(e => e.EsFuncionarioSenacsa)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ES_FUNCIONARIO_SENACSA");
                entity.Property(e => e.EsMalDeudor)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ES_MAL_DEUDOR");
                entity.Property(e => e.EsPropietario)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ES_PROPIETARIO");
                entity.Property(e => e.EsVacunador)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ES_VACUNADOR");
                entity.Property(e => e.EsVeterinario)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ES_VETERINARIO");
                entity.Property(e => e.Estatal)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ESTATAL");
                entity.Property(e => e.FecActualizacion)
                    .HasPrecision(0)
                    .HasColumnName("FEC_ACTUALIZACION");
                entity.Property(e => e.FecAlta)
                    .HasPrecision(0)
                    .HasColumnName("FEC_ALTA");
                entity.Property(e => e.FecNacimiento)
                    .HasPrecision(0)
                    .HasColumnName("FEC_NACIMIENTO");
                entity.Property(e => e.Lucrativa)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("LUCRATIVA");
                entity.Property(e => e.NivelEstudios)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("NIVEL_ESTUDIOS");
                entity.Property(e => e.NombFantasia)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("NOMB_FANTASIA");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");
                entity.Property(e => e.NroRegistroProf)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("NRO_REGISTRO_PROF");
                entity.Property(e => e.NroRegistroSenacsa)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("NRO_REGISTRO_SENACSA");
                entity.Property(e => e.PaginaWeb)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("PAGINA_WEB");
                entity.Property(e => e.Profesion)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("PROFESION");
                entity.Property(e => e.Sexo)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SEXO");
                entity.Property(e => e.TipoSociedad)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_SOCIEDAD");
                entity.Property(e => e.TotalIngresos)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("TOTAL_INGRESOS");
            });
            modelBuilder.Entity<PjMarca>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("PJ_MARCAS", "INV");
                entity.Property(e => e.BolMarca1)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("BOL_MARCA1");
                entity.Property(e => e.BolMarca2)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("BOL_MARCA2");
                entity.Property(e => e.BolMarca3)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("BOL_MARCA3");
                entity.Property(e => e.CodDistrito)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("COD_DISTRITO");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.NombPropietario)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("NOMB_PROPIETARIO");
                entity.Property(e => e.NroCi)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("NRO_CI");
            });
            modelBuilder.Entity<PlanTable>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("PLAN_TABLE", "INV");
                entity.Property(e => e.Distribution)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("DISTRIBUTION");
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.ObjectInstance).HasColumnName("OBJECT_INSTANCE");
                entity.Property(e => e.ObjectName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("OBJECT_NAME")
                    .IsFixedLength();
                entity.Property(e => e.ObjectNode)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("OBJECT_NODE")
                    .IsFixedLength();
                entity.Property(e => e.ObjectOwner)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("OBJECT_OWNER")
                    .IsFixedLength();
                entity.Property(e => e.ObjectType)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("OBJECT_TYPE")
                    .IsFixedLength();
                entity.Property(e => e.Operation)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("OPERATION")
                    .IsFixedLength();
                entity.Property(e => e.Optimizer)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("OPTIMIZER");
                entity.Property(e => e.Options)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("OPTIONS")
                    .IsFixedLength();
                entity.Property(e => e.Other)
                    .IsUnicode(false)
                    .HasColumnName("OTHER");
                entity.Property(e => e.OtherTag)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("OTHER_TAG");
                entity.Property(e => e.ParentId).HasColumnName("PARENT_ID");
                entity.Property(e => e.PartitionStart)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("PARTITION_START");
                entity.Property(e => e.PartitionStop)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("PARTITION_STOP");
                entity.Property(e => e.Position).HasColumnName("POSITION");
                entity.Property(e => e.Remarks)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("REMARKS")
                    .IsFixedLength();
                entity.Property(e => e.SearchColumns).HasColumnName("SEARCH_COLUMNS");
                entity.Property(e => e.StatementId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("STATEMENT_ID")
                    .IsFixedLength();
                entity.Property(e => e.Timestamp)
                    .HasPrecision(0)
                    .HasColumnName("TIMESTAMP");
            });
            modelBuilder.Entity<PlsqlProfilerDatum>(entity =>
            {
                entity.HasKey(e => new { e.Runid, e.UnitNumber, e.Line })
                    .HasName("SYS_C007957");
                entity.ToTable("PLSQL_PROFILER_DATA", "INV");
                entity.Property(e => e.Runid).HasColumnName("RUNID");
                entity.Property(e => e.UnitNumber).HasColumnName("UNIT_NUMBER");
                entity.Property(e => e.Line).HasColumnName("LINE#");
                entity.Property(e => e.MaxTime).HasColumnName("MAX_TIME");
                entity.Property(e => e.MinTime).HasColumnName("MIN_TIME");
                entity.Property(e => e.Spare1).HasColumnName("SPARE1");
                entity.Property(e => e.Spare2).HasColumnName("SPARE2");
                entity.Property(e => e.Spare3).HasColumnName("SPARE3");
                entity.Property(e => e.Spare4).HasColumnName("SPARE4");
                entity.Property(e => e.TotalOccur).HasColumnName("TOTAL_OCCUR");
                entity.Property(e => e.TotalTime).HasColumnName("TOTAL_TIME");
                entity.HasOne(d => d.PlsqlProfilerUnit)
                    .WithMany(p => p.PlsqlProfilerData)
                    .HasForeignKey(d => new { d.Runid, d.UnitNumber })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SYS_C008041");
            });
            modelBuilder.Entity<PlsqlProfilerRun>(entity =>
            {
                entity.HasKey(e => e.Runid)
                    .HasName("SYS_C007958");
                entity.ToTable("PLSQL_PROFILER_RUNS", "INV");
                entity.Property(e => e.Runid).HasColumnName("RUNID");
                entity.Property(e => e.RelatedRun).HasColumnName("RELATED_RUN");
                entity.Property(e => e.RunComment)
                    .HasMaxLength(2047)
                    .IsUnicode(false)
                    .HasColumnName("RUN_COMMENT");
                entity.Property(e => e.RunComment1)
                    .HasMaxLength(2047)
                    .IsUnicode(false)
                    .HasColumnName("RUN_COMMENT1");
                entity.Property(e => e.RunDate)
                    .HasPrecision(0)
                    .HasColumnName("RUN_DATE");
                entity.Property(e => e.RunOwner)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("RUN_OWNER");
                entity.Property(e => e.RunSystemInfo)
                    .HasMaxLength(2047)
                    .IsUnicode(false)
                    .HasColumnName("RUN_SYSTEM_INFO");
                entity.Property(e => e.RunTotalTime).HasColumnName("RUN_TOTAL_TIME");
                entity.Property(e => e.Spare1)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("SPARE1");
            });
            modelBuilder.Entity<PlsqlProfilerUnit>(entity =>
            {
                entity.HasKey(e => new { e.Runid, e.UnitNumber })
                    .HasName("SYS_C007959");
                entity.ToTable("PLSQL_PROFILER_UNITS", "INV");
                entity.Property(e => e.Runid).HasColumnName("RUNID");
                entity.Property(e => e.UnitNumber).HasColumnName("UNIT_NUMBER");
                entity.Property(e => e.Spare1).HasColumnName("SPARE1");
                entity.Property(e => e.Spare2).HasColumnName("SPARE2");
                entity.Property(e => e.TotalTime).HasColumnName("TOTAL_TIME");
                entity.Property(e => e.UnitName)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("UNIT_NAME");
                entity.Property(e => e.UnitOwner)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("UNIT_OWNER");
                entity.Property(e => e.UnitTimestamp)
                    .HasPrecision(0)
                    .HasColumnName("UNIT_TIMESTAMP");
                entity.Property(e => e.UnitType)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("UNIT_TYPE");
                entity.HasOne(d => d.Run)
                    .WithMany(p => p.PlsqlProfilerUnits)
                    .HasForeignKey(d => d.Runid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SYS_C008042");
            });
            modelBuilder.Entity<PreMaPrestamo>(entity =>
            {
                entity.HasKey(e => e.PreCodigo);
                entity.ToTable("PRE_MA_PRESTAMOS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.PreCodigo)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("PRE_CODIGO");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<Profesione>(entity =>
            {
                entity.HasKey(e => e.CodProfesion);

                entity.ToTable("PROFESIONES", "INV");

                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();

                entity.Property(e => e.CodProfesion)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_PROFESION");

                entity.Property(e => e.Abreviatura)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ABREVIATURA");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("newid()");

                entity.Property(e => e.Siglas)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("SIGLAS");
            });
            modelBuilder.Entity<PropiedadesXPersona>(entity =>
            {
                entity.HasKey(e => new { e.CodPersona, e.NumFinca, e.Distrito })
                    .HasName("PK_PROPIEDADES_X_PERSONAS");
                entity.ToTable("PROPIEDADES_X_PERSONA", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodPersona)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_PERSONA");
                entity.Property(e => e.NumFinca)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("NUM_FINCA");
                entity.Property(e => e.Distrito)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DISTRITO");
                entity.Property(e => e.CodMoneda)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("COD_MONEDA");
                entity.Property(e => e.CuentaCte)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CUENTA_CTE");
                entity.Property(e => e.HipotecadoA)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("HIPOTECADO_A");
                entity.Property(e => e.IndHipoteca)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IND_HIPOTECA");
                entity.Property(e => e.NomTitular)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("NOM_TITULAR");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
                entity.Property(e => e.Superficie)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("SUPERFICIE");
                entity.Property(e => e.Ubicacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("UBICACION");
                entity.Property(e => e.ValComercial)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("VAL_COMERCIAL");
                entity.Property(e => e.ValHipoteca)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("VAL_HIPOTECA");
            });
            modelBuilder.Entity<Propietario>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("PROPIETARIOS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.IdPropietario)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("ID_PROPIETARIO");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<Provincia>(entity =>
            {
                entity.HasKey(e => new { e.CodPais, e.CodProvincia });
                entity.ToTable("PROVINCIAS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodPais)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_PAIS");
                entity.Property(e => e.CodProvincia)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_PROVINCIA");
                entity.Property(e => e.Abreviatura)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ABREVIATURA");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("newid()");
                entity.HasOne(d => d.CodPaisNavigation)
                    .WithMany(p => p.Provincia)
                    .HasForeignKey(d => d.CodPais)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROVIN_PAI");
            });
            modelBuilder.Entity<ProvisorioCab>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("PROVISORIO_CAB", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodEjercicio)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_EJERCICIO");
                entity.Property(e => e.CodEmpresa)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_EMPRESA");
                entity.Property(e => e.CodModulo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("COD_MODULO");
                entity.Property(e => e.CodSucursal)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_SUCURSAL");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(240)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.EjercicioCerrado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("EJERCICIO_CERRADO");
                entity.Property(e => e.FecAsiento)
                    .HasPrecision(0)
                    .HasColumnName("FEC_ASIENTO");
                entity.Property(e => e.IndMayorizado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IND_MAYORIZADO");
                entity.Property(e => e.NumAsiento)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("NUM_ASIENTO");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
                entity.Property(e => e.TipAsiento)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("TIP_ASIENTO");
            });
            modelBuilder.Entity<RmAnulacionesMarca>(entity =>
            {
                entity.HasKey(e => e.IdMarca)
                   .HasName("RM_ANULACIONES_MARCAS_PK");

                entity.ToTable("RM_ANULACIONES_MARCAS", "INV");

                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();

                entity.Property(e => e.IdMarca)
                    .HasColumnType("numeric(15, 0)")
                    .HasColumnName("ID_MARCA");

                entity.Property(e => e.FechaAlta)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_ALTA");

                entity.Property(e => e.IdMotivoAnulacion)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ID_MOTIVO_ANULACION");

                entity.Property(e => e.IdUsuario)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ID_USUARIO");

                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("newid()");

                entity.HasOne(d => d.IdMotivoAnulacionNavigation)
                    .WithMany(p => p.RmAnulacionesMarcas)
                    .HasForeignKey(d => d.IdMotivoAnulacion)
                    .HasConstraintName("FK_MAR_RM_ANU_MAR_ID_MOT");

            });
            modelBuilder.Entity<RmAsigDi>(entity =>
            {
                entity.HasKey(e => new { e.NroEntrada, e.NroAsignacion });
                entity.ToTable("RM_ASIG_DIS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.Desasignado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DESASIGNADO");
                entity.Property(e => e.FechaAsignada)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_ASIGNADA");
                entity.Property(e => e.FechaDesasignacion)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_DESASIGNACION");
                entity.Property(e => e.IdUsuarioAlta)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ID_USUARIO_ALTA");
                entity.Property(e => e.IdUsuarioAsignado)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ID_USUARIO_ASIGNADO");
                entity.Property(e => e.NroAsignacion)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("NRO_ASIGNACION");
                entity.Property(e => e.NroAsignacionNueva)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("NRO_ASIGNACION_NUEVA");
                entity.Property(e => e.NroEntrada)
                    .HasColumnType("numeric(20, 0)")
                    .HasColumnName("NRO_ENTRADA");
                entity.Property(e => e.Recibido)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("RECIBIDO");
                entity.Property(e => e.RepId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("REP_ID");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("newid()");
                entity.Property(e => e.UsuarioDesasignacion)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("USUARIO_DESASIGNACION");
            });
            modelBuilder.Entity<RmAsignacione>(entity =>
            {
                entity.HasKey(e => new { e.NroEntrada, e.NroAsignacion });
                entity.ToTable("RM_ASIGNACIONES", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.Desasignado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DESASIGNADO");
                entity.Property(e => e.FechaAsignada)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_ASIGNADA");
                entity.Property(e => e.FechaDesasignacion)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_DESASIGNACION");
                entity.Property(e => e.IdUsuarioAlta)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ID_USUARIO_ALTA");
                entity.Property(e => e.IdUsuarioAsignado)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ID_USUARIO_ASIGNADO");
                entity.Property(e => e.NroAsignacion)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("NRO_ASIGNACION");
                entity.Property(e => e.NroAsignacionNueva)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("NRO_ASIGNACION_NUEVA");
                entity.Property(e => e.NroEntrada)
                    .HasColumnType("numeric(20, 0)")
                    .HasColumnName("NRO_ENTRADA");
                entity.Property(e => e.Recibido)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("RECIBIDO");
                entity.Property(e => e.RepId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("REP_ID");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("newid()");
                entity.Property(e => e.UsuarioDesasignacion)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("USUARIO_DESASIGNACION"); 
                entity.Property(e => e.TipoAsignacion)
                    .HasMaxLength(1)
                    .HasColumnName("TIPO_ASIGNACION");
            });
            modelBuilder.Entity<RmAutorizante>(entity =>
            {
                entity.HasKey(e => e.IdAutorizante)
                   .HasName("RM_AUTORIZANTES_PK");

                entity.ToTable("RM_AUTORIZANTES", "INV");

                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();

                entity.Property(e => e.IdAutorizante).ValueGeneratedOnAdd()
                    .HasColumnType("numeric(8, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_AUTORIZANTE");

                entity.Property(e => e.CodCiudad)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_CIUDAD");

                entity.Property(e => e.DescripAutorizante)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIP_AUTORIZANTE");

                entity.Property(e => e.MatriculaRegistro)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("MATRICULA_REGISTRO");

                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("newid()");

                entity.Property(e => e.TipoAutorizante)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_AUTORIZANTE");
            });
            modelBuilder.Entity<RmBoletasMarca>(entity =>
            {
                entity.HasKey(e => new { e.NroBoleta, e.Descripcion });
                entity.ToTable("RM_BOLETAS_MARCA", "INV");
                entity.Property(e => e.NroBoleta)
                    .HasColumnType("numeric(15, 0)")
                    .HasColumnName("NRO_BOLETA");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.Asignado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ASIGNADO");
            });
            modelBuilder.Entity<RmCambiosEstado>(entity =>
            {
                entity.HasKey(e => e.NroMovimiento);
                entity.ToTable("RM_CAMBIOS_ESTADOS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.NroMovimiento)
                    .HasColumnType("numeric(20, 0)")
                    .HasColumnName("NRO_MOVIMIENTO");
                entity.Property(e => e.CodOperacion)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("COD_OPERACION");
                entity.Property(e => e.CodUsuario)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO");
                entity.Property(e => e.EstadoAnterior)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO_ANTERIOR");
                entity.Property(e => e.EstadoNuevo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO_NUEVO");
                entity.Property(e => e.FechaOperacion)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_OPERACION");
                entity.Property(e => e.NroEntrada)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("NRO_ENTRADA");
                entity.Property(e => e.Observacion)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("OBSERVACION");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("newid()");
                entity.HasOne(d => d.CodOperacionNavigation)
                     .WithMany(p => p.RmCambiosEstados)
                     .HasForeignKey(d => d.CodOperacion)
                     .HasConstraintName("RM_CAMBIOS_ESTADOS_FK");
            });
            modelBuilder.Entity<RmCambiosOficina>(entity =>
            {
                entity.HasKey(e => e.NroMovimiento)
                    .HasName("RM_CAMBIOS_OFICINA_PK");

                entity.ToTable("RM_CAMBIOS_OFICINA", "INV");

                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique()
                    .HasFillFactor(100);

                entity.Property(e => e.NroMovimiento)
                    .HasColumnType("numeric(20, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("NRO_MOVIMIENTO");

                entity.Property(e => e.CodOperacion)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("COD_OPERACION");

                entity.Property(e => e.CodUsuario)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO");

                entity.Property(e => e.FechaOperacion)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_OPERACION");

                entity.Property(e => e.NroEntrada)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("NRO_ENTRADA");

                entity.Property(e => e.Observacion)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("OBSERVACION");

                entity.Property(e => e.OficRetiroAnt)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("OFIC_RETIRO_ANT");

                entity.Property(e => e.OficRetiroNuev)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("OFIC_RETIRO_NUEV");

                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");

                //entity.HasOne(d => d.CodOperacionNavigation)
                //    .WithMany(p => p.RmCambiosOficinas)
                //    .HasForeignKey(d => d.CodOperacion)
                //    .HasConstraintName("RM_CAMBIOS_OFICINA_FK");
            });
            modelBuilder.Entity<RmCaracSimilare>(entity =>
            {
                entity.HasKey(e => e.CodCaracSimilares)
                    .HasName("PK_CARAC_SIMILARES");
                entity.ToTable("RM_CARAC_SIMILARES", "INV");
                entity.Property(e => e.CodCaracSimilares)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("COD_CARAC_SIMILARES");
                entity.Property(e => e.ListaCaracteres)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("LISTA_CARACTERES");
            });
            modelBuilder.Entity<RmCertificacione>(entity =>
            {
                entity.HasKey(e => e.IdCertificacion)
                    .HasName("RM_CERTIFICACIONES_PK");

                entity.ToTable("RM_CERTIFICACIONES", "INV");

                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();

                entity.Property(e => e.IdCertificacion)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_CERTIFICACION");

                entity.Property(e => e.Entregado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ENTREGADO");

                entity.Property(e => e.EstadoCertificacion)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO_CERTIFICACION");

                entity.Property(e => e.EstadoRegistral)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ESTADO_REGISTRAL");

                entity.Property(e => e.EstadoTransaccion)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO_TRANSACCION");

                entity.Property(e => e.FechaCaducidad)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_CADUCIDAD");

                entity.Property(e => e.FechaCertificacion)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_CERTIFICACION");

                entity.Property(e => e.FechaRegistro)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_REGISTRO");

                entity.Property(e => e.IdAutorizante)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("ID_AUTORIZANTE");

                entity.Property(e => e.IdBeneficiario)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ID_BENEFICIARIO");

                entity.Property(e => e.IdMarca)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ID_MARCA");

                entity.Property(e => e.IdUsuario)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("ID_USUARIO");

                entity.Property(e => e.NombreEscribano)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ESCRIBANO");

                entity.Property(e => e.NroBoleta)
                    .HasColumnType("numeric(15, 0)")
                    .HasColumnName("NRO_BOLETA");

                entity.Property(e => e.NroBoletaSenal)
                    .HasColumnType("numeric(15, 0)")
                    .HasColumnName("NRO_BOLETA_SENAL");

                entity.Property(e => e.NroEntrada)
                    .HasColumnType("numeric(20, 0)")
                    .HasColumnName("NRO_ENTRADA");

                entity.Property(e => e.Observacion)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("OBSERVACION");

                entity.Property(e => e.ObservacionSup)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("OBSERVACION_SUP");

                entity.Property(e => e.RepId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("REP_ID");

                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.TipoOperacion)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_OPERACION");

                entity.Property(e => e.UsuarioSup)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("USUARIO_SUP");
            });
            modelBuilder.Entity<RmCertificacionesDet>(entity =>
            {
                entity.HasKey(e => new { e.NroDetalle, e.IdCertificacion })
                    .HasName("PK_CERTIFICACIONES_DET");
                entity.ToTable("RM_CERTIFICACIONES_DET", "INV");
                entity.HasIndex(e => new { e.IdCertificacion, e.IdBeneficiario }, "UK_CERTIFICACIONES_DET")
                    .IsUnique();
                entity.Property(e => e.NroDetalle)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("NRO_DETALLE");
                entity.Property(e => e.IdCertificacion)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ID_CERTIFICACION");
                entity.Property(e => e.IdBeneficiario)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ID_BENEFICIARIO");
                //entity.HasOne(d => d.IdBeneficiarioNavigation)
                //    .WithMany(p => p.RmCertificacionesDets)
                //    .HasForeignKey(d => d.IdBeneficiario)
                //    .HasConstraintName("FK_CERTIF_PER");
                //entity.HasOne(d => d.IdCertificacionNavigation)
                //    .WithMany(p => p.RmCertificacionesDets)
                //    .HasForeignKey(d => d.IdCertificacion)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_CERTIFICACIONES");
            });
            modelBuilder.Entity<RmConceptosLiquidacione>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("RM_CONCEPTOS_LIQUIDACIONES", "INV");
                entity.Property(e => e.DescripcionConcepto)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION_CONCEPTO");
                entity.Property(e => e.IdConcepto)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ID_CONCEPTO");
                entity.Property(e => e.MontoConcepto)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("MONTO_CONCEPTO");
                entity.Property(e => e.TipoConcepto)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_CONCEPTO");
                entity.Property(e => e.TipoSolicitud)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_SOLICITUD");
            });
            modelBuilder.Entity<RmDistrito>(entity =>
            {
                entity.HasKey(e => new { e.CodigoDepto, e.CodigoDistrito })
                    .HasName("PK_DISTRITO");
                entity.ToTable("RM_DISTRITOS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodigoDepto)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("CODIGO_DEPTO");
                entity.Property(e => e.CodigoDistrito)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("CODIGO_DISTRITO");
                entity.Property(e => e.DescripDistrito)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIP_DISTRITO");
                entity.Property(e => e.Nomenclador)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("NOMENCLADOR");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("newid()");
            });
            modelBuilder.Entity<RmEntradaBoletum>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("RM_ENTRADA_BOLETA", "INV");
                entity.Property(e => e.NroBoleta)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NRO_BOLETA");
                entity.Property(e => e.NroSenal)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NRO_SENAL");
                entity.Property(e => e.NumeroEntrada)
                    .HasColumnType("numeric(20, 0)")
                    .HasColumnName("NUMERO_ENTRADA");
            });
            modelBuilder.Entity<RmEstadosEntradum>(entity =>
            {
                entity.HasKey(e => e.CodigoEstado)
                    .HasName("PK_ESTADOS_ENTRADA");
                entity.ToTable("RM_ESTADOS_ENTRADA", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodigoEstado)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("CODIGO_ESTADO");
                entity.Property(e => e.DescripEstado)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIP_ESTADO");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("newid()");
            });
            modelBuilder.Entity<RmFormasConcurrencium>(entity =>
            {
                entity.HasKey(e => e.CodFormaConcurrencia)
                    .HasName("CONSTR$RM_FORMAS_CONCURRENCIA");
                entity.ToTable("RM_FORMAS_CONCURRENCIA", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodFormaConcurrencia)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_FORMA_CONCURRENCIA");
                entity.Property(e => e.DescFormaConcurrencia)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("DESC_FORMA_CONCURRENCIA");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<RmFormasGruposMenu>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("RM_FORMAS_GRUPOS_MENU", "INV");
                entity.Property(e => e.GrupoMenu)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("GRUPO_MENU");
                entity.Property(e => e.NomForma)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("NOM_FORMA");
            });
            modelBuilder.Entity<RmFormasMenu>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("RM_FORMAS_MENU", "INV");
                entity.Property(e => e.NomForma)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NOM_FORMA");
                entity.Property(e => e.NomItemMenu)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NOM_ITEM_MENU");
                entity.Property(e => e.NomMenu)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NOM_MENU");
                entity.Property(e => e.NomMenuPadre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NOM_MENU_PADRE");
            });
            modelBuilder.Entity<RmFormasTableCab>(entity =>
            {
                entity.HasKey(e => e.NombForma)
                    .HasName("PK_FORMAS_TABLE_CAB");
                entity.ToTable("RM_FORMAS_TABLE_CAB", "INV");
                entity.Property(e => e.NombForma)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("NOMB_FORMA");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
            });
            modelBuilder.Entity<RmFormasTableDet>(entity =>
            {
                entity.HasKey(e => new { e.NombForma, e.NroDetalle })
                    .HasName("PK_FORMAS_TABLE_DET");
                entity.ToTable("RM_FORMAS_TABLE_DET", "INV");
                entity.Property(e => e.NombForma)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("NOMB_FORMA");
                entity.Property(e => e.NroDetalle)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("NRO_DETALLE");
                entity.Property(e => e.NombTable)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("NOMB_TABLE");
                entity.HasOne(d => d.NombFormaNavigation)
                    .WithMany(p => p.RmFormasTableDets)
                    .HasForeignKey(d => d.NombForma)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FORMAS_TABLE");
            });
            modelBuilder.Entity<RmGruposMenu>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("RM_GRUPOS_MENU", "INV");
                entity.Property(e => e.CodGrupo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("COD_GRUPO");
                entity.Property(e => e.CodModulo)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("COD_MODULO");
            });
            modelBuilder.Entity<RmImagen2>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("RM_IMAGEN2", "INV");
                entity.Property(e => e.Ima)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("IMA");
                entity.Property(e => e.Imagen)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("IMAGEN");
            });
            modelBuilder.Entity<RmImagenTemp>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("RM_IMAGEN_TEMP", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.IdUsuario)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ID_USUARIO");
                entity.Property(e => e.Marca)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("MARCA");
                entity.Property(e => e.MarcaBlob).HasColumnName("MARCA_BLOB");
                entity.Property(e => e.MarcaSig)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("MARCA_SIG");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
                entity.Property(e => e.Senal)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("SENAL");
                entity.Property(e => e.SenalBlob).HasColumnName("SENAL_BLOB");
            });
            modelBuilder.Entity<RmImagenesSenale>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("RM_IMAGENES_SENALES", "INV");
                entity.Property(e => e.CodImgSenal).HasColumnName("COD_IMG_SENAL");
                entity.Property(e => e.CodTipoCorte)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("COD_TIPO_CORTE")
                    .IsFixedLength();
                entity.Property(e => e.UrlImg)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("URL_IMG");
            });
            modelBuilder.Entity<RmImgSenalArea>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("RM_IMG_SENAL_AREA", "INV");
                entity.Property(e => e.CodImgSenal).HasColumnName("COD_IMG_SENAL");
                entity.Property(e => e.CodImgSenalArea).HasColumnName("COD_IMG_SENAL_AREA");
            });
            modelBuilder.Entity<RmInforme>(entity =>
            {
                entity.HasKey(e => e.IdInforme)
                    .HasName("PK_INFORME");
                entity.ToTable("RM_INFORMES", "INV");
                entity.Property(e => e.IdInforme)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ID_INFORME");
                entity.Property(e => e.Entregado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ENTREGADO");
                entity.Property(e => e.EstadoInforme)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO_INFORME");
                entity.Property(e => e.EstadoRegistral)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ESTADO_REGISTRAL");
                entity.Property(e => e.EstadoTransaccion)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO_TRANSACCION");
                entity.Property(e => e.FechaCaducidad)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_CADUCIDAD");
                entity.Property(e => e.FechaCertificacion)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_CERTIFICACION");
                entity.Property(e => e.FechaRegistro)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_REGISTRO");
                entity.Property(e => e.IdAutorizante)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("ID_AUTORIZANTE");
                entity.Property(e => e.IdBeneficiario)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ID_BENEFICIARIO");
                entity.Property(e => e.IdMarca)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ID_MARCA");
                entity.Property(e => e.IdUsuario)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("ID_USUARIO");
                entity.Property(e => e.NombreEscribano)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ESCRIBANO");
                entity.Property(e => e.NroBoleta)
                    .HasColumnType("numeric(15, 0)")
                    .HasColumnName("NRO_BOLETA");
                entity.Property(e => e.NroBoletaSenal)
                    .HasColumnType("numeric(15, 0)")
                    .HasColumnName("NRO_BOLETA_SENAL");
                entity.Property(e => e.NroEntrada)
                    .HasColumnType("numeric(20, 0)")
                    .HasColumnName("NRO_ENTRADA");
                entity.Property(e => e.Observacion)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("OBSERVACION");
                entity.Property(e => e.ObservacionSup)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("OBSERVACION_SUP");
                entity.Property(e => e.RepId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("REP_ID");
                entity.Property(e => e.TipoOperacion)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_OPERACION");
                entity.Property(e => e.UsuarioSup)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("USUARIO_SUP");
                //entity.HasOne(d => d.NroEntradaNavigation)
                //    .WithMany(p => p.RmInformes)
                //    .HasForeignKey(d => d.NroEntrada)
                //    .HasConstraintName("FK_MAR_RM_INFOR_NRO_ENT");
            });
            modelBuilder.Entity<RmInformeDet>(entity =>
            {
                entity.HasKey(e => new { e.NroDetalle, e.IdInforme })
                    .HasName("PK_INFORME_DET");
                entity.ToTable("RM_INFORME_DET", "INV");
                entity.HasIndex(e => new { e.IdInforme, e.IdBeneficiario }, "UK_INFORME_DET")
                    .IsUnique();
                entity.Property(e => e.NroDetalle)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("NRO_DETALLE");
                entity.Property(e => e.IdInforme)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ID_INFORME");
                entity.Property(e => e.IdBeneficiario)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ID_BENEFICIARIO");
                entity.HasOne(d => d.IdBeneficiarioNavigation)
                    .WithMany(p => p.RmInformeDets)
                    .HasForeignKey(d => d.IdBeneficiario)
                    .HasConstraintName("FK_INFORM_PER");
                entity.HasOne(d => d.IdInformeNavigation)
                    .WithMany(p => p.RmInformeDets)
                    .HasForeignKey(d => d.IdInforme)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_INFORMES");
            });
            modelBuilder.Entity<RmInterviniente>(entity =>
            {
                entity.HasKey(e => e.IdProfesional)
                    .HasName("PK_INTERVINIENTES");
                entity.ToTable("RM_INTERVINIENTES", "INV");
                entity.Property(e => e.IdProfesional)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ID_PROFESIONAL");
                entity.Property(e => e.Apellidos)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("APELLIDOS");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");
                entity.Property(e => e.Nombres)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRES");
                entity.Property(e => e.TipoInterviniente)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_INTERVINIENTE");
            });
            modelBuilder.Entity<RmLevantamiento>(entity =>
            {
                entity.HasKey(e => e.IdLevantamiento)
                    .HasName("RM_LEVANTAMIENTOS_PK");

                entity.ToTable("RM_LEVANTAMIENTOS", "INV");

                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();

                entity.Property(e => e.IdLevantamiento)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_LEVANTAMIENTO");

                entity.Property(e => e.Entregado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ENTREGADO");

                entity.Property(e => e.EstadoRegistral)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ESTADO_REGISTRAL");

                entity.Property(e => e.EstadoTransaccion)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO_TRANSACCION");

                entity.Property(e => e.FechaActoJuridico)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_ACTO_JURIDICO");

                entity.Property(e => e.FechaAlta)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_ALTA");

                entity.Property(e => e.IdAutorizante)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("ID_AUTORIZANTE");

                entity.Property(e => e.IdMarca)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ID_MARCA");

                entity.Property(e => e.IdMedida)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ID_MEDIDA");

                entity.Property(e => e.IdUsuario)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ID_USUARIO");

                entity.Property(e => e.MontoLevantamiento)
                    .HasColumnType("numeric(15, 0)")
                    .HasColumnName("MONTO_LEVANTAMIENTO");

                entity.Property(e => e.NroBoleta)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NRO_BOLETA");

                entity.Property(e => e.NroDocumentoLevanta)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("NRO_DOCUMENTO_LEVANTA");

                entity.Property(e => e.NroEntrada)
                    .HasColumnType("numeric(20, 0)")
                    .HasColumnName("NRO_ENTRADA");

                entity.Property(e => e.NroOficio)
                    .HasColumnType("numeric(15, 0)")
                    .HasColumnName("NRO_OFICIO");

                entity.Property(e => e.Observacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("OBSERVACION");

                entity.Property(e => e.ObservacionSup)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("OBSERVACION_SUP");

                entity.Property(e => e.RepId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("REP_ID");

                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("newid()");

                entity.Property(e => e.TipoMoneda)
                    .HasColumnType("numeric(38, 0)")
                    .HasColumnName("TIPO_MONEDA");

                entity.Property(e => e.TipoOperacion)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_OPERACION");

                entity.Property(e => e.TotalParcial)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("TOTAL_PARCIAL");

                entity.Property(e => e.UsuarioSup)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("USUARIO_SUP");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.RmLevantamientos)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("RM_LEVANTAMIENTOS_FK");
            });
            modelBuilder.Entity<RmLiquidacionesMarca>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("RM_LIQUIDACIONES_MARCAS", "INV");
                entity.Property(e => e.Actualizado).HasColumnType("numeric(3, 0)");
                entity.Property(e => e.FechaUtilizacion)
                    .HasPrecision(0)
                    .HasColumnName("Fecha_utilizacion");
                entity.Property(e => e.IdLiquidacion)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("Id_liquidacion");
                entity.Property(e => e.Utilizado).HasColumnType("numeric(3, 0)");
            });
            modelBuilder.Entity<RmMarcaSenalDistrito>(entity =>
            {
                entity.HasKey(e => e.IdMarca)
                    .HasName("PK_MARCA_SENAL_DISTRIO");
                entity.ToTable("RM_MARCA_SENAL_DISTRITO", "INV");
                entity.Property(e => e.IdMarca)
                    .HasColumnType("numeric(15, 0)")
                    .HasColumnName("ID_MARCA");
                entity.Property(e => e.CodCiudad)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_CIUDAD");
                entity.Property(e => e.CodPais)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_PAIS");
                entity.Property(e => e.CodProvincia)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_PROVINCIA");
                entity.Property(e => e.CodigoSenal)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CODIGO_SENAL");
            });
            modelBuilder.Entity<RmMarcasSenale>(entity =>
            {
                entity.HasKey(e => e.IdMarca);
                entity.ToTable("RM_MARCAS_SENALES", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.IdMarca)
                    .HasColumnType("numeric(15, 0)")
                    .HasColumnName("ID_MARCA");
                entity.Property(e => e.CantidadGanado)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("CANTIDAD_GANADO");
                entity.Property(e => e.CodigoEstablecimiento)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("CODIGO_ESTABLECIMIENTO");
                entity.Property(e => e.CodigoSenal)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CODIGO_SENAL");
                entity.Property(e => e.EstadoMarca)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ESTADO_MARCA");
                entity.Property(e => e.EstadoProceso)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ESTADO_PROCESO");
                entity.Property(e => e.FechaAlta)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_ALTA");
                entity.Property(e => e.FechaAltaAnterior)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_ALTA_ANTERIOR");
                entity.Property(e => e.FechaBoletaAnterior)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_BOLETA_ANTERIOR");
                entity.Property(e => e.IdSrsImg)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ID_SRS_IMG");
                entity.Property(e => e.IdUsuario)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ID_USUARIO");
                entity.Property(e => e.MarcaNombre)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("MARCA_NOMBRE");
                entity.Property(e => e.MarcaTipo)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("MARCA_TIPO");
                entity.Property(e => e.NroBoleta)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NRO_BOLETA");
                entity.Property(e => e.NroBoletaAnterior)
                    .HasColumnType("numeric(15, 0)")
                    .HasColumnName("NRO_BOLETA_ANTERIOR");
                entity.Property(e => e.NumeroBoletaAnterior)
                    .HasColumnType("numeric(15, 0)")
                    .HasColumnName("NUMERO_BOLETA_ANTERIOR");
                entity.Property(e => e.NumeroEntrada)
                    .HasColumnType("numeric(20, 0)")
                    .HasColumnName("NUMERO_ENTRADA");
                entity.Property(e => e.ObsMarca)
                    .HasMaxLength(2500)
                    .IsUnicode(false)
                    .HasColumnName("OBS_MARCA");
                entity.Property(e => e.ObsSenal)
                    .HasMaxLength(2500)
                    .IsUnicode(false)
                    .HasColumnName("OBS_SENAL");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("newid()");
                entity.Property(e => e.Semejantea)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("SEMEJANTEA");
                entity.Property(e => e.SenalNombre)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("SENAL_NOMBRE");
                entity.Property(e => e.TipoGanado)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("TIPO_GANADO");
            });
            modelBuilder.Entity<RmMarcasXEstab>(entity =>
            {
                entity.HasKey(e => e.IdMarEst)
                   .HasName("RM_MARCAS_X_ESTAB_PK");

                entity.ToTable("RM_MARCAS_X_ESTAB", "INV");

                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();

                entity.Property(e => e.IdMarEst)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_MAR_EST");

                entity.Property(e => e.CodDistrito)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("COD_DISTRITO");

                entity.Property(e => e.CodEstable)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("COD_ESTABLE");

                entity.Property(e => e.CodEstablePj)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("COD_ESTABLE_PJ");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.GpsH)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("GPS_H");

                entity.Property(e => e.GpsS)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("GPS_S");

                entity.Property(e => e.GpsSc)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("GPS_SC");

                entity.Property(e => e.GpsV)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("GPS_V");

                entity.Property(e => e.GpsW)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("GPS_W");

                entity.Property(e => e.IdMarca)
                    .HasColumnType("numeric(15, 0)")
                    .HasColumnName("ID_MARCA");

                entity.Property(e => e.IdTransaccion)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ID_TRANSACCION");

                entity.Property(e => e.NroInscripcion)
                    .HasColumnType("numeric(15, 0)")
                    .HasColumnName("NRO_INSCRIPCION");

                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("newid()");
            });
            modelBuilder.Entity<RmMedidasPrenda>(entity =>
            {
                entity.HasKey(e => e.IdMedida)
                    .HasName("RM_MEDIDAS_PRENDAS_PK");

                entity.ToTable("RM_MEDIDAS_PRENDAS", "INV");

                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();

                entity.Property(e => e.IdMedida)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_MEDIDA");

                entity.Property(e => e.Acreedor)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("ACREEDOR");

                entity.Property(e => e.CantGanado)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("CANT_GANADO");

                entity.Property(e => e.CodDistrito)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("COD_DISTRITO");

                entity.Property(e => e.CodUsuario)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO");

                entity.Property(e => e.Deudor)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("DEUDOR");

                entity.Property(e => e.Entregado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ENTREGADO");

                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO");

                entity.Property(e => e.EstadoRegistral)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ESTADO_REGISTRAL");

                entity.Property(e => e.EstadoTransaccion)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO_TRANSACCION");

                entity.Property(e => e.FechaAutorizacion)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_AUTORIZACION");

                entity.Property(e => e.FechaInscripcion)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_INSCRIPCION");

                entity.Property(e => e.FechaOperacion)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_OPERACION");

                entity.Property(e => e.FechaVencimiento)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_VENCIMIENTO");

                entity.Property(e => e.Folio)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("FOLIO");

                entity.Property(e => e.IdAutorizante)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("ID_AUTORIZANTE");

                entity.Property(e => e.IdMarca)
                    .HasColumnType("numeric(15, 0)")
                    .HasColumnName("ID_MARCA");

                entity.Property(e => e.IdTransaccion)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ID_TRANSACCION");

                entity.Property(e => e.Instrumento)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("INSTRUMENTO");

                entity.Property(e => e.Juicio)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("JUICIO");

                entity.Property(e => e.Juzgado)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("JUZGADO");

                entity.Property(e => e.Libro)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("LIBRO");

                entity.Property(e => e.MontoDeJusticia)
                    .HasColumnType("numeric(12, 0)")
                    .HasColumnName("MONTO_DE_JUSTICIA");

                entity.Property(e => e.MontoPrenda)
                    .HasColumnType("numeric(12, 0)")
                    .HasColumnName("MONTO_PRENDA");

                entity.Property(e => e.NroBoleta)
                    .HasColumnType("varchar(10, 0)")
                    .HasColumnName("NRO_BOLETA");

                entity.Property(e => e.NroBoletaSenal)
                    .HasColumnType("varchar(10, 0)")
                    .HasColumnName("NRO_BOLETA_SENAL");

                entity.Property(e => e.NroEntrada)
                    .HasColumnType("numeric(20, 0)")
                    .HasColumnName("NRO_ENTRADA");

                entity.Property(e => e.NroOficio)
                    .HasColumnType("numeric(15, 0)")
                    .HasColumnName("NRO_OFICIO");

                entity.Property(e => e.Observacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("OBSERVACION");

                entity.Property(e => e.ObservacionSup)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("OBSERVACION_SUP");

                entity.Property(e => e.RepId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("REP_ID");

                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Secretaria)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SECRETARIA");

                entity.Property(e => e.TipoEmbargo)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_EMBARGO");

                entity.Property(e => e.TipoMoneda)
                    .HasColumnType("numeric(38, 0)")
                    .HasColumnName("TIPO_MONEDA");

                entity.Property(e => e.TipoOperacion)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_OPERACION");

                entity.Property(e => e.UsuarioSup)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("USUARIO_SUP");

                entity.HasOne(d => d.IdAutorizanteNavigation)
                    .WithMany(p => p.RmMedidasPrenda)
                    .HasForeignKey(d => d.IdAutorizante)
                    .HasConstraintName("FK_RM_MED_PRE_AUTORI");

                entity.HasOne(d => d.NroEntradaNavigation)
                    .WithMany(p => p.RmMedidasPrenda)
                    .HasForeignKey(d => d.NroEntrada)
                    .HasConstraintName("FK_MAR_RM_MED_PRE_NRO_ENT");

                entity.HasOne(d => d.TipoMonedaNavigation)
                    .WithMany(p => p.RmMedidasPrenda)
                    .HasForeignKey(d => d.TipoMoneda)
                    .HasConstraintName("FK_RM_MED_PRE_TIP_MON");
            });
            modelBuilder.Entity<RmMesaEntradaDup>(entity =>
            {
                entity.HasKey(e => e.NumeroLiquidacion)
                    .HasName("RM_MESA_ENTRADA_DUP_PK");

                entity.ToTable("RM_MESA_ENTRADA_DUP", "INV");

                entity.Property(e => e.NumeroLiquidacion)
                    .HasColumnType("numeric(30, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("NUMERO_LIQUIDACION");

                entity.Property(e => e.CodigoOficina)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("CODIGO_OFICINA");

                entity.Property(e => e.DocReingresante)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("DOC_REINGRESANTE");

                entity.Property(e => e.EstadoEntrada)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ESTADO_ENTRADA");

                entity.Property(e => e.FechaEntrada)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_ENTRADA");

                entity.Property(e => e.FechaSalida)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_SALIDA");

                entity.Property(e => e.IdAutorizante)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("ID_AUTORIZANTE");

                entity.Property(e => e.Impreso)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IMPRESO");

                entity.Property(e => e.NomTitular)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOM_TITULAR");

                entity.Property(e => e.NombrePresentador)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_PRESENTADOR");

                entity.Property(e => e.NombreReingresante)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_REINGRESANTE");

                entity.Property(e => e.NombreRetirador)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_RETIRADOR");

                entity.Property(e => e.NroBoleta)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NRO_BOLETA");

                entity.Property(e => e.NroDocumentoPresentador)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("NRO_DOCUMENTO_PRESENTADOR");

                entity.Property(e => e.NroDocumentoRetirador)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("NRO_DOCUMENTO_RETIRADOR");

                entity.Property(e => e.NroDocumentoTitular)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("NRO_DOCUMENTO_TITULAR");

                entity.Property(e => e.NroEntradaOriginal)
                    .HasColumnType("numeric(20, 0)")
                    .HasColumnName("NRO_ENTRADA_ORIGINAL");

                entity.Property(e => e.NroFormulario)
                    .HasColumnType("numeric(30, 0)")
                    .HasColumnName("NRO_FORMULARIO");

                entity.Property(e => e.NroOficio)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NRO_OFICIO");

                entity.Property(e => e.NumeroEntrada)
                    .HasColumnType("numeric(20, 0)")
                    .HasColumnName("NUMERO_ENTRADA");

                entity.Property(e => e.Reingreso)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("REINGRESO");

                entity.Property(e => e.TipoDocReingresante)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("TIPO_DOC_REINGRESANTE");

                entity.Property(e => e.TipoDocumento)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_DOCUMENTO");

                entity.Property(e => e.TipoDocumentoPresentador)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("TIPO_DOCUMENTO_PRESENTADOR");

                entity.Property(e => e.TipoDocumentoRetirador)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("TIPO_DOCUMENTO_RETIRADOR");

                entity.Property(e => e.TipoDocumentoTitular)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_DOCUMENTO_TITULAR");

                entity.Property(e => e.TipoSolicitud)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("TIPO_SOLICITUD");

                entity.Property(e => e.UsuarioEntrada)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("USUARIO_ENTRADA");

                entity.Property(e => e.UsuarioSalida)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("USUARIO_SALIDA");
            });
            modelBuilder.Entity<RmMesaEntradum>(entity =>
            {
                entity.HasKey(e => e.NumeroEntrada)
                    .HasName("RM_MESA_ENTRADA_PK");

                entity.ToTable("RM_MESA_ENTRADA", "INV");

                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();

                entity.Property(e => e.NumeroEntrada)
                    .HasColumnType("numeric(20, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("NUMERO_ENTRADA");

                entity.Property(e => e.Anulado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ANULADO");

                entity.Property(e => e.CantidadReingreso)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("CANTIDAD_REINGRESO");

                entity.Property(e => e.CodigoOficina)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("CODIGO_OFICINA");

                entity.Property(e => e.CodOficinaRetiro)
                   .HasColumnType("numeric(5, 0)")
                   .HasColumnName("COD_OFICINA_RETIRO");

                entity.Property(e => e.DocReingresante)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("DOC_REINGRESANTE");

                entity.Property(e => e.EstadoEntrada)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ESTADO_ENTRADA");

                entity.Property(e => e.FechaAntReingreso)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_ANT_REINGRESO");

                entity.Property(e => e.FechaAntSalReing)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_ANT_SAL_REING");

                entity.Property(e => e.FechaAnulacion)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_ANULACION");

                entity.Property(e => e.FechaEntrada)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_ENTRADA");

                entity.Property(e => e.FechaRecSalida)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_REC_SALIDA");

                entity.Property(e => e.FechaSalida)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_SALIDA");

                entity.Property(e => e.IdAutorizante)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("ID_AUTORIZANTE");

                entity.Property(e => e.IdMotivoAnulacion)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ID_MOTIVO_ANULACION");

                entity.Property(e => e.Impreso)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IMPRESO");

                entity.Property(e => e.MontoLiquidacion)
                    .HasColumnType("numeric(12, 3)")
                    .HasColumnName("MONTO_LIQUIDACION");

                entity.Property(e => e.NomTitular)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOM_TITULAR");

                entity.Property(e => e.NombrePresentador)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_PRESENTADOR");

                entity.Property(e => e.NombreReingresante)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_REINGRESANTE");

                entity.Property(e => e.NombreRetirador)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_RETIRADOR");

                entity.Property(e => e.NroBoleta)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NRO_BOLETA");

                entity.Property(e => e.NroBoletaAntSenhal)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NRO_BOLETA_ANT_SENHAL");

                entity.Property(e => e.NroBoletaAnterior)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NRO_BOLETA_ANTERIOR");

                entity.Property(e => e.NroDocumentoPresentador)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("NRO_DOCUMENTO_PRESENTADOR");

                entity.Property(e => e.NroDocumentoRetirador)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("NRO_DOCUMENTO_RETIRADOR");

                entity.Property(e => e.NroDocumentoTitular)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("NRO_DOCUMENTO_TITULAR");

                entity.Property(e => e.NroEntradaOriginal)
                    .HasColumnType("numeric(20, 0)")
                    .HasColumnName("NRO_ENTRADA_ORIGINAL");

                entity.Property(e => e.NroFormulario)
                    .HasColumnType("numeric(30, 0)")
                    .HasColumnName("NRO_FORMULARIO");

                entity.Property(e => e.NroOficio)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NRO_OFICIO");

                entity.Property(e => e.NroSenal)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NRO_SENAL");

                entity.Property(e => e.NumeroLiquidacion)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("NUMERO_LIQUIDACION");

                entity.Property(e => e.NumeroLiquidacionLetras)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("NUMERO_LIQUIDACION_LETRAS");

                entity.Property(e => e.Reingreso)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("REINGRESO");

                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("newid()");

                entity.Property(e => e.TipoDocReingresante)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("TIPO_DOC_REINGRESANTE");

                entity.Property(e => e.TipoDocumento)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_DOCUMENTO");

                entity.Property(e => e.TipoDocumentoPresentador)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("TIPO_DOCUMENTO_PRESENTADOR");

                entity.Property(e => e.TipoDocumentoRetirador)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("TIPO_DOCUMENTO_RETIRADOR");

                entity.Property(e => e.TipoDocumentoTitular)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_DOCUMENTO_TITULAR");

                entity.Property(e => e.TipoSolicitud)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("TIPO_SOLICITUD");

                entity.Property(e => e.UsuarioAnulacion)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("USUARIO_ANULACION");

                entity.Property(e => e.UsuarioEntrada)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("USUARIO_ENTRADA");

                entity.Property(e => e.UsuarioSalida)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("USUARIO_SALIDA");

                entity.Property(e => e.ArchivoPDF)
                   .HasColumnType("VARBINARY(MAX)")
                   .HasColumnName("ARCHIVOPDF");
                entity.Property(e => e.AnexoPDF)
                   .HasColumnType("VARBINARY(MAX)")
                   .HasColumnName("ANEXOPDF");

                //entity.Property(e => e.RutaPdf)
                //   .HasColumnType("varchar(100)")
                //   .HasColumnName("RUTAPDF");

                //entity.HasOne(d => d.CodigoOficinaNavigation)
                //    .WithMany(p => p.RmMesaEntrada)
                //    .HasForeignKey(d => d.CodigoOficina)
                //    .HasConstraintName("FK_MESA_ENT_OFIC_REG");

                //entity.HasOne(d => d.IdAutorizanteNavigation)
                //    .WithMany(p => p.RmMesaEntrada)
                //    .HasForeignKey(d => d.IdAutorizante)
                //    .HasConstraintName("FK_MESA_ENT_AUT");

                //entity.HasOne(d => d.IdMotivoAnulacionNavigation)
                //    .WithMany(p => p.RmMesaEntrada)
                //    .HasForeignKey(d => d.IdMotivoAnulacion)
                //    .HasConstraintName("FK_MESA_ENT_MOT_ANULACION");

                //entity.HasOne(d => d.TipoSolicitudNavigation)
                //    .WithMany(p => p.RmMesaEntrada)
                //    .HasForeignKey(d => d.TipoSolicitud)
                //    .HasConstraintName("FK_MESA_ENT_TIP_SOL");
            });

            modelBuilder.Entity<RmMotivosAnulacion>(entity =>
            {
                entity.HasKey(e => e.IdMotivoAnulacion);
                entity.ToTable("RM_MOTIVOS_ANULACION", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.IdMotivoAnulacion).ValueGeneratedOnAdd()
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ID_MOTIVO_ANULACION");
                entity.Property(e => e.DescripMotivo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIP_MOTIVO");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("newid()");
            });
            modelBuilder.Entity<RmMovimientosDoc>(entity =>
            {
                entity.HasKey(e => e.NroMovimiento)
                    .HasName("PK__RM_MOVIM__7AA94DEA5F027159");

                entity.ToTable("RM_MOVIMIENTOS_DOC", "INV");

                entity.Property(e => e.NroMovimiento)
                    .HasColumnType("numeric(15, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("NRO_MOVIMIENTO");

                entity.Property(e => e.CodOperacion)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("COD_OPERACION");

                entity.Property(e => e.CodUsuario)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO");

                entity.Property(e => e.EstadoEntrada)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("ESTADO_ENTRADA");

                entity.Property(e => e.FechaOperacion)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_OPERACION");

                entity.Property(e => e.NroEntrada)
                    .HasColumnType("numeric(20, 0)")
                    .HasColumnName("NRO_ENTRADA");

                entity.Property(e => e.NroMovimientoRef)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NRO_MOVIMIENTO_REF");
            });
            modelBuilder.Entity<RmNotasNegativa>(entity =>
            {
                entity.HasKey(e => e.IdEntrada)
                    .HasName("PK_ENTRADA");
                entity.ToTable("RM_NOTAS_NEGATIVAS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.IdEntrada)
                    .HasColumnType("numeric(15, 0)")
                    .HasColumnName("ID_ENTRADA");
                entity.Property(e => e.DescripNotaNegativa)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIP_NOTA_NEGATIVA");
                entity.Property(e => e.FechaAlta)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_ALTA");
                entity.Property(e => e.IdUsuario)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("ID_USUARIO");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<RmOficinasRegistrale>(entity =>
            {
                entity.HasKey(e => e.CodigoOficina);
                entity.ToTable("RM_OFICINAS_REGISTRALES", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodigoOficina).ValueGeneratedOnAdd()
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("CODIGO_OFICINA");
                entity.Property(e => e.DescripOficina)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIP_OFICINA");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("newid()");
            });
            modelBuilder.Entity<RmOperacionesSist>(entity =>
            {
                entity.HasKey(e => e.CodOperacion)
                    .HasName("PK_OPERACIONES_SIST");
                entity.ToTable("RM_OPERACIONES_SIST", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodOperacion)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("COD_OPERACION");
                entity.Property(e => e.DescOperacion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DESC_OPERACION");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<RmParametrosNotificado>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("RM_PARAMETROS_NOTIFICADOS", "INV");
                entity.Property(e => e.CodModulo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("COD_MODULO");
                entity.Property(e => e.Notificado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("NOTIFICADO");
                entity.Property(e => e.NroOperacion)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("NRO_OPERACION");
                entity.Property(e => e.OldCodModulo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("OLD_COD_MODULO");
                entity.Property(e => e.OldNotificado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("OLD_NOTIFICADO");
                entity.Property(e => e.OldParametro)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("OLD_PARAMETRO");
                entity.Property(e => e.OldValor)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("OLD_VALOR");
                entity.Property(e => e.Operacion)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("OPERACION");
                entity.Property(e => e.Parametro)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("PARAMETRO");
                entity.Property(e => e.Valor)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("VALOR");
            });
            modelBuilder.Entity<RmReingreso>(entity =>
            {
                entity.HasKey(e => e.IdReingreso)
                    .HasName("RM_REINGRESOS_PK");

                entity.ToTable("RM_REINGRESOS", "INV");

                entity.Property(e => e.IdReingreso)
                    .HasColumnType("numeric(15, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REINGRESO");

                entity.Property(e => e.DocReingresante)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("DOC_REINGRESANTE");

                entity.Property(e => e.FechaRegistro)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_REGISTRO");

                entity.Property(e => e.FechaReingreso)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_REINGRESO");

                entity.Property(e => e.FechaSalida)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_SALIDA");

                entity.Property(e => e.NombreReingresante)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_REINGRESANTE");

                entity.Property(e => e.NroEntrada)
                    .HasColumnType("numeric(20, 0)")
                    .HasColumnName("NRO_ENTRADA");

                entity.Property(e => e.Observacion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("OBSERVACION");

                entity.Property(e => e.TipoDocReingresante)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("TIPO_DOC_REINGRESANTE");

                entity.Property(e => e.UsuarioInscriptor)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("USUARIO_INSCRIPTOR");

                entity.Property(e => e.UsuarioReingreso)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("USUARIO_REINGRESO");

                entity.Property(e => e.UsuarioSalida)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("USUARIO_SALIDA");
            });
            modelBuilder.Entity<RmSemejanteMarca>(entity =>
            {
                entity.HasKey(e => new { e.IdMarca, e.IdMarcaSem })
                    .HasName("RM_SEMEJANTES_PK");
                entity.ToTable("RM_SEMEJANTE_MARCA", "INV");
                entity.Property(e => e.IdMarca)
                    .HasColumnType("numeric(15, 0)")
                    .HasColumnName("ID_MARCA");
                entity.Property(e => e.IdMarcaSem)
                    .HasColumnType("numeric(15, 0)")
                    .HasColumnName("ID_MARCA_SEM");
                entity.Property(e => e.FecProceso)
                    .HasPrecision(0)
                    .HasColumnName("FEC_PROCESO");
                entity.Property(e => e.PorcSemejanza)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("PORC_SEMEJANZA");
            });
            modelBuilder.Entity<RmSemejanteSenal>(entity =>
            {
                entity.HasKey(e => e.IdMarca)
                    .HasName("PK_ID_MARCA");
                entity.ToTable("RM_SEMEJANTE_SENAL", "INV");
                entity.Property(e => e.IdMarca).HasColumnName("ID_MARCA");
                entity.Property(e => e.FechaProceso)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_PROCESO");
                entity.Property(e => e.IdMarcaSem).HasColumnName("ID_MARCA_SEM");
            });
            modelBuilder.Entity<RmTipoSolicitud>(entity =>
            {
                entity.HasKey(e => e.TipoSolicitud)
                    .HasName("PK_TIPO_SOLICITUD");
                entity.ToTable("RM_TIPO_SOLICITUD", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.TipoSolicitud).ValueGeneratedOnAdd()
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("TIPO_SOLICITUD");
                entity.Property(e => e.BloqueaEntrada)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("BLOQUEA_ENTRADA");
                entity.Property(e => e.DescripSolicitud)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIP_SOLICITUD");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("newid()");
            });
            modelBuilder.Entity<RmTiposDocumento>(entity =>
            {
                entity.HasKey(e => e.TipoDocumento)
                    .HasName("RM_TIPOS_DOCUMENTOS_PK");

                entity.ToTable("RM_TIPOS_DOCUMENTOS", "INV");

                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();

                entity.Property(e => e.TipoDocumento)
                    .HasColumnType("numeric(2, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("TIPO_DOCUMENTO");

                entity.Property(e => e.DescripTipoDocumento)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIP_TIPO_DOCUMENTO");

                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("newid()");
            });
            modelBuilder.Entity<RmTiposLiquidacion>(entity =>
            {
                entity.HasKey(e => e.IdTipoLiquidacion);
                entity.ToTable("RM_TIPOS_LIQUIDACION", "INV");
                entity.Property(e => e.DescripcionTipoLiquidacion)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION_TIPO_LIQUIDACION");
                entity.Property(e => e.IdConceptoLiquidacion)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ID_CONCEPTO_LIQUIDACION");
                entity.Property(e => e.IdTipoLiquidacion)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ID_TIPO_LIQUIDACION");
            });
            modelBuilder.Entity<RmTiposMoneda>(entity =>
            {
                entity.HasKey(e => e.TipoMoneda);
                entity.ToTable("RM_TIPOS_MONEDAS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.DescripTipoMoneda)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIP_TIPO_MONEDA");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("newid()");
                entity.Property(e => e.TipoMoneda).HasColumnName("TIPO_MONEDA");
            });
            modelBuilder.Entity<RmTiposOperacione>(entity =>
            {
                entity.HasKey(e => e.TipoOperacion)
                    .HasName("PK_TIPO");
                entity.ToTable("RM_TIPOS_OPERACIONES", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.TipoOperacion)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TIPO_OPERACION");
                entity.Property(e => e.DescripTipoOperacion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIP_TIPO_OPERACION");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("newid()");
            });
            modelBuilder.Entity<RmTiposPropiedad>(entity =>
            {
                entity.HasKey(e => e.IdTipoPropiedad)
                    .HasName("PK_TIPOS_PROPIEDAD");
                entity.ToTable("RM_TIPOS_PROPIEDAD", "INV");
                entity.Property(e => e.IdTipoPropiedad)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ID_TIPO_PROPIEDAD");
                entity.Property(e => e.DescripcionTipoPropiedad)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION_TIPO_PROPIEDAD");
            });
            modelBuilder.Entity<RmTiposUsuario>(entity =>
            {
                entity.HasKey(e => e.IdTipoUsuario)
                    .HasName("PK_TIPOUSU");
                entity.ToTable("RM_TIPOS_USUARIOS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.IdTipoUsuario)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ID_TIPO_USUARIO");
                entity.Property(e => e.CodigoTipoUsuario)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("CODIGO_TIPO_USUARIO");
                entity.Property(e => e.DescripcionTipoUsuario)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION_TIPO_USUARIO");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("newid()");
            });
            modelBuilder.Entity<RmTitularesMarca>(entity =>
            {
                entity.HasKey(e => e.IdTm)
                     .HasName("RM_TITULARES_MARCAS_PK");

                entity.ToTable("RM_TITULARES_MARCAS", "INV");

                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();

                entity.Property(e => e.IdTm)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_TM");

                entity.Property(e => e.CodUsuario)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO");

                entity.Property(e => e.EsPropietario)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ES_PROPIETARIO");

                entity.Property(e => e.EstadoTitularidad)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ESTADO_TITULARIDAD");

                entity.Property(e => e.FechaRegistro)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_REGISTRO");

                entity.Property(e => e.IdMarca)
                    .HasColumnType("numeric(15, 0)")
                    .HasColumnName("ID_MARCA");

                entity.Property(e => e.IdPropietario)
                    .HasColumnType("varchar(30)")
                    .HasColumnName("ID_PROPIETARIO");

                entity.Property(e => e.IdTipoPropiedad)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("ID_TIPO_PROPIEDAD");

                entity.Property(e => e.IdTitular)
                    .HasColumnType("varchar(30)")
                    .HasColumnName("ID_TITULAR");

                entity.Property(e => e.IdTransaccion)
                    .HasColumnType("numeric(20, 0)")
                    .HasColumnName("ID_TRANSACCION");

                entity.Property(e => e.IdUsuario)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID_USUARIO");

                entity.Property(e => e.PorcentajeTitularidad)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("PORCENTAJE_TITULARIDAD");

                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("newid()");
            });
            modelBuilder.Entity<RmTransaccione>(entity =>
            {
                entity.HasKey(e => e.IdTransaccion);
                entity.ToTable("RM_TRANSACCIONEScopia", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.IdTransaccion)
                    .ValueGeneratedOnAdd()
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ID_TRANSACCION");
                entity.Property(e => e.Asiento)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ASIENTO");
                entity.Property(e => e.CodEstadoCivilApoderado)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_ESTADO_CIVIL_APODERADO");
                entity.Property(e => e.CodEstadoCivilTitular)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_ESTADO_CIVIL_TITULAR");
                entity.Property(e => e.CodProfesionApoderado)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_PROFESION_APODERADO");
                entity.Property(e => e.CodProfesionTitular)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_PROFESION_TITULAR");
                entity.Property(e => e.Comentario)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");
                entity.Property(e => e.DireccionApoderado)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("DIRECCION_APODERADO");
                entity.Property(e => e.DireccionTitular)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("DIRECCION_TITULAR");
                entity.Property(e => e.Entregado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ENTREGADO");
                entity.Property(e => e.EntregadoDis)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ENTREGADO_DIS");
                entity.Property(e => e.EstadoRegistral)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ESTADO_REGISTRAL");
                entity.Property(e => e.EstadoTransaccion)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO_TRANSACCION");
                entity.Property(e => e.FecActoJuridico)
                    .HasPrecision(0)
                    .HasColumnName("FEC_ACTO_JURIDICO");
                entity.Property(e => e.FecAlta)
                    .HasPrecision(0)
                    .HasColumnName("FEC_ALTA");
                entity.Property(e => e.FechaActoJuridico)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_ACTO_JURIDICO");
                entity.Property(e => e.FechaAlta)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_ALTA");
                entity.Property(e => e.FechaEscrituraDsAi)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_ESCRITURA_DS_AI");
                entity.Property(e => e.FechaRecDis)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_REC_DIS");
                entity.Property(e => e.FirmanteRuego)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("FIRMANTE_RUEGO");
                entity.Property(e => e.FormaConcurre)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("FORMA_CONCURRE");
                entity.Property(e => e.FormaConcurrente)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FORMA_CONCURRENTE");
                entity.Property(e => e.IdAutorizante)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ID_AUTORIZANTE");
                entity.Property(e => e.IdEscribanoJuez)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ID_ESCRIBANO_JUEZ");
                entity.Property(e => e.IdMarca)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ID_MARCA");
                entity.Property(e => e.IdTranfe)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ID_TRANFE");
                entity.Property(e => e.IdUsuario)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ID_USUARIO");
                entity.Property(e => e.IdUsuarioRecDis)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ID_USUARIO_REC_DIS");
                entity.Property(e => e.Imagen).HasColumnName("IMAGEN");
                entity.Property(e => e.MontoOperacion)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("MONTO_OPERACION");
                entity.Property(e => e.NombreRepresentante)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_REPRESENTANTE");
                entity.Property(e => e.NroBolMarcaAnt)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NRO_BOL_MARCA_ANT");
                entity.Property(e => e.NroBolSenhalAnt)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NRO_BOL_SENHAL_ANT");
                entity.Property(e => e.NroBoleta)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NRO_BOLETA");
                entity.Property(e => e.NroEscrituraDsAi)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NRO_ESCRITURA_DS_AI");
                entity.Property(e => e.NroOficio)
                    .HasColumnType("numeric(15, 0)")
                    .HasColumnName("NRO_OFICIO");
                entity.Property(e => e.NumeroEntrada)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("NUMERO_ENTRADA");
                entity.Property(e => e.Observacion)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("OBSERVACION");
                entity.Property(e => e.ObservacionSup)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("OBSERVACION_SUP");
                entity.Property(e => e.RepId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("REP_ID");
                entity.Property(e => e.Representante)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("REPRESENTANTE");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("newid()");
                entity.Property(e => e.Semejanza)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("SEMEJANZA");
                entity.Property(e => e.Serie)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SERIE");
                entity.Property(e => e.TipoMoneda)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_MONEDA");
                entity.Property(e => e.TipoOperacion)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_OPERACION");
                entity.Property(e => e.UsuarioSup)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("USUARIO_SUP");
                //entity.HasOne(d => d.IdAutorizanteNavigation)
                //    .WithMany(p => p.RmTransacciones)
                //    .HasForeignKey(d => d.IdAutorizante)
                //    .HasConstraintName("FK_MAR_RM_TRAN_ID_AUT");
            });
            modelBuilder.Entity<RmUsuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario);
                entity.ToTable("RM_USUARIOS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.IdUsuario)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ID_USUARIO");
                entity.Property(e => e.CodigoOficina)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("CODIGO_OFICINA");
                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");
                entity.Property(e => e.FechaAlta)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_ALTA");
                entity.Property(e => e.FechaBaja)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_BAJA");
                entity.Property(e => e.IdEstado)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ID_ESTADO");
                entity.Property(e => e.IdTipoUsuario)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ID_TIPO_USUARIO");
                entity.Property(e => e.NombreUsuario)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_USUARIO");
                entity.Property(e => e.PasswordBaseDatos)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("PASSWORD_BASE_DATOS");
                entity.Property(e => e.PasswordUsuario)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("PASSWORD_USUARIO");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
                entity.Property(e => e.Username)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("USERNAME");
                entity.Property(e => e.UsuarioBaseDatos)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("USUARIO_BASE_DATOS");
                entity.HasOne(d => d.CodigoOficinaNavigation)
                    .WithMany(p => p.RmUsuarios)
                    .HasForeignKey(d => d.CodigoOficina)
                    .HasConstraintName("FK_RM_USUARIOS_RM_OFI");
            });
            modelBuilder.Entity<Sectore>(entity =>
            {
                entity.HasKey(e => new { e.CodEmpresa, e.CodSector });
                entity.ToTable("SECTORES", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodEmpresa)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_EMPRESA");
                entity.Property(e => e.CodSector)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_SECTOR");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.IndGestion)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IND_GESTION");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<SectoresEcon>(entity =>
            {
                entity.HasKey(e => e.CodSector);
                entity.ToTable("SECTORES_ECON", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodSector)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("COD_SECTOR");
                entity.Property(e => e.Comision)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("COMISION");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<SegurosPersona>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("SEGUROS_PERSONA", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodPersona)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_PERSONA");
                entity.Property(e => e.CompaniaAseguradora)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("COMPANIA_ASEGURADORA");
                entity.Property(e => e.FecVencimiento)
                    .HasPrecision(0)
                    .HasColumnName("FEC_VENCIMIENTO");
                entity.Property(e => e.MontoSeguro)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("MONTO_SEGURO");
                entity.Property(e => e.NomBeneficiario)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NOM_BENEFICIARIO");
                entity.Property(e => e.RiesgosCubiertos)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("RIESGOS_CUBIERTOS");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<SeriesComprob>(entity =>
            {
                entity.HasKey(e => new { e.CodEmpresa, e.TipComprobante, e.SerComprobante });
                entity.ToTable("SERIES_COMPROB", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodEmpresa)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_EMPRESA");
                entity.Property(e => e.TipComprobante)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("TIP_COMPROBANTE");
                entity.Property(e => e.SerComprobante)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("SER_COMPROBANTE");
                entity.Property(e => e.Exento)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("EXENTO");
                entity.Property(e => e.NumFinal)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("NUM_FINAL");
                entity.Property(e => e.NumInicial)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("NUM_INICIAL");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
                entity.Property(e => e.SerArticulo)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("SER_ARTICULO");
            });
            modelBuilder.Entity<SocioClube>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("SOCIO_CLUBES", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodPersona)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("COD_PERSONA");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<SrsImg>(entity =>
            {
                entity.HasKey(e => e.ImageId)
                    .HasName("SYS_C008007");
                entity.ToTable("SRS_IMG", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.ImageId)
                    .HasColumnType("numeric(38, 0)")
                    .HasColumnName("IMAGE_ID");
                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("NAME");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
                entity.Property(e => e.XmlData).HasColumnName("XML_DATA");
            });
            modelBuilder.Entity<SrsImgDatum>(entity =>
            {
                entity.HasKey(e => new { e.ImageId, e.ImageSeq })
                    .HasName("SYS_C008008");
                entity.ToTable("SRS_IMG_DATA", "INV");
                entity.Property(e => e.ImageId)
                    .HasColumnType("numeric(38, 0)")
                    .HasColumnName("IMAGE_ID");
                entity.Property(e => e.ImageSeq)
                    .HasColumnType("numeric(38, 0)")
                    .HasColumnName("IMAGE_SEQ");
                entity.Property(e => e.ImageData)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("IMAGE_DATA");
                entity.Property(e => e.ImageSig)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("IMAGE_SIG");
                entity.Property(e => e.RawData).HasColumnName("RAW_DATA");
            });
            modelBuilder.Entity<SrsTmpImg>(entity =>
            {
                entity.HasKey(e => new { e.TUid, e.TPid, e.TIid })
                    .HasName("SYS_C008009");
                entity.ToTable("SRS_TMP_IMG", "INV");
                entity.Property(e => e.TUid)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("T_UID");
                entity.Property(e => e.TPid)
                    .HasColumnType("numeric(38, 0)")
                    .HasColumnName("T_PID");
                entity.Property(e => e.TIid)
                    .HasColumnType("numeric(38, 0)")
                    .HasColumnName("T_IID");
                entity.Property(e => e.XmlData).HasColumnName("XML_DATA");
            });
            modelBuilder.Entity<SrsTmpImgDatum>(entity =>
            {
                entity.HasKey(e => new { e.TUid, e.TPid, e.TIid, e.TSeq })
                    .HasName("SYS_C008010");
                entity.ToTable("SRS_TMP_IMG_DATA", "INV");
                entity.Property(e => e.TUid)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("T_UID");
                entity.Property(e => e.TPid)
                    .HasColumnType("numeric(38, 0)")
                    .HasColumnName("T_PID");
                entity.Property(e => e.TIid)
                    .HasColumnType("numeric(38, 0)")
                    .HasColumnName("T_IID");
                entity.Property(e => e.TSeq)
                    .HasColumnType("numeric(38, 0)")
                    .HasColumnName("T_SEQ");
                entity.Property(e => e.ImageData)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("IMAGE_DATA");
                entity.Property(e => e.ImageSig)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("IMAGE_SIG");
                entity.Property(e => e.RawData).HasColumnName("RAW_DATA");
            });
            modelBuilder.Entity<SrsTmpRaw>(entity =>
            {
                entity.HasKey(e => new { e.TUid, e.TPid, e.TIid })
                    .HasName("SYS_C008011");
                entity.ToTable("SRS_TMP_RAW", "INV");
                entity.Property(e => e.TUid)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("T_UID");
                entity.Property(e => e.TPid)
                    .HasColumnType("numeric(38, 0)")
                    .HasColumnName("T_PID");
                entity.Property(e => e.TIid)
                    .HasColumnType("numeric(38, 0)")
                    .HasColumnName("T_IID");
                entity.Property(e => e.TData).HasColumnName("T_DATA");
            });
            modelBuilder.Entity<SrsTmpRe>(entity =>
            {
                entity.HasKey(e => new { e.TUid, e.TPid, e.TIid, e.TSeq, e.ImageId, e.ImageSeq })
                    .HasName("SYS_C008012");
                entity.ToTable("SRS_TMP_RES", "INV");
                entity.Property(e => e.TUid)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("T_UID");
                entity.Property(e => e.TPid)
                    .HasColumnType("numeric(38, 0)")
                    .HasColumnName("T_PID");
                entity.Property(e => e.TIid)
                    .HasColumnType("numeric(38, 0)")
                    .HasColumnName("T_IID");
                entity.Property(e => e.TSeq)
                    .HasColumnType("numeric(38, 0)")
                    .HasColumnName("T_SEQ");
                entity.Property(e => e.ImageId)
                    .HasColumnType("numeric(38, 0)")
                    .HasColumnName("IMAGE_ID");
                entity.Property(e => e.ImageSeq)
                    .HasColumnType("numeric(38, 0)")
                    .HasColumnName("IMAGE_SEQ");
                entity.Property(e => e.ResScore)
                    .HasColumnType("numeric(38, 0)")
                    .HasColumnName("RES_SCORE");
            });
            modelBuilder.Entity<StListaPrecio>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("ST_LISTA_PRECIOS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodEmpresa)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_EMPRESA");
                entity.Property(e => e.CodProveedor)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_PROVEEDOR");
                entity.Property(e => e.CodSucursal)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_SUCURSAL");
                entity.Property(e => e.DescArticulo)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("DESC_ARTICULO");
                entity.Property(e => e.Fecha)
                    .HasPrecision(0)
                    .HasColumnName("FECHA");
                entity.Property(e => e.Garantia)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("GARANTIA");
                entity.Property(e => e.GrupoArt)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("GRUPO_ART");
                entity.Property(e => e.Precio)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PRECIO");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<SubtiposTran>(entity =>
            {
                entity.HasKey(e => new { e.CodEmpresa, e.CodModulo, e.TipoTrans, e.SubtipoTrans });
                entity.ToTable("SUBTIPOS_TRANS", "INV");
                entity.HasIndex(e => new { e.CodEmpresa, e.TipoTrans, e.SubtipoTrans }, "IND_SUBTIPOS_TRANS");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodEmpresa)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_EMPRESA");
                entity.Property(e => e.CodModulo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("COD_MODULO");
                entity.Property(e => e.TipoTrans)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("TIPO_TRANS");
                entity.Property(e => e.SubtipoTrans)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("SUBTIPO_TRANS");
                entity.Property(e => e.Abreviatura)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ABREVIATURA");
                entity.Property(e => e.AfectaAsiento)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("AFECTA_ASIENTO");
                entity.Property(e => e.AfectaImponible)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("AFECTA_IMPONIBLE");
                entity.Property(e => e.AfectaIva)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("AFECTA_IVA");
                entity.Property(e => e.AfectaRenta)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("AFECTA_RENTA");
                entity.Property(e => e.CargaDeposito)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("CARGA_DEPOSITO");
                entity.Property(e => e.CargaOtros)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("CARGA_OTROS");
                entity.Property(e => e.CargaValores)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("CARGA_VALORES");
                entity.Property(e => e.CodAgrupacion)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_AGRUPACION");
                entity.Property(e => e.CodCuenta)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_CUENTA");
                entity.Property(e => e.Concepto)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("CONCEPTO");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.GastoDespacho)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("GASTO_DESPACHO");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
                entity.Property(e => e.TipDocumento)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("TIP_DOCUMENTO");
                entity.Property(e => e.UsaDinero)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("USA_DINERO");
                entity.Property(e => e.VerificaValores)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("VERIFICA_VALORES");
                entity.HasOne(d => d.CodEmpresaNavigation)
                    .WithMany(p => p.SubtiposTrans)
                    .HasForeignKey(d => d.CodEmpresa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SUBTIP_TRA_EMP");
                entity.HasOne(d => d.CodModuloNavigation)
                    .WithMany(p => p.SubtiposTrans)
                    .HasForeignKey(d => d.CodModulo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SUBTIP_TRA_MOD");
                entity.HasOne(d => d.TiposTran)
                    .WithMany(p => p.SubtiposTrans)
                    .HasForeignKey(d => new { d.CodEmpresa, d.CodModulo, d.TipoTrans })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SUBTIP_TRA_TPT");
            });
            modelBuilder.Entity<Sucursale>(entity =>
            {
                entity.HasKey(e => new { e.CodEmpresa, e.CodSucursal });
                entity.ToTable("SUCURSALES", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodEmpresa)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_EMPRESA");
                entity.Property(e => e.CodSucursal)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_SUCURSAL");
                entity.Property(e => e.CasillaCorreo)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("CASILLA_CORREO");
                entity.Property(e => e.CodBarrio)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_BARRIO");
                entity.Property(e => e.CodCiudad)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_CIUDAD");
                entity.Property(e => e.CodCustodioIni)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_CUSTODIO_INI");
                entity.Property(e => e.CodPais)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_PAIS");
                entity.Property(e => e.CodProvincia)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_PROVINCIA");
                entity.Property(e => e.CodRegional)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_REGIONAL");
                entity.Property(e => e.CodSucursalCentral)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("COD_SUCURSAL_CENTRAL");
                entity.Property(e => e.CodigoPostal)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CODIGO_POSTAL");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.DetalleDir)
                    .HasMaxLength(160)
                    .IsUnicode(false)
                    .HasColumnName("DETALLE_DIR");
                entity.Property(e => e.EsMatriz)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ES_MATRIZ");
                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO");
                entity.Property(e => e.IndUmDef)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IND_UM_DEF");
                entity.Property(e => e.ManejaStock)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("MANEJA_STOCK");
                entity.Property(e => e.NroPatronal)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NRO_PATRONAL");
                entity.Property(e => e.PlazoEnvio)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("PLAZO_ENVIO");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("newid()");
                entity.Property(e => e.StockPropio)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STOCK_PROPIO");
                entity.Property(e => e.TrabajaDom)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("TRABAJA_DOM");
                entity.Property(e => e.TrabajaSab)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("TRABAJA_SAB");
                entity.HasOne(d => d.CodEmpresaNavigation)
                    .WithMany(p => p.Sucursales)
                    .HasForeignKey(d => d.CodEmpresa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SUCUR_EMP");
                entity.HasOne(d => d.CodPaisNavigation)
                    .WithMany(p => p.Sucursales)
                    .HasForeignKey(d => d.CodPais)
                    .HasConstraintName("FK_SUCUR_PAI");
                entity.HasOne(d => d.CodP)
                    .WithMany(p => p.Sucursales)
                    .HasForeignKey(d => new { d.CodPais, d.CodProvincia })
                    .HasConstraintName("FK_SUCUR_PROVIN");
                entity.HasOne(d => d.Cod)
                    .WithMany(p => p.Sucursales)
                    .HasForeignKey(d => new { d.CodPais, d.CodProvincia, d.CodCiudad, d.CodBarrio })
                    .HasConstraintName("FK_SUCUR_BAR");
            });
            modelBuilder.Entity<TablasNoProcTransf>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("TABLAS_NO_PROC_TRANSF", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.NomTabla)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("NOM_TABLA");
                entity.Property(e => e.Operacion)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("OPERACION");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<Talonario>(entity =>
            {
                entity.HasKey(e => new { e.CodEmpresa, e.TipTalonario, e.NroTalonario })
                    .HasName("PKM_TALONARIOS");
                entity.ToTable("TALONARIOS", "INV");
                entity.HasIndex(e => new { e.CodEmpresa, e.TipTalonario, e.CodSucursal }, "IND_EMP_TIP_SUC");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodEmpresa)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_EMPRESA");
                entity.Property(e => e.TipTalonario)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIP_TALONARIO");
                entity.Property(e => e.NroTalonario)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("NRO_TALONARIO");
                entity.Property(e => e.Activo)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ACTIVO");
                entity.Property(e => e.CodSector)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_SECTOR");
                entity.Property(e => e.CodSucursal)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_SUCURSAL");
                entity.Property(e => e.Imprime)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IMPRIME");
                entity.Property(e => e.NumeroFinal)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("NUMERO_FINAL");
                entity.Property(e => e.NumeroInicial)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("NUMERO_INICIAL");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
                entity.Property(e => e.Serie)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("SERIE");
                entity.Property(e => e.TipImpresion)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIP_IMPRESION");
            });
            modelBuilder.Entity<TarjetasCredito>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("TARJETAS_CREDITO", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.ClaseTarjetaCredito)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("CLASE_TARJETA_CREDITO");
                entity.Property(e => e.CodPersona)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_PERSONA");
                entity.Property(e => e.MarcaTarjetaCredito)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("MARCA_TARJETA_CREDITO");
                entity.Property(e => e.NomEntidadEmisora)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NOM_ENTIDAD_EMISORA");
                entity.Property(e => e.NroTarjetaCredito)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("NRO_TARJETA_CREDITO");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<TarjetasDebito>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("TARJETAS_DEBITO", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.ClaseTarjetaDebito)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("CLASE_TARJETA_DEBITO");
                entity.Property(e => e.CodPersona)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_PERSONA");
                entity.Property(e => e.MarcaTarjetaDebito)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("MARCA_TARJETA_DEBITO");
                entity.Property(e => e.NomEntidadEmisora)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NOM_ENTIDAD_EMISORA");
                entity.Property(e => e.NroTarjetaDebito)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("NRO_TARJETA_DEBITO");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<TelefPersona>(entity =>
            {
                entity.HasKey(e => new { e.CodPersona, e.CodigoArea, e.NumTelefono });
                entity.ToTable("TELEF_PERSONAS", "INV");
                entity.HasIndex(e => e.CodPersona, "IND_TELEF_PERSONA");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodPersona)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_PERSONA");
                entity.Property(e => e.CodigoArea)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CODIGO_AREA");
                entity.Property(e => e.NumTelefono)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("NUM_TELEFONO");
                entity.Property(e => e.CodDireccion)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("COD_DIRECCION");
                entity.Property(e => e.Interno)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("INTERNO");
                entity.Property(e => e.Nota)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("NOTA");
                entity.Property(e => e.PorDefecto)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("POR_DEFECTO");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
                entity.Property(e => e.TelUbicacion)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("TEL_UBICACION");
                entity.Property(e => e.TipTelefono)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("TIP_TELEFONO");
              
            });
            modelBuilder.Entity<TiposCambio>(entity =>
            {
                entity.HasKey(e => new { e.CodMoneda, e.TipoCambio, e.FecTipoCambio });
                entity.ToTable("TIPOS_CAMBIO", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodMoneda)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("COD_MONEDA");
                entity.Property(e => e.TipoCambio)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_CAMBIO");
                entity.Property(e => e.FecTipoCambio)
                    .HasPrecision(0)
                    .HasColumnName("FEC_TIPO_CAMBIO");
                entity.Property(e => e.ActualizadoPor)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ACTUALIZADO_POR");
                entity.Property(e => e.AltaPor)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ALTA_POR");
                entity.Property(e => e.FecActualizado)
                    .HasPrecision(0)
                    .HasColumnName("FEC_ACTUALIZADO");
                entity.Property(e => e.FecAlta)
                    .HasPrecision(0)
                    .HasColumnName("FEC_ALTA");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
                entity.Property(e => e.TipoCambioContado)
                    .HasColumnType("numeric(12, 6)")
                    .HasColumnName("TIPO_CAMBIO_CONTADO");
                entity.Property(e => e.TipoCambioCredito)
                    .HasColumnType("numeric(12, 6)")
                    .HasColumnName("TIPO_CAMBIO_CREDITO");
                entity.Property(e => e.ValCompra)
                    .HasColumnType("numeric(12, 6)")
                    .HasColumnName("VAL_COMPRA");
                entity.Property(e => e.ValVenta)
                    .HasColumnType("numeric(12, 6)")
                    .HasColumnName("VAL_VENTA");
            });
            modelBuilder.Entity<TiposCambioMensual>(entity =>
            {
                entity.HasKey(e => new { e.Año, e.Mes, e.CodMoneda });
                entity.ToTable("TIPOS_CAMBIO_MENSUAL", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.Año)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("AÑO");
                entity.Property(e => e.Mes)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("MES");
                entity.Property(e => e.CodMoneda)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_MONEDA");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
                entity.Property(e => e.TipCambio)
                    .HasColumnType("numeric(18, 6)")
                    .HasColumnName("TIP_CAMBIO");
            });
            modelBuilder.Entity<TiposSociedad>(entity =>
            {
                entity.HasKey(e => e.TipoSociedad);
                entity.ToTable("TIPOS_SOCIEDAD", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.TipoSociedad)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_SOCIEDAD");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<TiposTalonario>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("TIPOS_TALONARIOS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
                entity.Property(e => e.TipTalonario)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIP_TALONARIO");
            });
            modelBuilder.Entity<TiposTran>(entity =>
            {
                entity.HasKey(e => new { e.CodEmpresa, e.CodModulo, e.TipoTrans });
                entity.ToTable("TIPOS_TRANS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodEmpresa)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_EMPRESA");
                entity.Property(e => e.CodModulo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("COD_MODULO");
                entity.Property(e => e.TipoTrans)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("TIPO_TRANS");
                entity.Property(e => e.Contabiliza)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("CONTABILIZA");
                entity.Property(e => e.CostoXServic)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("COSTO_X_SERVIC");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO");
                entity.Property(e => e.ImprimeComprob)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IMPRIME_COMPROB");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
                entity.HasOne(d => d.CodEmpresaNavigation)
                    .WithMany(p => p.TiposTrans)
                    .HasForeignKey(d => d.CodEmpresa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TIP_TRANS_EMP");
                entity.HasOne(d => d.CodModuloNavigation)
                    .WithMany(p => p.TiposTrans)
                    .HasForeignKey(d => d.CodModulo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TIP_TRANS_MOD");
            });
            modelBuilder.Entity<TmpSubtiposTran>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("TMP_SUBTIPOS_TRANS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CargaValores)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("CARGA_VALORES");
                entity.Property(e => e.CodEmpresa)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_EMPRESA");
                entity.Property(e => e.CodModulo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("COD_MODULO");
                entity.Property(e => e.Concepto)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("CONCEPTO");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
                entity.Property(e => e.SubtipoTrans)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("SUBTIPO_TRANS");
                entity.Property(e => e.TipoTrans)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("TIPO_TRANS");
                entity.Property(e => e.UsaDinero)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("USA_DINERO");
                entity.Property(e => e.VerificaValores)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("VERIFICA_VALORES");
            });
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.CodUsuario);
                entity.ToTable("USUARIOS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodUsuario)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO");
                entity.Property(e => e.AutorizaCtacte)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("AUTORIZA_CTACTE");
                entity.Property(e => e.AutorizaStock)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("AUTORIZA_STOCK");
                entity.Property(e => e.Clave)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE");
                entity.Property(e => e.CodArea)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_AREA");
                entity.Property(e => e.CodColorRegistro)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("COD_COLOR_REGISTRO");
                entity.Property(e => e.CodCustodio)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_CUSTODIO")
                    .HasDefaultValueSql("(' 01')");
                entity.Property(e => e.CodEmpresa)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_EMPRESA");
                entity.Property(e => e.CodGrupo)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("COD_GRUPO");
                entity.Property(e => e.CodOficina)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("COD_OFICINA");
                entity.Property(e => e.CodPersona)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_PERSONA");
                entity.Property(e => e.CodSucursal)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_SUCURSAL");
                entity.Property(e => e.EMail)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("E_MAIL");
                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO");
                entity.Property(e => e.FechaAlta)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_ALTA");
                entity.Property(e => e.FechaBaja)
                    .HasPrecision(0)
                    .HasColumnName("FECHA_BAJA");
                entity.Property(e => e.IdTipoUsuario)
                    .HasColumnType("numeric(30, 0)")
                    .HasColumnName("ID_TIPO_USUARIO");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("newid()");
                entity.HasOne(d => d.CodEmpresaNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.CodEmpresa)
                    .HasConstraintName("FK_USUARIOS_EMP");
               entity.HasOne(d => d.CodGrupoNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.CodGrupo)
                    .HasConstraintName("FK_USUARIOS_GRU");
                entity.HasOne(d => d.Cod)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => new { d.CodEmpresa, d.CodSucursal })
                    .HasConstraintName("FK_USUARIOS_SUC");
            });
            modelBuilder.Entity<UsuariosBkp>(entity =>
            {
                entity.HasKey(e => e.CodUsuario);
                entity.ToTable("USUARIOS_BKP", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodUsuario)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO");
                entity.Property(e => e.AutorizaCtacte)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("AUTORIZA_CTACTE");
                entity.Property(e => e.AutorizaStock)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("AUTORIZA_STOCK");
                entity.Property(e => e.Clave)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE");
                entity.Property(e => e.CodArea)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_AREA");
                entity.Property(e => e.CodColorRegistro)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("COD_COLOR_REGISTRO");
                entity.Property(e => e.CodCustodio)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_CUSTODIO")
                    .HasDefaultValueSql("(' 01')");
                entity.Property(e => e.CodEmpresa)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_EMPRESA");
                entity.Property(e => e.CodGrupo)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("COD_GRUPO");
                entity.Property(e => e.CodPersona)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_PERSONA");
                entity.Property(e => e.CodSucursal)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_SUCURSAL");
                entity.Property(e => e.EMail)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("E_MAIL");
                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
                entity.HasOne(d => d.CodEmpresaNavigation)
                    .WithMany(p => p.UsuariosBkps)
                    .HasForeignKey(d => d.CodEmpresa)
                    .HasConstraintName("FK_USUARIOS_EMP_BKP");
            });
            modelBuilder.Entity<UsuariosCustodio>(entity =>
            {
                entity.HasKey(e => new { e.CodEmpresa, e.CodUsuario, e.CodCustodio });
                entity.ToTable("USUARIOS_CUSTODIOS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodEmpresa)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_EMPRESA");
                entity.Property(e => e.CodUsuario)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO");
                entity.Property(e => e.CodCustodio)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_CUSTODIO");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<UsuariosGruposEmail>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("USUARIOS_GRUPOS_EMAIL", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodGrupo)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_GRUPO");
                entity.Property(e => e.CodUsuario)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<UsuariosTran>(entity =>
            {
                entity.HasKey(e => new { e.CodUsuario, e.CodModulo, e.TipoTrans });
                entity.ToTable("USUARIOS_TRANS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodUsuario)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO");
                entity.Property(e => e.CodModulo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("COD_MODULO");
                entity.Property(e => e.TipoTrans)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("TIPO_TRANS");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
                entity.HasOne(d => d.CodModuloNavigation)
                    .WithMany(p => p.UsuariosTrans)
                    .HasForeignKey(d => d.CodModulo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USUA_TRAN_MOD");
            });
            modelBuilder.Entity<Vestab>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("VESTAB", "INV");
                entity.Property(e => e.CodEstable)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("COD_ESTABLE");
                entity.Property(e => e.CodPropietario)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("COD_PROPIETARIO");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.Distrito)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DISTRITO");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");
            });
            modelBuilder.Entity<ZonasGeografica>(entity =>
            {
                entity.HasKey(e => e.CodZona);
                entity.ToTable("ZONAS_GEOGRAFICAS", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodZona)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_ZONA");
                entity.Property(e => e.CodGrupo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("COD_GRUPO");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<ZonasUbicacion>(entity =>
            {
                entity.HasKey(e => new { e.CodZona, e.CodPais, e.CodProvincia, e.CodCiudad, e.CodBarrio });
                entity.ToTable("ZONAS_UBICACION", "INV");
                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();
                entity.Property(e => e.CodZona)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_ZONA");
                entity.Property(e => e.CodPais)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_PAIS");
                entity.Property(e => e.CodProvincia)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_PROVINCIA");
                entity.Property(e => e.CodCiudad)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_CIUDAD");
                entity.Property(e => e.CodBarrio)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_BARRIO");
                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
                entity.HasOne(d => d.CodPaisNavigation)
                    .WithMany(p => p.ZonasUbicacions)
                    .HasForeignKey(d => d.CodPais)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ZON_UBI_PAI");
                entity.HasOne(d => d.CodZonaNavigation)
                    .WithMany(p => p.ZonasUbicacions)
                    .HasForeignKey(d => d.CodZona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ZONUBI_ZON");
                entity.HasOne(d => d.CodP)
                    .WithMany(p => p.ZonasUbicacions)
                    .HasForeignKey(d => new { d.CodPais, d.CodProvincia })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ZONAS_UBICACION_PROV");
                entity.HasOne(d => d.Cod)
                    .WithMany(p => p.ZonasUbicacions)
                    .HasForeignKey(d => new { d.CodPais, d.CodProvincia, d.CodCiudad, d.CodBarrio })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ZONAUBI_BAR");
            });

            modelBuilder.Entity<Comentario>(entity =>
            {
                entity.HasKey(e => e.Idcomentario);

                entity.ToTable("COMENTARIO", "INV");

                entity.Property(e => e.Idcomentario)
                    .ValueGeneratedNever()
                    .HasColumnName("IDCOMENTARIO");

                entity.Property(e => e.CodUsuario)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO");

                entity.Property(e => e.Descripcion)
                    .HasColumnType("text")
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.Fechacomentario)
                    .HasColumnType("date")
                    .HasColumnName("FECHACOMENTARIO");

                entity.Property(e => e.ForodebateIdforodebate).HasColumnName("FORODEBATE_IDFORODEBATE");

                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.CodUsuarioNavigation)
                    .WithMany(p => p.Comentarios)
                    .HasForeignKey(d => d.CodUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_COMENTARIO_USUARIO");

                //entity.HasOne(d => d.ForodebateIdforodebateNavigation)
                //    .WithMany(p => p.Comentarios)
                //    .HasForeignKey(d => d.ForodebateIdforodebate)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_COMENTARIO_FORODEBATE");
            });
            modelBuilder.Entity<Carrera>(entity =>
            {
                entity.HasKey(e => e.Idcarrera);

                entity.ToTable("CARRERA", "INV");

                entity.Property(e => e.Idcarrera)
                    .ValueGeneratedNever()
                    .HasColumnName("IDCARRERA");

                entity.Property(e => e.Descripcion)
                            .HasMaxLength(255)
                            .IsUnicode(false)
                            .HasColumnName("DESCRIPCION");

                entity.Property(e => e.Rowid)
                            .HasColumnName("ROWID")
                            .HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<CarreraUsuario>(entity =>
            {
                entity.HasKey(e => new {
                    e.Idcarrera,
                    e.CodUsuario
                });

                entity.ToTable("CARRERA_USUARIO", "INV");

                entity.Property(e => e.Idcarrera).HasColumnName("IDCARRERA");

                entity.Property(e => e.CodUsuario)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO");

                entity.Property(e => e.Promo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PROMO");

                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.CodUsuarioNavigation)
                    .WithMany(p => p.CarreraUsuarios)
                    .HasForeignKey(d => d.CodUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CARRERA_USUARIO_USUARIO");
            });
            modelBuilder.Entity<Datosacademico>(entity =>
            {
                entity.HasKey(e => e.Iddatosacademicos);

                entity.ToTable("DATOSACADEMICOS", "INV");

                entity.Property(e => e.Iddatosacademicos)
                    .ValueGeneratedNever()
                    .HasColumnName("IDDATOSACADEMICOS");

                entity.Property(e => e.CodUsuario)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO");

                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO")
                    .IsFixedLength();

                entity.Property(e => e.Fechafin)
                    .HasColumnType("date")
                    .HasColumnName("FECHAFIN");

                entity.Property(e => e.Fechainicio)
                    .HasColumnType("date")
                    .HasColumnName("FECHAINICIO");

                entity.Property(e => e.Idcarrera).HasColumnName("IDCARRERA");

                entity.Property(e => e.Idcentroestudio).HasColumnName("IDCENTROESTUDIO");

                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.CodUsuarioNavigation)
                    .WithMany(p => p.Datosacademicos)
                    .HasForeignKey(d => d.CodUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DATOSACADEMICOS_USUARIO");
            });

            modelBuilder.Entity<Datoslaborale>(entity =>
            {
                entity.HasKey(e => e.Iddatoslaborales);

                entity.ToTable("DATOSLABORALES", "INV");

                entity.Property(e => e.Iddatoslaborales)
                    .ValueGeneratedNever()
                    .HasColumnName("IDDATOSLABORALES");

                entity.Property(e => e.Antiguedad)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ANTIGUEDAD");

                entity.Property(e => e.Cargo)
                   .HasMaxLength(200)
                   .IsUnicode(false)
                   .HasColumnName("CARGO");

                entity.Property(e => e.CargoIdcargo).HasColumnName("CARGO_IDCARGO");

                entity.Property(e => e.CodUsuario)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO");

                entity.Property(e => e.Direccionlaboral)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DIRECCIONLABORAL");

                entity.Property(e => e.Lugartrabajo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("LUGARTRABAJO");

                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Universidadtrabajo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("UNIVERSIDADTRABAJO");

                entity.HasOne(d => d.CodUsuarioNavigation)
                    .WithMany(p => p.Datoslaborales)
                    .HasForeignKey(d => d.CodUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DATOSLABORALES_USUARIO");
            });
            modelBuilder.Entity<Centroestudio>(entity =>
            {
                entity.HasKey(e => e.Idcentroestudio);

                entity.ToTable("CENTROESTUDIO", "INV");

                entity.Property(e => e.Idcentroestudio)
                    .ValueGeneratedNever()
                    .HasColumnName("IDCENTROESTUDIO");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<Forodebate>(entity =>
            {
                entity.HasKey(e => e.Idforodebate);

                entity.ToTable("FORODEBATE", "INV");

                entity.Property(e => e.Idforodebate)
                    .ValueGeneratedNever()
                    .HasColumnName("IDFORODEBATE");

                entity.Property(e => e.CodUsuario)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO");

                entity.Property(e => e.Descripcion)
                    .HasColumnType("text")
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.Estado)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO");

                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Titulo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("TITULO");

                entity.HasOne(d => d.CodUsuarioNavigation)
                    .WithMany(p => p.Forodebates)
                    .HasForeignKey(d => d.CodUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FORODEBATE_USUARIO");
            });



            modelBuilder.HasSequence<decimal>("CODIGO_CLIENTE", "INV")
                .StartsAt(21)
                .HasMin(1)
                .HasMax(9223372036854775807);
            modelBuilder.HasSequence<decimal>("COMPRAS", "INV")
                .IncrementsBy(-46642)
                .HasMin(0)
                .HasMax(9223372036854775807);
            modelBuilder.HasSequence<decimal>("COMPRAS1", "INV")
                .HasMin(1)
                .HasMax(9223372036854775807);
            modelBuilder.HasSequence<decimal>("NAMED_SQL_GROUP_SEQ", "INV")
                .HasMin(1)
                .HasMax(9223372036854775807);
            modelBuilder.HasSequence<decimal>("NAMED_SQL_SEQ", "INV")
                .HasMin(1)
                .HasMax(9223372036854775807);
            modelBuilder.HasSequence<decimal>("PAGOS", "INV")
                .HasMin(1)
                .HasMax(9223372036854775807);
            modelBuilder.HasSequence<decimal>("PLSQL_PROFILER_RUNNUMBER", "INV")
                .StartsAt(2)
                .HasMin(1)
                .HasMax(9223372036854775807);
            modelBuilder.HasSequence<decimal>("SEQ_PERSONAS", "INV")
                .StartsAt(3451)
                .HasMin(1)
                .HasMax(9999999999)
                .IsCyclic();
            OnModelCreatingPartial(modelBuilder);
        }
       
partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        public DbSet<SistemaBase.ModelsCustom.BSUSUARI> BSUSUARI { get; set; }
        public DbSet<SistemaBase.ModelsCustom.BSUSUCUS> BSUSUCUS { get; set; }
        public DbSet<SistemaBase.Models.RmAsigdise>? RmAsigdise { get; set; }
    }
}
