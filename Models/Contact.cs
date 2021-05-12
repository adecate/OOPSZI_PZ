

namespace Web_Service.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string BalanceholderPhone { get; set; }

        public System.Collections.Generic.List<ParkingAreaInfo> ParkingAreaInfo { get; set; } = new System.Collections.Generic.List<ParkingAreaInfo>();
    }
}