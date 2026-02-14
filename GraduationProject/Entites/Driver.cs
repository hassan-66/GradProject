namespace GraduationProject.Entites
{
    public class Driver
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public bool IsActive { get; set; }

        public int? BusId { get; set; }
        public Bus Bus { get; set; }
    }
}
