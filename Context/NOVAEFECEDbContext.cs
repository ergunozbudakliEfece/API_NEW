using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQL_API.Models;

namespace SQL_API.Context
{
    public class NOVAEFECEDbContext : DbContext
    {
        public DbSet<CustomerModel> TBL_CUSTOMERS { get; set; }
        public DbSet<SectorModel> TBL_SECTORS { get; set; }
        public DbSet<QualificationModel> TBL_QUALIFICATION { get; set; }
        public DbSet<MeetingModel> TBL_CUSTOMERCALENDAR { get; set; }
        public DbSet<ShippingForm> TBL_SHIPPINGFORMS { get; set; }
        public DbSet<AttendanceModel> TBL_ATTENDANCE { get; set; }
        public DbSet<ShiftModel> TBL_SHIFTS { get; set; }
        public DbSet<ShiftDetailModel> TBL_SHIFTDETAILS { get; set; }
        public DbSet<SettedPriceModel> SETTED { get; set; }
        public NOVAEFECEDbContext(DbContextOptions<NOVAEFECEDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerModel>().ToTable(tb => { tb.HasTrigger("CUSTOMERS_UPD"); tb.HasTrigger("CUSTOMERS_INS"); });
            modelBuilder.Entity<MeetingModel>().ToTable(tb => { tb.HasTrigger("CUSTOMERCALENDAR_UPD"); tb.HasTrigger("CUSTOMERCALENDAR_INS"); });
        }
    }
}
