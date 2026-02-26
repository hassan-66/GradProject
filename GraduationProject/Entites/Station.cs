using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Entites
{
    public class Station
    {
        public int Id { get; set; }

        public string Name { get; set; }
        [Required]
        [Range(-90, 90)]
        public double Latitude { get; set; }
        [Required]
        [Range(-90, 90)]
        public double Longitude { get; set; }

        public int RouteId { get; set; }
        public Route Route { get; set; }
        public int? CreatedByAdmin { get; set; }
        public Admin CreatedStations { get; set; }
    }
}
