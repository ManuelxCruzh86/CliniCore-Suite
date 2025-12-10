using CliniCore.Core.Entities;
using CliniCore.Infrastructure.Data;
using CliniCore.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CliniCore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AppointmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Appointments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
        {
            // Trae los datos relacionados
            return await _context.Appointments
                                 .Include(a => a.Patient) // Carga datos del Paciente
                                 .Include(a => a.Doctor)  // Carga datos del Doctor
                                 .ToListAsync();
        }

        // GET: api/Appointments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointment(int id)
        {
            var appointment = await _context.Appointments
                                            .Include(a => a.Patient)
                                            .Include(a => a.Doctor)
                                            .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            return appointment;
        }

        // POST: api/Appointments
        [HttpPost]
        public async Task<ActionResult<Appointment>> PostAppointment(CreateAppointmentDto dto)
        {
            //  Validar que el Paciente exista
            var pacienteExiste = await _context.Patients.AnyAsync(p => p.Id == dto.PatientId);
            if (!pacienteExiste)
            {
                return BadRequest($"El Paciente con ID {dto.PatientId} no existe.");
            }

            // Validar que el Doctor exista
            var doctorExiste = await _context.Doctors.AnyAsync(d => d.Id == dto.DoctorId);
            if (!doctorExiste)
            {
                return BadRequest($"El Doctor con ID {dto.DoctorId} no existe.");
            }

            // Crear la Cita
            var appointment = new Appointment
            {
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                ScheduledDate = dto.ScheduledDate,
                Reason = dto.Reason,
                Status = "Scheduled",
                CreatedAt = DateTime.UtcNow
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAppointment", new { id = appointment.Id }, appointment);
        }

        // PUT: api/Appointments/5 (Reagendar o cambiar estatus)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointment(int id, UpdateAppointmentDto dto)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            appointment.ScheduledDate = dto.ScheduledDate;
            appointment.Reason = dto.Reason;
            appointment.Status = dto.Status;

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}