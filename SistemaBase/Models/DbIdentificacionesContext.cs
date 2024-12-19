using Microsoft.EntityFrameworkCore;

namespace SistemaBase.Models
{
    public class DbIdentificacionesContext : DbContext
    {
        public DbIdentificacionesContext()
        {
        }
        public DbIdentificacionesContext(DbContextOptions<DbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ConsultaPorCedula>()
                .HasNoKey();

            modelBuilder.Entity<ConsultaPorRUC>()
               .HasNoKey();// Indicar que la entidad no tiene una clave primaria
        }

        public DbSet<ConsultaPorCedula> ConsultaPorCedula { get; set; }
        public DbSet<ConsultaPorRUC> ConsultaPorRUC { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder
                    .UseSqlServer("Server=172.30.8.44,1433; Database=identificaciones;Persist Security Info=False;User ID=Conexion_Auxiliares_MyS;Password=Auxiliares_MyS;Encrypt=False;Trusted_Connection=False; MultipleActiveResultSets=true;")
                .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
            }
        }

    }
    // MODELO del resultado que devuleve el procedimiento almacenado
    public class ConsultaPorCedula
    {
        public Int32? NumeroCedula { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public DateTime? FechaNacimiento { get; set; }
   
    }
    public class ConsultaPorRUC
    {
        public Int32? codigo_ruc { get; set; }
        public string? NroRuc { get; set; }
        public string? nombre { get; set; }

    }
}
