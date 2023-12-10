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
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {

        }

        public Task<int> DefaultEFSaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return SaveChangesAsync(cancellationToken);
        }
    }
}
