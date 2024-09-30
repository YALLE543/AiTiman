using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AiTimanMVC.Models
{
    public class BookedViewModel
    {
        public string? Id { get; set; }
        [Required]
        [DisplayName("Appointment Name")]
        public string? AppointmentName { get; set; }

        [DisplayName("Appointment Schedule Date")]
        public DateTime? AppointmentScheduleDate { get; set; }

        [DisplayName("Appointment Schedule Time")]
        public string? AppointmentScheduleTime { get; set; }

        [DisplayName("Doctor In Charge")]
        public string? AppointmentDoctorInCharge { get; set; }

        [DisplayName("Patient Name")]
        public string? PatientName { get; set; }

        [DisplayName("Address")]
        public string? Address { get; set; }

        [DisplayName("Birthdate")]
        public string? Birthdate { get; set; }

        [DisplayName("Age")]
        public string? Age { get; set; }

        [DisplayName("Gender")]
        public string? Gender { get; set; }

        [DisplayName("Approved By")]
        public string? ApprovedBy { get; set; }

        [DisplayName("Booking Status")]
        public string? BookingStatus { get; set; }

        [DisplayName("Guardian Name")]
        public string? GuardianName { get; set; }

        [DisplayName("Booking Approved Date")]
        public string? BookingApprovedDate { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        [DisplayName("Date Updated")]
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;
    }
}
