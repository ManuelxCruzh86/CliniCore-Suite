namespace CliniCore.Client.DTOs
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string Reason { get; set; } = "";
        public string Status { get; set; } = "";

        public string PatientName { get; set; } = ""; 
                                                      
        public PatientDto? Patient { get; set; }
        public DoctorDto? Doctor { get; set; }
    }

    public class DoctorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Specialty { get; set; } = "";
    }
}