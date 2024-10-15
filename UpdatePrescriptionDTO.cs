using System.ComponentModel;

namespace AiTiman_API.Services.DTO
{
    public class UpdatePrescriptionDTO
    {
        public string? Id { get; set; }

       public string? PatientName { get; set; }

       
        public string? PatientAge { get; set; }

        
        public string? PatientAddress { get; set; }


      
        public string? PresMed { get; set; }

        public string? PresDose { get; set; }


      
        public string? PresInst { get; set; }

        public string? ProviderName { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;
    }
}
