using CliniCore.Core.Entities;
using CliniCore.Infrastructure.Data;
using CliniCore.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CliniCore.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    [Authorize]
    public class DoctorsController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public DoctorsController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: api/Doctors
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctors()
		{
			return await _context.Doctors
								 .Where(d => d.IsActive == true)
								 .ToListAsync();
		}

		// GET: api/Doctors/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Doctor>> GetDoctor(int id)
		{
			var doctor = await _context.Doctors.FindAsync(id);

			if (doctor == null)
			{
				return NotFound();
			}

			return doctor;
		}

		// POST: api/Doctors
		[HttpPost]
		public async Task<ActionResult<Doctor>> PostDoctor(CreateDoctorDto doctorDto)
		{
			var doctor = new Doctor
			{
				FirstName = doctorDto.FirstName,
				LastName = doctorDto.LastName,
				Specialty = doctorDto.Specialty,
				LicenseNumber = doctorDto.LicenseNumber,

				// Datos automáticos
				CreatedAt = DateTime.UtcNow,
				IsActive = true
			};

			_context.Doctors.Add(doctor);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetDoctor", new { id = doctor.Id }, doctor);
		}

		// PUT: api/Doctors/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutDoctor(int id, UpdateDoctorDto doctorDto)
		{
			var doctor = await _context.Doctors.FindAsync(id);

			if (doctor == null)
			{
				return NotFound();
			}

			doctor.FirstName = doctorDto.FirstName;
			doctor.LastName = doctorDto.LastName;
			doctor.Specialty = doctorDto.Specialty;
			doctor.LicenseNumber = doctorDto.LicenseNumber;
			doctor.IsActive = doctorDto.IsActive;

			await _context.SaveChangesAsync();

			return NoContent();
		}

		// DELETE: api/Doctors/5 
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteDoctor(int id)
		{
			var doctor = await _context.Doctors.FindAsync(id);
			if (doctor == null)
			{
				return NotFound();
			}

			// Borrado lógico: Solo lo apagamos
			doctor.IsActive = false;

			await _context.SaveChangesAsync();

			return NoContent();
		}
	}
}