using Microsoft.EntityFrameworkCore;

namespace SistemaBase.Models
{
    public class DbPruebaContext : DbContext
    {
        public DbPruebaContext()
        {
        }
        public DbPruebaContext(DbContextOptions<DbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CorreoElectronicoResult>()
                .HasNoKey(); // Indicar que la entidad no tiene una clave primaria

        }
        // delcarar el dataset con la clase que mapea el resultado que devuelve el procedimiento almacenado
        public DbSet<CorreoElectronicoResult> CorreoElectronicoResult { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder
                    .UseSqlServer("Server=tcp:proecan.database.windows.net,1433; Database=jekakuaapruebas;Persist Security Info=False;User ID=proecan;Password=Guardian.2391;Encrypt=False;Trusted_Connection=False; MultipleActiveResultSets=true;")
                .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
            }
        }

    }
    // Clase que mapea el resultado del procedimiento almacenado, en este caso devuelve un string con el correo.
    public class CorreoElectronicoResult
    {
        public string CorreoElectronico { get; set; }
    }

}
