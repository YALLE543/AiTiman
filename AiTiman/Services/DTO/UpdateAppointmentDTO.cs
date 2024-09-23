namespace AiTiman_API.Services.DTO
{
    public class UpdateAppointmentDTO
    {
        public string? AppointmentName { get; set; }

        public DateTime? ScheduleDate { get; set; }

        public string? ScheduleTime { get; set; }

        public string? AppointmentStatus { get; set; }
    }
}
