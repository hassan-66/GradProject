using GraduationProject.Data;
using GraduationProject.Dtos;
using GraduationProject.Entites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost("user/register")]
        public IActionResult UserRegister([FromBody] RegisterDto dto)
        {
            if (_context.Users.Any(u => u.Email == dto.Email))
                return BadRequest("Email already exists.");

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Phone = dto.Phone,
                Balance = 0,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok("User registered successfully.");
        }



        [HttpPost("driver/register")]
        public IActionResult DriverRegister([FromBody] RegisterDto dto)
        {
            if (_context.Drivers.Any(d => d.Email == dto.Email))
                return BadRequest("Email already exists.");

            var driver = new Driver
            {
                Name = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Drivers.Add(driver);
            _context.SaveChanges();

            return Ok("Driver registered successfully.");
        }



        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);
            if (user != null && BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return Ok(new
                {
                    Type = "User",
                    user.Id,
                    user.FullName,
                    user.Email
                });
            }

            var driver = _context.Drivers.FirstOrDefault(d => d.Email == dto.Email);
            if (driver != null && BCrypt.Net.BCrypt.Verify(dto.Password, driver.PasswordHash))
            {
                return Ok(new
                {
                    Type = "Driver",
                    driver.Id,
                    driver.Name,
                    driver.Email
                });
            }

            return Unauthorized("Invalid email or password");
        }
    }
}
