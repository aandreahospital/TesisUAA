using Microsoft.EntityFrameworkCore;

namespace SistemaBase.Models
{
    public class DbPercepcionesContext : DbContext
    {
        public DbPercepcionesContext()
        {
        }
        public DbPercepcionesContext(DbContextOptions<DbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ObtenerLiquidacionMarcasSenales>()
                .HasNoKey(); // Indicar que la entidad NO tiene una clave primaria
        }

        public DbSet<ObtenerLiquidacionMarcasSenales> ObtenerLiquidacionMarcasSenales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder
                    .UseSqlServer("Server=172.30.8.22,1433; Database=Percepciones;Persist Security Info=False;User ID=Conexion_MyS;Password=Conexion_MyS;Encrypt=False;Trusted_Connection=False; MultipleActiveResultSets=true;")
                .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
            }
        }


    }
    public class ObtenerLiquidacionMarcasSenales
    {
        public Int32? liquidacionId { get; set; }
        public string? codigo { get; set; }
        public Int32? estadoId { get; set; }
        public string? estadoLiquidacion { get; set; }
        public Int32? tipoLiquidacionId { get; set; }
        public string? descripcionTipoLiquidacion { get; set; }
        public double? totalLiquidacion { get; set; }
        public Int32? intervinienteId { get; set; }
        public byte? estaUtilizado { get; set; }
        public string? nombreProfesional { get; set; }
        public string? matriculaProfesional { get; set; }
        public string? tipo { get; set; }
        public string? nroDocumento { get; set; }
        public Int32? hojaSeguridad { get; set; }
        public Int32? conceptoId { get; set; }
        public string? descripcionConcepto { get; set; }
        public byte? esJurisdiccional { get; set; }
        public byte? esRegistral { get; set; }
        public byte? esEspecial { get; set; }
        public string? tipoFiscalizacion { get; set; }
        public Int32? IdTipoFiscalizacion { get; set; }
        public string? nombreSolicitante { get; set; }
        public string? nroDocumentoSolicitante { get; set; }
        public string? tipoDocumentoSolicitante { get; set; }



    }
}
