using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AiTimanMVC.Models
{
    public class BookedViewModel
    {
        public string? Id { get; set; }

        // Reference to AppointmentViewModel and UsersViewModel
        [Required(ErrorMessage = "Appointment details are required.")]
        public AppointmentViewModel? AppointmentModel { get; set; }

        [Required(ErrorMessage = "User details are required.")]
        public UsersViewModel? UsersModel { get; set; }

        // Time slot selection based on the appointment's available time slots
        [DisplayName("Available Time Slots")]
        public List<TimeSlotViewModel> TimeSlots { get; set; }

        // Constructor initializing appointment and user models
        public BookedViewModel()
        {
            AppointmentModel = new AppointmentViewModel();
            UsersModel = new UsersViewModel();
            TimeSlots = new List<TimeSlotViewModel>();
        }

        public class TimeSlotViewModel
        {
            public TimeRangeViewModel TimeRange { get; set; }
            public int BookingCount { get; set; }

            public string FormattedTimeRange => $"{TimeRange.StartTime:h\\:mm tt} - {TimeRange.EndTime:h\\:mm tt}";
        }

        public class TimeRangeViewModel
        {
            public TimeSpan StartTime { get; set; }
            public TimeSpan EndTime { get; set; }
        }

        // Properties derived from AppointmentViewModel
        [Required(ErrorMessage = "Appointment Name is required.")]
        [DisplayName("Appointment Name")]
        public string? AppointmentName => AppointmentModel?.AppointmentName;

        [Required(ErrorMessage = "Appointment Schedule Date is required.")]
        [DisplayName("Appointment Schedule Date")]
        public DateTime? AppointmentScheduleDate => AppointmentModel?.ScheduleDate;

        [Required(ErrorMessage = "Appointment Schedule Time is required.")]
        [DisplayName("Appointment Schedule Time")]
        public string? AppointmentScheduleTime => AppointmentModel?.TimeSlots.ToString();

        [Required(ErrorMessage = "Doctor in charge is required.")]
        [DisplayName("Doctor In Charge")]
        public string? AppointmentDoctorInCharge => AppointmentModel?.DoctorInCharge;

        // Properties derived from UsersViewModel
        [DisplayName("Patient Name")]
        public string? PatientName => UsersModel != null
            ? $"{UsersModel.FirstName} {UsersModel.LastName}"
            : null;

        [Required(ErrorMessage = "Address is required.")]
        [DisplayName("Address")]
        public string? Address => UsersModel?.Address;

        [Required(ErrorMessage = "Birthdate is required.")]
        [DisplayName("Birthdate")]
        [DataType(DataType.Date)]
        public string? Birthdate => UsersModel?.Birthdate.ToString("yyyy-MM-dd");

        [Required(ErrorMessage = "Age is required.")]
        [DisplayName("Age")]
        public string? Age => UsersModel?.Age.ToString();

        [Required(ErrorMessage = "Gender is required.")]
        [DisplayName("Gender")]
        public string? Gender => UsersModel?.Gender;

        [DisplayName("Guardian Name")]
        public string? GuardianName => UsersModel?.GuardianName;

        // Booking-specific properties
        [Required(ErrorMessage = "Approved By is required.")]
        [DisplayName("Approved By")]
        public string? ApprovedBy { get; set; }

        [Required(ErrorMessage = "Booking Status is required.")]
        [DisplayName("Booking Status")]
        public string? BookingStatus { get; set; } = "Pending";

        [DisplayName("Booking Approved Date")]
        [DataType(DataType.Date)]
        public DateTime? BookingApprovedDate { get; set; }

        // Timestamps for record creation and updates
        [DisplayName("Date Created")]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        [DisplayName("Date Updated")]
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;
    }
}
