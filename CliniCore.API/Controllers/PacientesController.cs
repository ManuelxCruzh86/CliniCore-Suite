using CliniCore.Core.Entities;
using CliniCore.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CliniCore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacientesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PacientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Pacientes

        [HttpGet] //-- Atributos
        public async Task<ActionResult<IEnumerable<Patient>>> GetPacientes()
        {
            // Busca en la tabla "Patients" definida en el DbContext
            return await _context.Patients.ToListAsync();
        }

        // GET: api/Pacientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPaciente(int id)
        {
            var paciente = await _context.Patients.FindAsync(id);

            if (paciente == null)
            {
                return NotFound();
            }

            return paciente;
        }

        // POST: api/Pacientes
        [HttpPost] 
        public async Task<ActionResult<Patient>> PostPaciente(Patient paciente)
        {
            _context.Patients.Add(paciente);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPaciente", new { id = paciente.Id }, paciente);
        }
    }
}