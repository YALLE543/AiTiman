using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static AiTimanMVC.Models.AppointmentViewModel;

namespace AiTimanMVC.Models
{
    public class BookedViewModel
    {
        // Reference to AppointmentViewModel and UsersViewModel
        public AppointmentViewModel AppointmentModel { get; set; }
        public UsersViewModel UsersModel { get; set; }

        

        public string? Id { get; set; }

        public BookedViewModel()
        {
            AppointmentModel = new AppointmentViewModel();
            UsersModel = new UsersViewModel();
        }
        // These fields are populated from AppointmentViewModel
        [Required]
        [DisplayName("Appointment Name")]
        public string? AppointmentName => AppointmentModel?.AppointmentName;

        [DisplayName("Appointment Schedule Date")]
        public DateTime? AppointmentScheduleDate => AppointmentModel?.ScheduleDate;

        [DisplayName("Appointment Schedule Time")]
        public string? AppointmentScheduleTime => AppointmentModel?.ScheduleTime?.FormattedTime;

        [DisplayName("Doctor In Charge")]
        public string? AppointmentDoctorInCharge => AppointmentModel?.DoctorInCharge;

        // These fields are populated from UsersViewModel
        [DisplayName("Patient Name")]
        public string? PatientName => $"{UsersModel?.FirstName} {(string.IsNullOrWhiteSpace(UsersModel?.MiddleName) ? "" : UsersModel.MiddleName + " ")}{UsersModel?.LastName}".Trim();

        [DisplayName("Address")]
        public string? Address => UsersModel?.Address;

        [DisplayName("Birthdate")]
        public string? Birthdate => UsersModel?.Birthdate.ToString("yyyy-MM-dd");

        [DisplayName("Age")]
        public string? Age => UsersModel?.Age.ToString();

        [DisplayName("Gender")]
        public string? Gender => UsersModel?.Gender;

        [DisplayName("Guardian Name")]
        public string? GuardianName => UsersModel?.GuardianName;

        // Other properties specific to the booking process
        [DisplayName("Approved By")]
        public string? ApprovedBy { get; set; }

        [DisplayName("Booking Status")]
        public string? BookingStatus { get; set; }

        [DisplayName("Booking Approved Date")]
        public string? BookingApprovedDate { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        [DisplayName("Date Updated")]
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;


    }
}