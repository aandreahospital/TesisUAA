using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SistemaBase.Models
{
    public partial class DbvinDbContext : DbContext
    {
        public DbvinDbContext()
        {
        }

        public DbvinDbContext(DbContextOptions<DbvinDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccesosGrupo> AccesosGrupos { get; set; } = null!;
        public virtual DbSet<Cargo> Cargos { get; set; } = null!;
        public virtual DbSet<Carrera> Carreras { get; set; } = null!;
        public virtual DbSet<CarreraUsuario> CarreraUsuarios { get; set; } = null!;
        public virtual DbSet<Centroestudio> Centroestudios { get; set; } = null!;
        public virtual DbSet<Comentario> Comentarios { get; set; } = null!;
        public virtual DbSet<Datosacademico> Datosacademicos { get; set; } = null!;
        public virtual DbSet<Datoslaborale> Datoslaborales { get; set; } = null!;
        public virtual DbSet<Empresa> Empresas { get; set; } = null!;
        public virtual DbSet<Forma> Formas { get; set; } = null!;
        public virtual DbSet<Forodebate> Forodebates { get; set; } = null!;
        public virtual DbSet<GruposUsuario> GruposUsuarios { get; set; } = null!;
        public virtual DbSet<Modulo> Modulos { get; set; } = null!;
        public virtual DbSet<Ofertaacademica> Ofertaacademicas { get; set; } = null!;
        public virtual DbSet<Ofertalaboral> Ofertalaborals { get; set; } = null!;
        public virtual DbSet<PermisosOpcione> PermisosOpciones { get; set; } = null!;
        public virtual DbSet<Persona> Personas { get; set; } = null!;
        public virtual DbSet<RmUsuario> RmUsuarios { get; set; } = null!;
        public virtual DbSet<Sucursale> Sucursales { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-KP48E0B\\SQLEXPRESS; Integrated Security=SSPI; Initial Catalog=UAAconecta;");
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
                    .HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.CodGrupoNavigation)
                    .WithMany(p => p.AccesosGrupos)
                    .HasForeignKey(d => d.CodGrupo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ACC_GRUPOS_GRP");

                //entity.HasOne(d => d.CodModuloNavigation)
                //    .WithMany(p => p.AccesosGrupos)
                //    .HasForeignKey(d => d.CodModulo)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_ACC_GRU_MOD");
            });

            modelBuilder.Entity<Cargo>(entity =>
            {
                entity.HasKey(e => e.Idcargo);

                entity.ToTable("CARGO", "INV");

                entity.Property(e => e.Idcargo)
                    .ValueGeneratedNever()
                    .HasColumnName("IDCARGO");

                entity.Property(e => e.CodUsuario)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO");

                entity.Property(e => e.Lugartrabajo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("LUGARTRABAJO");

                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.CodUsuarioNavigation)
                    .WithMany(p => p.Cargos)
                    .HasForeignKey(d => d.CodUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CARGO_USUARIO");
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
                entity.HasKey(e => new { e.Idcarrera, e.CodUsuario });

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

                entity.HasOne(d => d.ForodebateIdforodebateNavigation)
                    .WithMany(p => p.Comentarios)
                    .HasForeignKey(d => d.ForodebateIdforodebate)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_COMENTARIO_FORODEBATE");
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
                    .HasMaxLength(255)
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

                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO");

                entity.Property(e => e.Herramientas)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("HERRAMIENTAS");

                entity.Property(e => e.Lugartrabajo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("LUGARTRABAJO");

                entity.Property(e => e.Materia)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("MATERIA");

                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Sector)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("SECTOR");

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
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Titulo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TITULO");

                //entity.HasOne(d => d.CodModuloNavigation)
                //    .WithMany(p => p.Formas)
                //    .HasForeignKey(d => d.CodModulo)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_FORMAS_MOD");
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
                    .HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<Ofertaacademica>(entity =>
            {
                entity.HasKey(e => e.Idofertaacademica);

                entity.ToTable("OFERTAACADEMICA", "INV");

                entity.Property(e => e.Idofertaacademica)
                    .ValueGeneratedNever()
                    .HasColumnName("IDOFERTAACADEMICA");

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

                entity.Property(e => e.Fechacierre)
                    .HasColumnType("date")
                    .HasColumnName("FECHACIERRE");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("date")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Titulo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("TITULO");

                entity.HasOne(d => d.CodUsuarioNavigation)
                    .WithMany(p => p.Ofertaacademicas)
                    .HasForeignKey(d => d.CodUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OFERTAACADEMICA_USUARIO");
            });

            modelBuilder.Entity<Ofertalaboral>(entity =>
            {
                entity.HasKey(e => e.Idofertalaboral);

                entity.ToTable("OFERTALABORAL", "INV");

                entity.Property(e => e.Idofertalaboral)
                    .ValueGeneratedNever()
                    .HasColumnName("IDOFERTALABORAL");

                entity.Property(e => e.CodUsuario)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO");

                entity.Property(e => e.Contacto)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("CONTACTO");

                entity.Property(e => e.Descripcion)
                    .HasColumnType("text")
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.Estado)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO");

                entity.Property(e => e.Fechacierre)
                    .HasColumnType("date")
                    .HasColumnName("FECHACIERRE");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("date")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.CodUsuarioNavigation)
                    .WithMany(p => p.Ofertalaborals)
                    .HasForeignKey(d => d.CodUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OFERTALABORAL_USUARIO");
            });

            modelBuilder.Entity<PermisosOpcione>(entity =>
            {
                entity.HasKey(e => new { e.CodEmpresa, e.CodUsuario, e.CodModulo, e.Parametro, e.NomForma });

                entity.ToTable("PERMISOS_OPCIONES", "INV");

                entity.HasIndex(e => e.Rowid, "ROWID_INDEX")
                    .IsUnique();

                entity.Property(e => e.CodEmpresa)
                    .HasMaxLength(5)
                    .HasColumnName("COD_EMPRESA");

                entity.Property(e => e.CodUsuario)
                    .HasMaxLength(20)
                    .HasColumnName("COD_USUARIO");

                entity.Property(e => e.CodModulo)
                    .HasMaxLength(5)
                    .HasColumnName("COD_MODULO");

                entity.Property(e => e.Parametro)
                    .HasMaxLength(50)
                    .HasColumnName("PARAMETRO");

                entity.Property(e => e.NomForma)
                    .HasMaxLength(10)
                    .HasColumnName("NOM_FORMA");

                entity.Property(e => e.Permiso)
                    .HasMaxLength(1)
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

                entity.Property(e => e.CodSector)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("COD_SECTOR");

                entity.Property(e => e.Direccionparticular)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DIRECCIONPARTICULAR");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.FecActualizacion)
                    .HasPrecision(0)
                    .HasColumnName("FEC_ACTUALIZACION");

                entity.Property(e => e.FecAlta)
                    .HasPrecision(0)
                    .HasColumnName("FEC_ALTA");

                entity.Property(e => e.FecNacimiento)
                    .HasPrecision(0)
                    .HasColumnName("FEC_NACIMIENTO");

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

                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Sexo)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SEXO");

                entity.Property(e => e.Sitioweb)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("SITIOWEB");
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

                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO");

                entity.Property(e => e.NroPatronal)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NRO_PATRONAL");

                entity.Property(e => e.PlazoEnvio)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("PLAZO_ENVIO");

                entity.Property(e => e.Rowid)
                    .HasColumnName("ROWID")
                    .HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.CodEmpresaNavigation)
                    .WithMany(p => p.Sucursales)
                    .HasForeignKey(d => d.CodEmpresa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SUCUR_EMP");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.CodUsuario);

                entity.ToTable("USUARIOS", "INV");

                entity.HasIndex(e => e.Rowid, "ROWID$INDEX")
                    .IsUnique();

                entity.Property(e => e.CodUsuario)
                    .HasMaxLength(15)
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
                    .HasDefaultValueSql("(newid())");

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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
