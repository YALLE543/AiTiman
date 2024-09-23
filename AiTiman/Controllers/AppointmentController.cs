using AiTiman_API.Services.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AiTiman_API.Services.Interfaces;
using System.Threading.Tasks;

namespace AiTiman_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointment _appointment;

        // Constructor injection of the IAppointment service
        public AppointmentController(IAppointment appointment)
        {
            _appointment = appointment;
        }

        [HttpGet("All-Appointments")]
        public async Task<IActionResult> AllAppointments()
        {
            var appointments = await _appointment.fetchAppointments();
            return Ok(appointments);
        }

        [HttpGet("Fetch-Appointment")]
        public async Task<IActionResult> FetchAppointment(string id)
        {
            var appointment = await _appointment.fetchAppointment(id);
            return Ok(appointment);
        }

        [HttpPost("Create-New-Appointment")]
        public async Task<IActionResult> CreateNewAppointment(CreateAppointmentDTO createAppointment)
        {
            var (isSuccess, message) = await _appointment.AddNewAppointment(createAppointment);

            if (!isSuccess)
                return BadRequest(message);

            return Ok(message);
        }

        [HttpPut("Update-Appointment")]
        public async Task<IActionResult> UpdateAppointment(string id, UpdateAppointmentDTO updateAppointment)
        {
            var (isSuccess, message) = await _appointment.UpdateAppointment(id, updateAppointment);

            if (!isSuccess)
                return BadRequest(message);

            return Ok(message);
        }

        [HttpDelete("Delete-Appointment")]
        public async Task<IActionResult> DeleteAppointment(string id)
        {
            var (isSuccess, message) = await _appointment.DeleteAppointment(id);

            if (!isSuccess)
                return BadRequest(message);

            return Ok(message);
        }
    }
}
