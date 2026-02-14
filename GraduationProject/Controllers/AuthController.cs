using GraduationProject.Data;
using GraduationProject.Dtos;
using GraduationProject.Entites;
using GraduationProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;
        private readonly GoogleAuthService _googleService;

        public AuthController(ApplicationDbContext context , EmailService emailService, GoogleAuthService googleService)
        {
            _context = context;
            _emailService = emailService;
            _googleService = googleService;
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
        [HttpPost("request-reset")]
        public IActionResult RequestPasswordReset([FromBody] ResetRequestDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);
            if (user == null)
                return BadRequest("Email not found");

            var code = new Random().Next(100000, 999999).ToString();

            var reset = new PasswordResetCode
            {
                Email = dto.Email,
                Code = code,
                ExpirationTime = DateTime.UtcNow.AddMinutes(10)
            };

            _context.PasswordResetCodes.Add(reset);
            _context.SaveChanges();

            _emailService.SendEmail(dto.Email, code);

            return Ok("Reset code sent");
        }
        [HttpPost("confirm-reset")]
        public IActionResult ConfirmReset([FromBody] ConfirmResetDto dto)
        {
            var reset = _context.PasswordResetCodes
                .FirstOrDefault(r => r.Email == dto.Email && r.Code == dto.Code);

            if (reset == null || reset.ExpirationTime < DateTime.UtcNow)
                return BadRequest("Invalid or expired code");

            var user = _context.Users.First(u => u.Email == dto.Email);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            _context.PasswordResetCodes.Remove(reset);
            _context.SaveChanges();

            return Ok("Password updated successfully");
        }


        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDto dto)
        {
            var payload = await _googleService.VerifyToken(dto.IdToken);

            var user = _context.Users.FirstOrDefault(u => u.Email == payload.Email);

            if (user == null)
            {
                user = new User
                {
                    Email = payload.Email,
                    FullName = payload.Name,
                    Balance = 0,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                _context.SaveChanges();
            }

            return Ok(new
            {
                Type = "User",
                user.Id,
                user.Email,
                user.FullName
            });
        }
    }
}
