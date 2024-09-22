using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TODO_LIST.Models;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using TODO_LIST.Services;

namespace TODO_LIST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TodoListContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(TodoListContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // POST: api/Auth/signup
        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupRequest request)
        {
            if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("All fields are required.");
            }

            if (_context.Users.Any(u => u.Email == request.Email))
            {
                return BadRequest("Email is already in use.");
            }

            // Create new user with email and password
            var newUser = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                Password = request.Password
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "User registered successfully",
                user = new { newUser.Email }
            });
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Email and password are required.");
            }

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.Password == request.Password);

            if (existingUser == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            var token = GenerateToken(existingUser);

            return Ok(new
            {
                message = "Login successful",
                data = new
                {
                    user = new
                    {
                        existingUser.UserName,
                        existingUser.Email,
                        existingUser.UserId

                    },
                    token
                },
                status = 200
            });
        }

        // POST: api/Auth/forgot-password
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return BadRequest("Email is required.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return NotFound("Email address not found.");
            }

            // Generate a reset token and expiry (optional, not needed now)
            var resetLink = $"https://localhost:4200/reset-password?email={request.Email}"; // Generate actual reset link

            // Send email to user (assuming your EmailService is implemented)
            var emailService = new EmailService();
            await emailService.SendPasswordResetEmail(request.Email, resetLink);

            return Ok("Password reset email sent.");
        }

        // POST: api/Auth/reset-password
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.NewPassword) || string.IsNullOrEmpty(request.ConfirmPassword))
            {
                return BadRequest("Email, new password, and confirm password are required.");
            }

            if (request.NewPassword != request.ConfirmPassword)
            {
                return BadRequest("Passwords do not match.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Update user password (consider hashing in real-world use cases)
            user.Password = request.NewPassword;
            await _context.SaveChangesAsync();

            return Ok("Password has been reset.");
        }
        // GET: api/Auth/users
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users
                .Select(u => new
                {
                    u.UserId,
                    u.UserName,
                    u.Email
                })
                .ToListAsync();

            return Ok(users);
        }
        // GET: api/Auth/user/{id}
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.Users
                .Where(u => u.UserId == id)
                .Select(u => new
                {
                    u.UserId,
                    u.UserName,
                    u.Email
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            return Ok(user);
        }
        private string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    public class SignupRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class ForgotPasswordRequest
    {
        public string Email { get; set; }
    }

    public class ResetPasswordRequest
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}