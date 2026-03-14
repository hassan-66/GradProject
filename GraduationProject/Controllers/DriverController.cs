using GraduationProject.Data;
using GraduationProject.Dtos;
using GraduationProject.Entites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace GraduationProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriverController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DriverController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🔐 Password Hash
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        // ======================================================
        // 🔑 LOGIN
        // ======================================================

        [HttpPost("login")]
        public IActionResult DriverLogin(LoginDto input)
        {
            var passwordHash = HashPassword(input.Password);

            var driver = _context.Drivers
                .FirstOrDefault(d => d.Email == input.Email &&
                                     d.PasswordHash == passwordHash);

            if (driver == null)
                return Unauthorized("Invalid email or password");

            return Ok(new
            {
                message = "Login successful",
                driverId = driver.Id
            });
        }

        // ======================================================
        // 📋 GET ALL DRIVERS
        // ======================================================

        [HttpGet]
        public IActionResult GetAllDrivers()
        {
            var drivers = _context.Drivers
                .Include(d => d.Bus)
                .Select(d => new
                {
                    d.Id,
                    d.Name,
                    d.Phone,
                    d.Email,
                    d.LicenseNumber,
                    d.Status,
                    Bus = d.Bus == null ? null : new
                    {
                        d.Bus.Id,
                        d.Bus.BusNumber
                    }
                })
                .ToList();

            return Ok(drivers);
        }

        // ======================================================
        // 🔍 GET DRIVER BY ID
        // ======================================================

        [HttpGet("{id}")]
        public IActionResult GetDriverById(int id)
        {
            var driver = _context.Drivers
                .Include(d => d.Bus)
                .FirstOrDefault(d => d.Id == id);

            if (driver == null)
                return NotFound("Driver not found");

            return Ok(new
            {
                driver.Id,
                driver.Name,
                driver.Phone,
                driver.Email,
                driver.LicenseNumber,
                driver.Status,
                Bus = driver.Bus == null ? null : new
                {
                    driver.Bus.Id,
                    driver.Bus.BusNumber
                }
            });
        }

        // ======================================================
        // ➕ CREATE DRIVER
        // ======================================================

        [HttpPost]
        public IActionResult CreateDriver(CreateDriverDto input)
        {
            if (_context.Drivers.Any(d =>
                d.Email == input.Email ||
                d.Phone == input.PhoneNumber))
            {
                return BadRequest("Email or Phone already exists");
            }

            var driver = new Driver
            {
                Name = input.FullName,
                Phone = input.PhoneNumber,
                Email = input.Email,
                LicenseNumber = input.LicenseNumber,
                BusId = input.BusId,
                PasswordHash = HashPassword(input.Password),
                Status = "Inactive"
            };

            _context.Drivers.Add(driver);
            _context.SaveChanges();

            return Ok(new { message = "Driver created successfully" });
        }

        // ======================================================
        // ✏ UPDATE DRIVER
        // ======================================================

        [HttpPut("{id}")]
        public IActionResult UpdateDriver(int id, UpdateDriverDto input)
        {
            var driver = _context.Drivers.Find(id);

            if (driver == null)
                return NotFound("Driver not found");

            driver.Name = input.FullName;
            driver.Phone = input.PhoneNumber;
            driver.Email = input.Email;

            _context.SaveChanges();

            return Ok(new { message = "Driver updated successfully" });
        }

        // ======================================================
        // ❌ DELETE DRIVER
        // ======================================================

        [HttpDelete("{id}")]
        public IActionResult DeleteDriver(int id)
        {
            var driver = _context.Drivers.Find(id);

            if (driver == null)
                return NotFound("Driver not found");

            if (driver.BusId != null)
                return BadRequest("Driver is assigned to a bus");

            _context.Drivers.Remove(driver);
            _context.SaveChanges();

            return Ok(new { message = "Driver deleted successfully" });
        }

        // ======================================================
        // 🟢 CHANGE DRIVER STATUS
        // ======================================================

        [HttpPut("status/{id}")]
        public IActionResult ChangeDriverStatus(int id, [FromQuery] string status)
        {
            var driver = _context.Drivers.Find(id);

            if (driver == null)
                return NotFound("Driver not found");

            driver.Status = status;

            _context.SaveChanges();

            return Ok(new { message = "Driver status updated" });
        }

        // ======================================================
        // 🔄 ASSIGN DRIVER TO BUS
        // ======================================================

        [HttpPost("assign")]
        public IActionResult AssignDriverToBus(int driverId, int busId)
        {
            var driver = _context.Drivers.Find(driverId);
            var bus = _context.Buses.Find(busId);

            if (driver == null || bus == null)
                return NotFound("Driver or Bus not found");

            if (driver.Status != "Active")
                return BadRequest("Driver is not active");

            if (_context.Drivers.Any(d => d.BusId == busId))
                return BadRequest("Bus already has a driver");

            driver.BusId = busId;

            _context.SaveChanges();

            return Ok(new { message = "Driver assigned to bus successfully" });
        }

        // ======================================================
        // 🔓 UNASSIGN DRIVER
        // ======================================================

        [HttpPost("unassign/{driverId}")]
        public IActionResult UnassignDriver(int driverId)
        {
            var driver = _context.Drivers.Find(driverId);

            if (driver == null)
                return NotFound("Driver not found");

            driver.BusId = null;

            _context.SaveChanges();

            return Ok(new { message = "Driver unassigned from bus" });
        }

        // ======================================================
        // 🔑 RESET PASSWORD
        // ======================================================

        [HttpPut("reset-password/{id}")]
        public IActionResult ResetPassword(int id, [FromBody] string newPassword)
        {
            var driver = _context.Drivers.Find(id);

            if (driver == null)
                return NotFound("Driver not found");

            driver.PasswordHash = HashPassword(newPassword);

            _context.SaveChanges();

            return Ok(new { message = "Password reset successfully" });
        }

        // ======================================================
        // 🔎 SEARCH DRIVERS
        // ======================================================

        [HttpGet("search")]
        public IActionResult SearchDrivers([FromQuery] string name)
        {
            var drivers = _context.Drivers
                .Where(d => d.Name.Contains(name))
                .Select(d => new
                {
                    d.Id,
                    d.Name,
                    d.Phone,
                    d.Email
                })
                .ToList();

            return Ok(drivers);
        }

        // ======================================================
        // 👤 DRIVER PROFILE (FOR MOBILE APP)
        // ======================================================

        [HttpGet("profile/{id}")]
        public IActionResult GetDriverProfile(int id)
        {
            var driver = _context.Drivers
                .Include(d => d.Bus)
                .FirstOrDefault(d => d.Id == id);

            if (driver == null)
                return NotFound();

            return Ok(new
            {
                driver.Id,
                driver.Name,
                driver.Phone,
                driver.Email,
                driver.Status,
                BusNumber = driver.Bus?.BusNumber
            });
        }
    }
}