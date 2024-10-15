using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace AiTiman_API.Models
{
    public class Referral
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Patient Name")]
        public string? PatientName { get; set; }

        [BsonElement("Address")]
        public string? Address { get; set; }

        [BsonElement("Patient Age")]
        public string? Age { get; set; }

        [BsonElement("Diagnosis")]
        public string? Diagnosis { get; set; }

        [BsonElement("Details")]
        public string? RefferTo { get; set; }
        
        [BsonElement("Doctor In Charge")]
        public string?DoctorInCharge { get; set; }

        [BsonElement("Date Created")]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        [BsonElement("Date Updated")]
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;
    }
}
