using CliniCore.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CliniCore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SeedController : ControllerBase
    {
        private readonly DataSeederService _seeder;

        public SeedController(DataSeederService seeder)
        {
            _seeder = seeder;
        }

        [HttpPost]
        public async Task<IActionResult> SeedData()
        {
            await _seeder.SeedAsync();
            return Ok("¡Base de datos Inyectada con éxito! Se crearon Doctores, Pacientes y Citas.");
        }
    }
}