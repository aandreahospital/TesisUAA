using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SistemaBase.Models
{
    public partial class UAADbContext : DbContext
    {
        public UAADbContext()
        {
        }

        public UAADbContext(DbContextOptions<UAADbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccesosGrupo> AccesosGrupos { get; set; } = null!;
        public virtual DbSet<Cargo> Cargos { get; set; } = null!;
        public virtual DbSet<Carrera> Carreras { get; set; } = null!;
        public virtual DbSet<CarreraUsuario> CarreraUsuarios { get; set; } = null!;
        public virtual DbSet<CentroEstudio> CentroEstudios { get; set; } = null!;
        public virtual DbSet<Comentario> Comentarios { get; set; } = null!;
        public virtual DbSet<DatosAcademico> DatosAcademicos { get; set; } = null!;
        public virtual DbSet<DatosLaborale> DatosLaborales { get; set; } = null!;
        public virtual DbSet<Forma> Formas { get; set; } = null!;
        public virtual DbSet<ForoDebate> ForoDebates { get; set; } = null!;
        public virtual DbSet<GruposUsuario> GruposUsuarios { get; set; } = null!;
        public virtual DbSet<Modulo> Modulos { get; set; } = null!;
        public virtual DbSet<OfertaAcademica> OfertaAcademicas { get; set; } = null!;
        public virtual DbSet<OfertaLaboral> OfertaLaborals { get; set; } = null!;
        public virtual DbSet<Persona> Personas { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-KP48E0B\\SQLEXPRESS; Integrated Security=SSPI; Initial Catalog=UAAConecta;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccesosGrupo>(entity =>
            {
                entity.HasKey(e => new { e.CodGrupo, e.CodModulo, e.NomForma });

                entity.ToTable("ACCESOS_GRUPOS");

                entity.Property(e => e.CodGrupo)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("COD_GRUPO");

                entity.Property(e => e.CodModulo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("COD_MODULO");

                entity.Property(e => e.NomForma)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NOM_FORMA");

                //entity.HasOne(d => d.Forma)
                //    .WithMany(p => p.AccesosGrupos)
                //    .HasForeignKey(d => new { d.CodModulo, d.NomForma })
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_AG_FORMAS");
            });

            modelBuilder.Entity<Cargo>(entity =>
            {
                entity.HasKey(e => e.IdCargo);

                entity.ToTable("CARGO");

                entity.Property(e => e.IdCargo).HasColumnName("ID_CARGO");

                entity.Property(e => e.CodUsuario)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                //entity.HasOne(d => d.CodUsuarioNavigation)
                //    .WithMany(p => p.Cargos)
                //    .HasForeignKey(d => d.CodUsuario)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_CARGO_USUARIO");
            });

            modelBuilder.Entity<Carrera>(entity =>
            {
                entity.HasKey(e => e.IdCarrera);

                entity.ToTable("CARRERA");

                entity.Property(e => e.IdCarrera).HasColumnName("ID_CARRERA");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
            });

            modelBuilder.Entity<CarreraUsuario>(entity =>
            {
                entity.HasKey(e => new { e.IdCarrera, e.CodUsuario });

                entity.ToTable("CARRERA_USUARIO");

                entity.Property(e => e.IdCarrera)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_CARRERA");

                entity.Property(e => e.CodUsuario)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO");

                entity.Property(e => e.Promo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PROMO");

                //entity.HasOne(d => d.CodUsuarioNavigation)
                //    .WithMany(p => p.CarreraUsuarios)
                //    .HasForeignKey(d => d.CodUsuario)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_CU_USUARIOS");
            });

            modelBuilder.Entity<CentroEstudio>(entity =>
            {
                entity.HasKey(e => e.IdCentroEstudio);

                entity.ToTable("CENTRO_ESTUDIO");

                entity.Property(e => e.IdCentroEstudio).HasColumnName("ID_CENTRO_ESTUDIO");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
            });

            modelBuilder.Entity<Comentario>(entity =>
            {
                entity.HasKey(e => e.IdComentario);

                entity.ToTable("COMENTARIO");

                entity.Property(e => e.IdComentario).HasColumnName("ID_COMENTARIO");

                entity.Property(e => e.CodUsuario)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO");

                entity.Property(e => e.Descripcion)
                    .HasColumnType("NVARCHAR(MAX)")
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.FechaComentario)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_COMENTARIO");

                entity.Property(e => e.IdForoDebate).HasColumnName("ID_FORO_DEBATE");

                //entity.HasOne(d => d.CodUsuarioNavigation)
                //    .WithMany(p => p.Comentarios)
                //    .HasForeignKey(d => d.CodUsuario)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_COMENTARIO_USUARIO");

                //entity.HasOne(d => d.IdForoDebateNavigation)
                //    .WithMany(p => p.Comentarios)
                //    .HasForeignKey(d => d.IdForoDebate)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_COMENTARIO_FORO");
            });

            modelBuilder.Entity<DatosAcademico>(entity =>
            {
                entity.HasKey(e => e.IdDatosAcademicos);

                entity.ToTable("DATOS_ACADEMICOS");

                entity.Property(e => e.IdDatosAcademicos).HasColumnName("ID_DATOS_ACADEMICOS");

                entity.Property(e => e.AnhoFin)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("ANHO_FIN");

                entity.Property(e => e.AnhoInicio)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("ANHO_INICIO");

                entity.Property(e => e.CodUsuario)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO");

                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO");

                entity.Property(e => e.IdCarrera).HasColumnName("ID_CARRERA");

                entity.Property(e => e.IdCentroEstudio).HasColumnName("ID_CENTRO_ESTUDIO");

                entity.HasOne(d => d.CodUsuarioNavigation)
                    .WithMany(p => p.DatosAcademicos)
                    .HasForeignKey(d => d.CodUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DA_USUARIOS");

                entity.HasOne(d => d.IdCarreraNavigation)
                    .WithMany(p => p.DatosAcademicos)
                    .HasForeignKey(d => d.IdCarrera)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DA_CARRERA");

                //entity.HasOne(d => d.IdCentroEstudioNavigation)
                //    .WithMany(p => p.DatosAcademicos)
                //    .HasForeignKey(d => d.IdCentroEstudio)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_DA_CENTRO_ESTUDIOS");
            });

            modelBuilder.Entity<DatosLaborale>(entity =>
            {
                entity.HasKey(e => e.IdDatosLaborales);

                entity.ToTable("DATOS_LABORALES");

                entity.Property(e => e.IdDatosLaborales).HasColumnName("ID_DATOS_LABORALES");

                entity.Property(e => e.Antiguedad).HasColumnName("ANTIGUEDAD");

                entity.Property(e => e.Cargo).HasColumnName("CARGO");

                entity.Property(e => e.Herramientas).HasColumnName("HERRAMIENTAS");

                entity.Property(e => e.Sector).HasColumnName("SECTOR");

                entity.Property(e => e.UniversidadTrabajo).HasColumnName("UNIVERSIDAD_TRABAJO");

                entity.Property(e => e.Estado).HasColumnName("ESTADO");

                entity.Property(e => e.MateriaTrabajo).HasColumnName("MATERIA_TRABAJO");

                entity.Property(e => e.CodUsuario)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO");

                entity.Property(e => e.LugarTrabajo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("LUGAR_TRABAJO");

                entity.HasOne(d => d.CodUsuarioNavigation)
                    .WithMany(p => p.DatosLaborales)
                    .HasForeignKey(d => d.CodUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DL_USUARIOS");
            });

            modelBuilder.Entity<Forma>(entity =>
            {
                entity.HasKey(e => new { e.CodModulo, e.NomForma });

                entity.ToTable("FORMAS");

                entity.Property(e => e.CodModulo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("COD_MODULO");

                entity.Property(e => e.NomForma)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NOM_FORMA");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                //entity.HasOne(d => d.CodModuloNavigation)
                //    .WithMany(p => p.Formas)
                //    .HasForeignKey(d => d.CodModulo)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_FORMAS_MODULO");
            });

            modelBuilder.Entity<ForoDebate>(entity =>
            {
                entity.HasKey(e => e.IdForoDebate);

                entity.ToTable("FORO_DEBATE");

                entity.Property(e => e.IdForoDebate).HasColumnName("ID_FORO_DEBATE");

                entity.Property(e => e.Adjunto)
                    .IsUnicode(false)
                    .HasColumnName("ARCHIVO");

                entity.Property(e => e.CodUsuario)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.Estado)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO");

                entity.Property(e => e.Titulo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("TITULO");

                //entity.HasOne(d => d.CodUsuarioNavigation)
                //    .WithMany(p => p.ForoDebates)
                //    .HasForeignKey(d => d.CodUsuario)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_FD_USUARIO");
            });

            modelBuilder.Entity<GruposUsuario>(entity =>
            {
                entity.HasKey(e => e.CodGrupo);

                entity.ToTable("GRUPOS_USUARIOS");

                entity.Property(e => e.CodGrupo)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("COD_GRUPO");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
            });

            modelBuilder.Entity<Modulo>(entity =>
            {
                entity.HasKey(e => e.CodModulo);

                entity.ToTable("MODULOS");

                entity.Property(e => e.CodModulo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("COD_MODULO");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");
            });

            modelBuilder.Entity<OfertaAcademica>(entity =>
            {
                entity.HasKey(e => e.IdOfertaAcademica);

                entity.ToTable("OFERTA_ACADEMICA");

                entity.Property(e => e.IdOfertaAcademica).HasColumnName("ID_OFERTA_ACADEMICA");

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

                entity.Property(e => e.FechaCierre)
                    .HasColumnType("DATETIME")
                    .HasColumnName("FECHA_CIERRE");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("DATETIME")
                    .HasColumnName("FECHA_CREACION");

                entity.Property(e => e.Titulo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("TITULO");
                entity.Property(e => e.Adjunto)
                   .HasColumnName("ARCHIVO");

                //entity.HasOne(d => d.CodUsuarioNavigation)
                //    .WithMany(p => p.OfertaAcademicas)
                //    .HasForeignKey(d => d.CodUsuario)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_OA_USUARIOS");
            });

            modelBuilder.Entity<OfertaLaboral>(entity =>
            {
                entity.HasKey(e => e.IdOfertaLaboral);

                entity.ToTable("OFERTA_LABORAL");

                entity.Property(e => e.IdOfertaLaboral).HasColumnName("ID_OFERTA_LABORAL");

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

                entity.Property(e => e.FechaCierre)
                    .HasColumnType("DATETIME")
                    .HasColumnName("FECHA_CIERRE");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("DATETIME")
                    .HasColumnName("FECHA_CREACION");

                entity.Property(e => e.Adjunto)
                 .HasColumnName("ARCHIVO");

                entity.Property(e => e.Titulo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("TITULO");

                //entity.HasOne(d => d.CodUsuarioNavigation)
                //    .WithMany(p => p.OfertaLaborals)
                //    .HasForeignKey(d => d.CodUsuario)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_OL_USUARIOS");
            });

            modelBuilder.Entity<Persona>(entity =>
            {
                entity.HasKey(e => e.CodPersona);

                entity.ToTable("PERSONAS");

                entity.Property(e => e.CodPersona)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("COD_PERSONA");

                entity.Property(e => e.DireccionParticular)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DIRECCION_PARTICULAR");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.EstadoCivil)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO_CIVIL");

                entity.Property(e => e.FecActualizacion)
                    .HasPrecision(0)
                    .HasColumnName("FEC_ACTUALIZACION");

                entity.Property(e => e.FecAlta)
                    .HasPrecision(0)
                    .HasColumnName("FEC_ALTA");

                entity.Property(e => e.FecNacimiento)
                    .HasPrecision(0)
                    .HasColumnName("FEC_NACIMIENTO");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");

                entity.Property(e => e.Sexo)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SEXO");

                entity.Property(e => e.SitioWeb)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("SITIO_WEB");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.CodUsuario);

                entity.ToTable("USUARIOS");

                entity.Property(e => e.CodUsuario)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COD_USUARIO");

                entity.Property(e => e.Clave)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE");

                entity.Property(e => e.CodGrupo)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("COD_GRUPO");

                entity.Property(e => e.CodPersona)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("COD_PERSONA");

                entity.Property(e => e.FecCreacion)
                    .HasColumnType("DATETIME")
                    .HasColumnName("FEC_CREACION");

                entity.HasOne(d => d.CodGrupoNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.CodGrupo)
                    .HasConstraintName("FK_USUARIOS_GRUPO");

                entity.HasOne(d => d.CodPersonaNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.CodPersona)
                    .HasConstraintName("FK_US_PERSONAS");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
