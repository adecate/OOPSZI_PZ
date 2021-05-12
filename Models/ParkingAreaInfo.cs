using System.Collections.Generic;

namespace Web_Service.Models
{
    public class ParkingAreaInfo
    {
        public int Id { get; set; }

        public string CommonName { get; set; }

        public string Location { get; set; }

        public List<Contact> BalanceholderPhone { get; set; } = new List<Contact>();
        
        public string BalanceholderWebSite { get; set; }

        public string NeighbourhoodPark { get; set; }

        public string HasWater { get; set; }

        public string HasPlayground { get; set; }

        public string HasSportground { get; set; }

        public string OperationOrganizationName { get; set; }

        public List<WorkingHours> WorkingHours { get; set; } = new List<WorkingHours>();

        public string DepartamentalAffiliationComp { get; set; }
    }
}