using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AiTimanMVC.Models
{
    public class PrescriptionViewModel
    {
        
        public string? Id { get; set; }

        [DisplayName("Patient Name")]
        public string? PatientName { get; set; }

        [DisplayName("Patient Age")]
        public string? PatientAge { get; set; }

        [DisplayName("Patient Address")]
        public string? PatientAddress { get; set; }


        [DisplayName("Presciption Medicine")]
        public string? PresMed { get; set; }

        [DisplayName("Presciption Dosage")]
        public string? PresDose { get; set; }


        [DisplayName("Presciption Instructions")]
        public string? PresInst { get; set; }


        [DisplayName("Provider Name")]
        public string? ProviderName { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        [DisplayName("Date Updated")]
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;
    }
}
