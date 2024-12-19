using Microsoft.EntityFrameworkCore;

namespace SistemaBase.Models
{
    public class DbAuxiliaresContext : DbContext
    {
        public DbAuxiliaresContext()
        {
        }
        public DbAuxiliaresContext(DbContextOptions<DbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<vAuxiliares_AJ>()
               .ToTable("vAuxiliares_IngresosWEB")
                .HasNoKey();
            
        }

        public DbSet<vAuxiliares_AJ> vAuxiliares_AJ { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder
                    .UseSqlServer("Server=172.30.8.44,1433; Database=sgjasu;Persist Security Info=False;User ID=Conexion_Auxiliares_MyS;Password=Auxiliares_MyS;Encrypt=False;Trusted_Connection=False; MultipleActiveResultSets=true;")
                .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
            }
        }
    }
    public class vAuxiliares_AJ
    {
        public Int32? tipo_interviniente { get; set; }
        public string? descrip_interviniente { get; set; }
        public Int32? Codigo_persona { get; set; }
        public Int32? matricula { get; set; }
        public string? Habilitado { get; set; }
        public string? RUC { get; set; }
        public DateTime? fecha_expedicion_matricula { get; set; }
        public string? Apellidos { get; set; }
        public string? Nombres { get; set; }
        public string? documento_nro { get; set; }
        public Int32? tipo_documento { get; set; }
        public DateTime? fecha_nacimiento { get; set; }
        public Byte? sexo { get; set; }
        public Int32? origen { get; set; }


    }

}
