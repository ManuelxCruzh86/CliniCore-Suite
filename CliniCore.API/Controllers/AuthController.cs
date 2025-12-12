using CliniCore.Core.Entities;
using CliniCore.Infrastructure.Data;
using CliniCore.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CliniCore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration; // Lee el appsettings/secrets

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // POST: api/Auth/Register
        // Para crear usuarios (Solo el Admin)
        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register(RegisterDto request)
        {
            // Validar si ya existe
            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
            {
                return BadRequest("El usuario ya existe.");
            }

            var user = new User
            {
                Username = request.Username,
                // En producción aquí usaríamos BCrypt para hashear el password.
                // Ahorita, lo guardamos directo (Solo para dev).
                Password = request.Password,
                Role = request.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        // POST: api/Auth/Login
        // intercambiar Credenciales por Token
        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(LoginDto request)
        {
            // Buscar usuario
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

            // Verificar password
            if (user == null || user.Password != request.Password)
            {
                return Unauthorized("Credenciales incorrectas, Padre Santo.");
            }

            // Generar el Token
            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        // Método privado para fabricar el token
        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings.GetValue<string>("Key")!);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("id", user.Id.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                // El token expira en 1 día
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = jwtSettings.GetValue<string>("Issuer"),
                Audience = jwtSettings.GetValue<string>("Audience"),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}