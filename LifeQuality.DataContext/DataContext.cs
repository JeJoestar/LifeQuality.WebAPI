using LifeQuality.DataContext.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace LifeQuality.DataContext
{
    public class DataContext : DbContext, IDataContext
    {
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Patron> Patrons { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<BloodAnalysisData> BloodAnalysisData { get; set; }
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {

        }

        public Task<int> DefaultEFSaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CholesterolBloodAnalysisData>().ToTable("BloodAnalysisData");
            builder.Entity<GeneralBloodAnalysisData>().ToTable("BloodAnalysisData");
            builder.Entity<SugarBloodAnalysisData>().ToTable("BloodAnalysisData");
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}