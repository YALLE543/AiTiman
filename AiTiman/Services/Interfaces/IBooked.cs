using AiTiman_API.Models;
using AiTiman_API.Services.DTO;

namespace AiTiman_API.Services.Interfaces
{
    public interface IBooked
    {
        Task<(bool, string)> AddNewBooked(CreateBookedDTO createBooked);
        Task<List<Booked>> fetchBooked();
        Task<Booked> fetchBooked(string? id);
        Task<(bool, string)> UpdateBooked(string id, UpdateBookedDTO updateBooked);
        Task<(bool, string)> DeleteBooked(string? id);
        Task<List<string>> FetchTimeSlotBookings(DateTime appointmentDate);

    }
}
