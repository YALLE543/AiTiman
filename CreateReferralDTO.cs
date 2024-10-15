using MongoDB.Bson.Serialization.Attributes;
namespace AiTiman_API.Services.DTO
{
    public class CreateReferralDTO
    {
        public string? Id { get; set; }
        public string? PatientName { get; set; }
        public string? Address { get; set; }
        public string? Age { get; set; }
        public string? Diagnosis { get; set; }

        public string? RefferTo { get; set; }
        public string? DoctorInCharge { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}
