using AiTiman_API.Models;
using AiTiman_API.Services.DTO;
using AiTiman_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AiTiman_API.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class Booked : ControllerBase
    {
        private readonly IBooked _booked;

        // Constructor injection of the IAppointment service
        public Booked(IBooked booked)
        {
            _booked = booked;
        }


        [HttpGet("All-Booked")]

        public async Task<IActionResult> AllBooked()
        {
            var booked = await _booked.fetchBooked();
            return Ok(booked);
        }

        [HttpGet("Fetch-Booked")]

        public async Task<IActionResult> fetchBooked(string id)
        {
            var booked = await _booked.fetchBooked(id);
            return Ok(booked);
        }

        [HttpPost("Create-New-Booked")]

        public async Task<IActionResult> CreateNewBooked(CreateBookedDTO createBooking)
        {
            var (isSuccess, message) = await _booked.AddNewBooked(createBooking);

            if (isSuccess)
                return BadRequest(message);

            return Ok(message);
        }

        [HttpPut("Update-Booked")]

        public async Task<IActionResult> UpdateBooked(string id, UpdateBookedDTO updateBooking)
        {
            var (isSuccess, message) = await _booked.UpdateBooked(id, updateBooking);


            if (isSuccess)
                return BadRequest(message);
            return Ok(message);

        }

        [HttpDelete("Delete-Booked")]

        public async Task<IActionResult> DeleteBooked(string id)

        {
            var (isSuccess, message) = await _booked.DeleteBooked(id);

            if (isSuccess)
                return BadRequest(message);
            return Ok(message);

        }
        [HttpGet("GetTimeSlotBookings")]
        public async Task<IActionResult> GetTimeSlotBookings([FromQuery] DateTime appointmentDate)
        {
            var appointmentDates = await _booked.FetchTimeSlotBookings(appointmentDate);
            return Ok(appointmentDates);
        }
    }
}



