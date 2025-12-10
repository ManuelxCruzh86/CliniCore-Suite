using CliniCore.API.DTOs;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPacientes()
        {
            // Solo trae los que IsActive sea true
            return await _context.Patients
                                 .Where(p => p.IsActive == true)
                                 .ToListAsync();
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
        public async Task<ActionResult<Patient>> PostPaciente(CreatePatientDto pacienteDto)
        {
            // (Convertir el DTO seguro a la Entidad real)  pasamos los datos al sistema real
            var nuevoPaciente = new Patient
            {
                FirstName = pacienteDto.FirstName,
                LastName = pacienteDto.LastName,
                BirthDate = pacienteDto.BirthDate,
                Gender = pacienteDto.Gender,
                // Email = pacienteDto.Email, 

                CreatedAt = DateTime.UtcNow, // Nosotros ponemos la fecha exacta
                IsActive = true              // Nace activo por defecto
            };

            // GUARDAR
            _context.Patients.Add(nuevoPaciente);
            await _context.SaveChangesAsync();

            // Retornamos el paciente creado
            return CreatedAtAction("GetPaciente", new { id = nuevoPaciente.Id }, nuevoPaciente);
        }

        // PUT: api/Pacientes/5
        // ACTUALIZAR un paciente existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaciente(int id, UpdatePatientDto pacienteDto)
        {
            // Buscar al paciente en la BD
            var pacienteExistente = await _context.Patients.FindAsync(id);

            if (pacienteExistente == null)
            {
                return NotFound(); // Retorna 404 si no existe
            }

            // Actualizar SOLO los campos permitidos 
            pacienteExistente.FirstName = pacienteDto.FirstName;
            pacienteExistente.LastName = pacienteDto.LastName;
            pacienteExistente.Gender = pacienteDto.Gender;
            pacienteExistente.IsActive = pacienteDto.IsActive;

            // Guardar los cambios
            await _context.SaveChangesAsync();

            return NoContent(); // Retorna 204 (Todo bien)
        }

        // DELETE: api/Pacientes/5
        // BORRAR un paciente
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaciente(int id)
        {
            var paciente = await _context.Patients.FindAsync(id);
            if (paciente == null)
            {
                return NotFound();
            }

            // Borrado Físico (Desaparece de la BD para siempre)
            //_context.Patients.Remove(paciente);

            // Opción B: Borrado Lógico (Solo lo desactivamos)
            paciente.IsActive = false; 

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}