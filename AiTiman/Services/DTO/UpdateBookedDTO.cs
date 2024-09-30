namespace AiTiman_API.Services.DTO
{
    public class UpdateBookedDTO
    {
        public string? AppointmentName { get; set; }
        public DateTime? AppointmentScheduleDate { get; set; }
        public string? AppointmentScheduleTime { get; set; }
        public string? AppointmentDoctorInCharge { get; set; }
        public string? PatientName { get; set; }

        public string? Address { get; set; }

        public string? Birthdate { get; set; }
        public string? Age { get; set; }

        public string? ApprovedBy { get; set; }

        public string? BookingStatus { get; set; }

        public string? GuardianName { get; set; }

        public string? BookingApprovedDate { get; set; }



    }
}
