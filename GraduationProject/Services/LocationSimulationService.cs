using GraduationProject.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace GraduationProject.Services
{
    public class LocationSimulationService : BackgroundService
    {
        private readonly IHubContext<TrackingHub> _hub;

        public LocationSimulationService(IHubContext<TrackingHub> hub)
        {
            _hub = hub;
        }

        private readonly List<(double lat, double lng)> _route = new()
            {
                (30.10617850522894, 31.3273059961349),
                (30.10607459495127, 31.327462199382122),
                (30.107870368037833, 31.32963279331024),
                (30.106207001539808, 31.33144751199704),
                (30.106949381444682, 31.333119943648406)
            };

        private int _currentIndex = 0;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var point = _route[_currentIndex];

                await _hub.Clients.All.SendAsync(
                    "ReceiveLocationUpdate",
                    "Train-1",
                    point.lat,
                    point.lng,
                    cancellationToken: stoppingToken
                );

                _currentIndex++;

                if (_currentIndex >= _route.Count)
                {
                    _currentIndex = 0; // loop again (or remove this line to stop)
                }

                await Task.Delay(1000, stoppingToken); // speed control
            }
        }
    }
}