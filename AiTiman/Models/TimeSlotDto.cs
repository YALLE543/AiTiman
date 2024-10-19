namespace AiTiman_API.Models
{
    public class TimeSlotDto
    {
        public string ScheduleTime { get; set; }  // This should match the JSON key exactly
        public int BookingCount { get; set; }
    }
}
