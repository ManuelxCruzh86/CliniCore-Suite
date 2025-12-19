namespace CliniCore.Client.DTOs
{
    public class DoctorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Specialty { get; set; } = "";
        public string LicenseNumber { get; set; } = "";
        public bool IsActive { get; set; }
    }
}