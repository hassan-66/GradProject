using static System.Collections.Specialized.BitVector32;

namespace GraduationProject.Entites
{
    public class Route
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ZoneId { get; set; }
        public Zone Zone { get; set; }
        public ICollection<Station> Stations { get; set; }
        public ICollection<Bus> Buses { get; set; }
        public ICollection<RoutePoint> RoutePoints { get; set; } // Add this property
    }
}
