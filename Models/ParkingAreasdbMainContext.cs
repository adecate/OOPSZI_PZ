using Microsoft.EntityFrameworkCore;

namespace Web_Service.Models
{
    public class ParkingAreasdbMainContext : DbContext
    {
        public ParkingAreasdbMainContext()
        {
        }

        public DbSet<ParkingAreaInfo> ParkingAreas { get; set; }

        public ParkingAreasdbMainContext(DbContextOptions<ParkingAreasdbMainContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();           
        }
    }
}