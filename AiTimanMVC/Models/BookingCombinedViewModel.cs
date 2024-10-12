
namespace AiTimanMVC.Models
{
    public class BookingCombinedViewModel
    {
        public ICollection<AppointmentViewModel> AppointmentList { get; set; } // Changed to AppointmentList for consistency
        public AppointmentViewModel AppointmentModel { get; set; } // Ensure it's initialized
        public BookedViewModel BookedModel { get; set; }
        public UsersViewModel UsersModel { get; set; }
    }
}
