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
        public DateTime ScheduleDate { get; set; } = DateTime.UtcNow.ToLocalTime();

        [Required]
        [DisplayName("Number of Slots")]
        public int NumberOfSlots { get; set; }

        [Required]
        [DisplayName("Appointment Status")]
        public string? AppointmentStatus { get; set; } = "Open for Booking";

        [Required]
        [DisplayName("Doctor In Charge")]
        public string? DoctorInCharge { get; set; }

        [DisplayName("Appointment Setter")]
        public string? AppointmentSetter { get; set; }

        public List<DateTime>? AppointmentDates { get; set; }

        // Use Newtonsoft.Json.JsonConverter
        [JsonConverter(typeof(TimeRangeConverter))]
        public TimeRangeViewModel ScheduleTime { get; set; }

        // Updated TimeSlots to store both time range and booking count
        public Dictionary<string, TimeSlotViewModel> TimeSlots { get; set; } = new Dictionary<string, TimeSlotViewModel>();



        [Required]
        [DisplayName("Date Created")]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        [DisplayName("Date Updated")]
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;

        public AppointmentViewModel()
        {
            TimeSlots = GenerateTimeSlots();
        }

        public class TimeRangeViewModel
        {
            public TimeSpan StartTime { get; set; }
            public TimeSpan EndTime { get; set; }

            public string FormattedTime
            {
                get
                {
                    // Format the StartTime and EndTime in 12-hour format without leading zeros
                    return $"{(StartTime.Hours % 12 == 0 ? 12 : StartTime.Hours % 12)}:{StartTime.Minutes:D2} - " +
                           $"{(EndTime.Hours % 12 == 0 ? 12 : EndTime.Hours % 12)}:{EndTime.Minutes:D2}";
                }
            }
        }

        // New class to store both time range and booking count
        public class TimeSlotViewModel
        {
            public TimeRangeViewModel TimeRange { get; set; }
            public int BookingCount { get; set; } = 0; // Default booking count is 0

            public string FormattedTimeRange
            {
                get
                {
                    // Format the StartTime and EndTime in 12-hour format without leading zeros
                    return $"{(TimeRange.StartTime.Hours % 12 == 0 ? 12 : TimeRange.StartTime.Hours % 12)}:{TimeRange.StartTime.Minutes:D2} - " +
                           $"{(TimeRange.EndTime.Hours % 12 == 0 ? 12 : TimeRange.EndTime.Hours % 12)}:{TimeRange.EndTime.Minutes:D2}";
                }
            }
        }



        // Updated to return TimeSlotViewModel for each slot
        public Dictionary<string, TimeSlotViewModel> GenerateTimeSlots()
        {
            var slots = new Dictionary<string, TimeSlotViewModel>();
            DateTime startTime = DateTime.Today.AddHours(9); // 9 AM
            DateTime endTime = DateTime.Today.AddHours(17);  // 5 PM
            startTime = startTime.AddMinutes(60);
            
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

        // This is the TimeRangeConverter for Newtonsoft.Json
        public class TimeRangeConverter : JsonConverter
        {
            // This method is called to write the JSON representation of the object
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                if (value is TimeRangeViewModel timeRange)
                {
                    // Convert TimeSpan to DateTime before formatting to handle AM/PM
                    DateTime startDateTime = DateTime.Today.Add(timeRange.StartTime);
                    DateTime endDateTime = DateTime.Today.Add(timeRange.EndTime);

                    // Format the TimeRange as a string without leading zeros for hours
                    string timeRangeString = $"{startDateTime.ToString("h:mm tt")} - {endDateTime.ToString("h:mm tt")}";
                    writer.WriteValue(timeRangeString);
                }
                else
                {
                    throw new JsonSerializationException("Expected TimeRangeViewModel object value.");
                }
            }

            // This method is called to read the JSON representation of the object
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                // Read the time range string from the JSON, e.g., "10:00 AM - 3:00 PM"
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

            // This method determines whether the converter can convert the specified type
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(TimeRangeViewModel);
            }
        }

    }
}