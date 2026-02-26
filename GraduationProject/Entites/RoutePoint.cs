using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Entites
{
    public class RoutePoint
    {
        public int Id { get; set; }

        public int RouteId { get; set; }
        public Route Route { get; set; }
        [Required]
        [Range(-90, 90)]
        public double Latitude { get; set; }
        [Required]
        [Range(-90, 90)]
        public double Longitude { get; set; }
    }
}
