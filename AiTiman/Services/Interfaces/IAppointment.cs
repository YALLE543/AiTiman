using AiTiman_API.Models;
using AiTiman_API.Services.DTO;

namespace AiTiman_API.Services.Interfaces
{
    public interface IAppointment
    {
        Task<(bool, string)> AddNewAppointment(CreateAppointmentDTO createAppointment);
        Task<List<Appointment>> fetchAppointments();
        Task<Appointment> fetchAppointment(string? id);
        Task<(bool, string)> UpdateAppointment(string id, UpdateAppointmentDTO updateAppointment);
        Task<(bool, string)> DeleteAppointment(string? id);
    }
}
