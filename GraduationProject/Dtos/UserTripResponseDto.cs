namespace GraduationProject.Dtos
{
    public class UserTripResponseDto
    {
        public string BusNumber { get; set; }

        public double DistanceToStationKm { get; set; }

        public double TripDistanceKm { get; set; }

        public string EstimatedArrivalTime { get; set; }
    }
}
