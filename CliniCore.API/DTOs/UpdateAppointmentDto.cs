namespace CliniCore.API.DTOs
{
    public class UpdateAppointmentDto
    {
        public DateTime ScheduledDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string Status { get; set; } = "Scheduled"; // Scheduled, Completed, Cancelled
    }
}