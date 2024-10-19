using MongoDB.Bson.Serialization.Attributes;

namespace AiTiman_API.Services.DTO
{
    public class CreateAppointmentDTO
    {
        public string? AppointmentName { get; set; }

        public DateTime ScheduleDate { get; set; } = DateTime.UtcNow.ToLocalTime();

        public string? ScheduleTime { get; set; } // Use long to receive ticks

        public string? AppointmentStatus { get; set; } = "Open for Booking";

        public string? AppointmentSetter { get; set; }


        public int? NumberOfSlots { get; set; }

        public string? DoctorInCharge { get; set; }
    }
}
