using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace AiTiman_API.Models
{
    public class Booked
    {

       

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]

        public string? Id { get; set; }

        [BsonElement("Appointment Name")]
        public string? AppointmentName { get; set; }

        [BsonElement("Appointment Schedule Date")]
        public DateTime? AppointmentScheduleDate { get; set; }

        [BsonElement("Appointment Schedule Time")]
        public string? AppointmentScheduleTime { get; set; }

        [BsonElement("Doctor In Charge")]
        public string? AppointmentDoctorInCharge { get; set; }

        [BsonElement("Patient Name")]
        public string? PatientName { get; set; }

        [BsonElement("Address")]
        public string? Address { get; set; }

        [BsonElement("Birthdate")]
        public string? Birthdate { get; set; }

        [BsonElement("Age")]
        public string? Age { get; set; }

        [BsonElement("Gender")]
        public string? Gender { get; set; }

        [BsonElement("Approved By")]
        public string? ApprovedBy { get; set; }

        [BsonElement("Booking Status")]
        public string? BookingStatus { get; set; }

        [BsonElement("Guardian Name")]
        public string? GuardianName { get; set; }

        [BsonElement("Booking Approved Date")]
        public DateTime? BookingApprovedDate { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        [BsonElement("Date Updated")]
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;


    }

}

