using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SQL_API.Models;
using System.Runtime.InteropServices.ComTypes;
using System.Xml;

namespace SQL_API.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Link> LINKS { get; set; }
        public DbSet<User> USERS { get; set; }
        public DbSet<TypeModel> TYPES { get; set; }
        public DbSet<ChatModel> CHAT { get; set; }
        public DbSet<Notification> NOTIFICATIONS { get; set; }
        public DbSet<NotificationTarget> NOTIFICATIONTARGETS { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Link>().ToTable(tb => tb.HasTrigger("INSERT_EXPIRE_DATE"));
            modelBuilder.Entity<User>().ToTable(tb => { tb.HasTrigger("USERDATA_UPD");tb.HasTrigger("USERDATA_INS"); tb.HasTrigger("USERDATA_DEL"); });
            modelBuilder.Entity<ChatModel>().ToTable(tb => tb.HasTrigger("INSERT_CHAT"));

            modelBuilder.Entity<Notification>().ToTable(tb => tb.HasTrigger("NOTIFICATIONS_INS_DATE"));
            modelBuilder.Entity<NotificationTarget>().ToTable(tb => tb.HasTrigger("NOTIFICATIONTARGET_UPD_DATE"));

        }
    }
}
