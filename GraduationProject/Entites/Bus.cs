using System.Net.Sockets;

namespace GraduationProject.Entites
{
    public class Bus
    {
        public int Id { get; set; }

        public string BusNumber { get; set; }
        public string PlateNumber { get; set; }

        public int Capacity { get; set; }

        public double CurrentLatitude { get; set; }
        public double CurrentLongitude { get; set; }

        public int RouteId { get; set; }
        public Route Route { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
        public ICollection<BusLocation> BusLocations { get; set; }
    }
}
