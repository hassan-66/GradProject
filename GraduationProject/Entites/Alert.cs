namespace GraduationProject.Entites
{
    public class Alert
    {
        public int Id { get; set; }

        public int BusId { get; set; }
        public Bus Bus { get; set; }

        public string Type { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
