using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQL_API.Models;

namespace SQL_API.Context
{
    
        public class NOVAEFECEDbContext : DbContext
        {
            public DbSet<CustomerModel> TBL_CUSTOMERS { get; set; }
            public NOVAEFECEDbContext(DbContextOptions<NOVAEFECEDbContext> options) : base(options)
            {
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {

            }
        }
    
}
