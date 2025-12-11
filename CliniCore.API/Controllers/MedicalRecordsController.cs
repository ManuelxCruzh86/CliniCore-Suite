using CliniCore.Core.Entities;
using CliniCore.Infrastructure.Data;
using CliniCore.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CliniCore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalRecordsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MedicalRecordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/MedicalRecords/5
        // Obtener un expediente por su ID
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicalRecord>> GetMedicalRecord(int id)
        {
            var record = await _context.MedicalRecords
                                       .Include(r => r.Appointment) // Traemos datos de la cita
                                       .FirstOrDefaultAsync(r => r.Id == id);

            if (record == null) return NotFound();

            return record;
        }

        // GET: api/MedicalRecords/ByAppointment/10
        // Buscar el expediente usando el ID de la Cita
        [HttpGet("ByAppointment/{appointmentId}")]
        public async Task<ActionResult<MedicalRecord>> GetByAppointment(int appointmentId)
        {
            var record = await _context.MedicalRecords
                                       .FirstOrDefaultAsync(r => r.AppointmentId == appointmentId);

            if (record == null) return NotFound();

            return record;
        }

        // POST: api/MedicalRecords
        // Crear el expediente y CERRAR la cita
        [HttpPost]
        public async Task<ActionResult<MedicalRecord>> PostMedicalRecord(CreateMedicalRecordDto dto)
        {
            // Validar que la Cita exista
            var appointment = await _context.Appointments.FindAsync(dto.AppointmentId);
            if (appointment == null)
            {
                return BadRequest("La cita especificada no existe.");
            }

            // Validar que la cita no tenga ya un expediente 
            var existeExpediente = await _context.MedicalRecords.AnyAsync(r => r.AppointmentId == dto.AppointmentId);
            if (existeExpediente)
            {
                return BadRequest("Esta cita ya tiene un expediente registrado.");
            }

            // Crear el Expediente
            var medicalRecord = new MedicalRecord
            {
                AppointmentId = dto.AppointmentId,
                Symptoms = dto.Symptoms,
                WeightKg = dto.WeightKg,
                HeightCm = dto.HeightCm,
                TemperatureC = dto.TemperatureC,
                Diagnosis = dto.Diagnosis,
                Icd10Code = dto.Icd10Code,
                TreatmentNotes = dto.TreatmentNotes,
                CreatedAt = DateTime.UtcNow
            };

            // Si ya se creó el expediente, significa que la consulta ocurrió.
            // Cambiamos el estado de la cita a "Completed".
            appointment.Status = "Completed";

            // Guardamos AMBOS cambios en una sola transacción
            _context.MedicalRecords.Add(medicalRecord);
            // EF Core ya sabe que modificamos 'appointment', Entonces lo guardará tambien.
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMedicalRecord", new { id = medicalRecord.Id }, medicalRecord);
        }
    }
}