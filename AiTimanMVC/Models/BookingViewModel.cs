using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static AiTimanMVC.Models.AppointmentViewModel;

namespace AiTimanMVC.Models
{
    public class BookingViewModel
    {
        public string? Id { get; set; }


        [Required]
        [DisplayName("Appointment Name")]
        public string? AppointmentName { get; set; }



        [DisplayName("Appointment Schedule Date")]
        public string? AppointmentScheduleDate { get; set; }


        [Required]
        [DisplayName("Appointment Schedule Time")]
        public string? AppointmentScheduleTime { get; set; }


        [Required]
        [DisplayName("Doctor In Charge")]
        public string? AppointmentDoctorInCharge { get; set; }

      
       [Required]
        [DisplayName("Patient Name")]
        public string? PatientName { get; set; }


        [Required]
        [DisplayName("Address")]
        public string? Address { get; set; }


        [Required]
        [DisplayName("Birthdate")]
        public string? Birthdate { get; set; }


        [Required]
        [DisplayName("Age")]
        public string? Age { get; set; }


        [Required]
        [DisplayName("Gender")]
        public string? Gender { get; set; }


        [Required]
        [DisplayName("Guardian Name")]
        public string? GuardianName { get; set; }


      
       [Required]
       [DisplayName("Approved By")]
        public string? ApprovedBy { get; set; }


        [Required]
        [DisplayName("Booking Status")]
        public string? BookingStatus { get; set; } = "Pending";


        [Required]
        [DisplayName("Booking Approved Date")]
        public string? BookingApprovedDate { get; set; }


        [DisplayName("Date Created")]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;



        [DisplayName("Date Updated")]

        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;

        public class TimeSlotViewModel
        {
            public TimeRangeViewModel TimeRange { get; set; }
            public int BookingCount { get; set; } = 0; // Default booking count is 0

            public string FormattedTimeRange
            {
                get
                {
                    return $"{(TimeRange.StartTime.Hours % 12 == 0 ? 12 : TimeRange.StartTime.Hours % 12)}:{TimeRange.StartTime.Minutes:D2} - " +
                           $"{(TimeRange.EndTime.Hours % 12 == 0 ? 12 : TimeRange.EndTime.Hours % 12)}:{TimeRange.EndTime.Minutes:D2}";
                }
            }
        }



      
        public Dictionary<string, TimeSlotViewModel> GenerateTimeSlots()
        {
            var slots = new Dictionary<string, TimeSlotViewModel>();
            DateTime startTime = DateTime.Today.AddHours(9); // 9 AM
            DateTime endTime = DateTime.Today.AddHours(17);  // 5 PM


            while (startTime < endTime)
            {
                slots.Add(startTime.ToString("h:mm tt"), new TimeSlotViewModel
                {
                    TimeRange = new TimeRangeViewModel
                    {
                        StartTime = startTime.TimeOfDay,
                        EndTime = startTime.AddMinutes(60).TimeOfDay
                    },
                    BookingCount = 0 // Initialize with 0 bookings
                });
                startTime = startTime.AddMinutes(60);
            }
            return slots;
        }

       
        public class TimeRangeConverter : JsonConverter
        {
          
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                if (value is TimeRangeViewModel timeRange)
                {
                    DateTime startDateTime = DateTime.Today.Add(timeRange.StartTime);
                    DateTime endDateTime = DateTime.Today.Add(timeRange.EndTime);


                   string timeRangeString = $"{startDateTime.ToString("h:mm tt")} - {endDateTime.ToString("h:mm tt")}";
                   writer.WriteValue(timeRangeString);
                }
                else
                {
                    throw new JsonSerializationException("Expected TimeRangeViewModel object value.");
                }
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                string timeRangeString = (string)reader.Value;
                Console.WriteLine($"Parsing Time Range: {timeRangeString}"); // Log the input for debugging

                var timeParts = timeRangeString.Split(" - ");

                if (timeParts.Length == 2)
                {
                    return new TimeRangeViewModel
                    {
                        StartTime = DateTime.Parse(timeParts[0].Trim()).TimeOfDay,
                        EndTime = DateTime.Parse(timeParts[1].Trim()).TimeOfDay
                    };
                }

                throw new JsonSerializationException("Invalid time range format.");
            }

            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(TimeRangeViewModel);
            }
        }
    }
}
