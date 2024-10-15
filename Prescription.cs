using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel;

namespace AiTiman_API.Models
{
    public class Prescription
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Patient Name")]
        public string? PatientName { get; set; }

        [BsonElement("Patient Age")]
        public string? PatientAge { get; set; }

        [BsonElement("Patient Address")]
        public string? PatientAddress { get; set; }


        [BsonElement("Presciption Medicine")]
        public string? PresMed { get; set; }

        [BsonElement("Presciption Dosage")]
        public string? PresDose { get; set; }


        [BsonElement("Presciption Instructions")]
        public string? PresInst { get; set; }


        [BsonElement("Provider Name")]
        public string? ProviderName { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        [BsonElement("Date Updated")]
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;

    }
}
