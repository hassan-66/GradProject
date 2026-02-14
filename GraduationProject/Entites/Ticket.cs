namespace GraduationProject.Entites
{
    public class Ticket
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int BusId { get; set; }
        public Bus Bus { get; set; }

        public int RouteId { get; set; }
        public Route Route { get; set; }

        public string QRToken { get; set; }

        public decimal Price { get; set; }
        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
