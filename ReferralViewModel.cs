using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel;

namespace AiTimanMVC.Models
{
    public class ReferralViewModel
    {
        public string? Id { get; set; }

        [DisplayName("Patient Name")]
        public string? PatientName { get; set; }

        [DisplayName("Address")]
        public string? Address { get; set; }

        [DisplayName("Patient Age")]
        public string? Age { get; set; }

        [DisplayName("Diagnosis")]
        public string? Diagnosis { get; set; }

        [DisplayName("Details")]
        public string? RefferTo { get; set; }

        [DisplayName("Doctor In Charge")]
        public string? DoctorInCharge { get; set; }

        [DisplayName("Date Created")]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        [DisplayName("Date Updated")]
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;
    }
}
