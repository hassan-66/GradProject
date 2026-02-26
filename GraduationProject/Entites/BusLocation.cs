namespace GraduationProject.Entites
{
    public class BusLocation
    {
        public int Id { get; set; }

        public int BusId { get; set; }
        public Bus Bus { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Speed { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt
        {
            get; set;
        }
    }
}
