using Microsoft.EntityFrameworkCore;
using SQL_API.Models;

namespace SQL_API.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Link> LINKS { get; set; }
        public DbSet<User> USERS { get; set; }
        public DbSet<ChatModel> CHAT { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Link>().ToTable(tb => tb.HasTrigger("INSERT_EXPIRE_DATE"));
            modelBuilder.Entity<User>().ToTable(tb => tb.HasTrigger("USERDATA_UPD"));
        }
    }
}
