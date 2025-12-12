using Bogus; // La librería mágica
using CliniCore.Core.Entities;
using CliniCore.Infrastructure.Data;

namespace CliniCore.API.Services
{
    public class DataSeederService
    {
        private readonly ApplicationDbContext _context;

        public DataSeederService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // Si ya hay datos, no hacemos nada 
            //if (_context.Patients.Any()) return;

            //  GENERAR DOCTORES
            var doctorFaker = new Faker<Doctor>()
                .RuleFor(d => d.FirstName, f => f.Name.FirstName())
                .RuleFor(d => d.LastName, f => f.Name.LastName())
                .RuleFor(d => d.Specialty, f => f.PickRandom("Cardiología", "Pediatría", "General", "Neurología"))
                .RuleFor(d => d.LicenseNumber, f => f.Random.Replace("#######"))
                .RuleFor(d => d.IsActive, _ => true);

            var doctores = doctorFaker.Generate(10);
            _context.Doctors.AddRange(doctores);
            await _context.SaveChangesAsync(); // Guardamos para que tengan ID real

            // 2. GENERAR PACIENTES (Creamos 50)
            var patientFaker = new Faker<Patient>()
                .RuleFor(p => p.FirstName, f => f.Name.FirstName())
                .RuleFor(p => p.LastName, f => f.Name.LastName())
                .RuleFor(p => p.BirthDate, f => f.Date.Past(80)) // Hasta 80 años atrás
                .RuleFor(p => p.Gender, f => f.PickRandom("Male", "Female"))
                .RuleFor(p => p.IsActive, _ => true);

            var pacientes = patientFaker.Generate(50);
            _context.Patients.AddRange(pacientes);
            await _context.SaveChangesAsync(); 

            // GENERAR CITAS
            var appointmentFaker = new Faker<Appointment>()
                .RuleFor(a => a.PatientId, f => f.PickRandom(pacientes).Id) // Asigna un paciente al azar
                .RuleFor(a => a.DoctorId, f => f.PickRandom(doctores).Id)   // Asigna un doctor al azar
                .RuleFor(a => a.ScheduledDate, f => f.Date.Future(1))       // Cita en el próximo año
                .RuleFor(a => a.Reason, f => f.Lorem.Sentence())
                .RuleFor(a => a.Status, _ => "Scheduled");

            var citas = appointmentFaker.Generate(100);
            _context.Appointments.AddRange(citas);
            await _context.SaveChangesAsync();
        }
    }
}