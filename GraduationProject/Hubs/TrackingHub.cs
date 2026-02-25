using Microsoft.AspNetCore.SignalR;

namespace GraduationProject.Hubs
{
    public class TrackingHub : Hub
    {
        public async Task SendLocationUpdate(string id, double lat, double lng)
        {
            await Clients.All.SendAsync("ReceiveLocationUpdate", id, lat, lng);
        }
    }
}