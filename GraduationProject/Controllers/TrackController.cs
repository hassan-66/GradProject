using GraduationProject.Dtos;
using GraduationProject.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace GraduationProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrackController : ControllerBase
    {
        private readonly IHubContext<TrackingHub> _hub;

        public TrackController(IHubContext<TrackingHub> hub)
        {
            _hub = hub;
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateLocation([FromBody] TrainLocationDto loc)
        {
            await _hub.Clients.All.SendAsync(
                "ReceiveLocationUpdate",
                loc.Id,
                loc.Lat,
                loc.Lng
            );

            return Ok(new { message = "Location broadcasted successfully" });
        }
    }
}