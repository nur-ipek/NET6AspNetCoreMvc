using Microsoft.EntityFrameworkCore;

namespace NET6AspNetCoreMvc.Entities
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
