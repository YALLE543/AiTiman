using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace AiTiman_API.Models
{
    public class Appointment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Appointment Name")]
        public string? AppointmentName { get; set; }

        [BsonElement("Schedule Date")]
        public DateTime ScheduleDate { get; set; } = DateTime.UtcNow.ToLocalTime();

        [BsonElement("Schedule Time")]
        public string? ScheduleTime { get; set; }

        [BsonElement("Number of Slots")]
        public int NumberOfSlots { get; set; }

        [BsonElement("Appointment Status")]
        public string? AppointmentStatus { get; set; }

        [BsonElement("Doctor In Charge")]
        public string? DoctorInCharge { get; set; }

        [BsonElement("Appointment Setter")]
        public string? AppointmentSetter { get; set; }

        [BsonElement("Date Created")]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        [BsonElement("Date Updated")]
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;
    }
}
