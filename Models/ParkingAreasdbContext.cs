using Microsoft.EntityFrameworkCore;
using Web_Service.Models;

namespace Client_WPF.Models
{
    public partial class ParkingAreasdbContext : DbContext
    {
        public ParkingAreasdbContext()
        {
        }

        public ParkingAreasdbContext(DbContextOptions<ParkingAreasdbContext> options) : base(options)
        {
        }

        public virtual DbSet<ParkingAreaInfo> ParkingAreas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var current_dir = new System.IO.DirectoryInfo(System.IO.Directory.GetCurrentDirectory());
                var application_dir = current_dir.Parent.Parent.Parent.Parent.FullName;
                string db_path = "Data Source=" + application_dir + "\\Web Service" + "\\ParkingAreas.db";

                optionsBuilder.UseSqlite(db_path);
            }
        }

    }
}