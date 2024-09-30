using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AiTimanMVC.Models
{
    public class AppointmentViewModel
    {

        
        public string? Id { get; set; }

        [Required]
        [DisplayName("Appointment Name")]
        public string? AppointmentName { get; set; }

        [Required]
        [DisplayName("Schedule Date")]
        public DateTime ScheduleDate { get; set; }

        [Required]
        [DisplayName("Schedule Time")]
        public string? ScheduleTime { get; set; }

        [Required]
        [DisplayName("Number of Slots")]
        public int NumberOfSlots { get; set; }

        [Required]
        [DisplayName("Appointment Status")]
        public string? AppointmentStatus { get; set; } = "Open for Booking";

        [Required]
        [DisplayName("Doctor In Charge")]
        public string? DoctorInCharge { get; set; }

        [Required]
        [DisplayName("Appointment Setter")]
        public string? AppointmentSetter { get; set; }

        [Required]
        [DisplayName("Date Created")]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        [DisplayName("Date Updated")]
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;
    }
}
