namespace CliniCore.Client.DTOs
{
    public class DashboardStatsDto
    {
        public int TotalPatients { get; set; }
        public int TotalDoctors { get; set; }
        public int PendingAppointments { get; set; }
    }
}