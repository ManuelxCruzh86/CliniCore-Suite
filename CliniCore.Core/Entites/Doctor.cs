namespace CliniCore.Core.Entities
{
    public class Doctor
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        // (Cardiología, Pediatría,etc)
        public string Specialty { get; set; } = string.Empty;

        public string LicenseNumber { get; set; } = string.Empty; // Cédula profesional

        // Auditoría
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}