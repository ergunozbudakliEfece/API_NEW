using Microsoft.EntityFrameworkCore;
using SQL_API.Models;

namespace SQL_API.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Link> LINKS { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Link>().ToTable(tb => tb.HasTrigger("INSERT_EXPIRE_DATE"));
        }
    }
}
