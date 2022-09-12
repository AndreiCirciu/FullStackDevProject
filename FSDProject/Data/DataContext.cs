using FSDProject;
using Microsoft.EntityFrameworkCore;

namespace FSDProjectAPI.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Medicine>Medicines { get; set; }
    }
}
