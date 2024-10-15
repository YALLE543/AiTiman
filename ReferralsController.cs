using AiTiman_API.Models;
using AiTiman_API.Services.DTO;
using AiTiman_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AiTiman_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReferralsController : Controller
    {
        private readonly IReferral _referral;

        // Constructor injection of the Referral service
        public ReferralsController(IReferral referral)
        {
            _referral = referral;
        }

        [HttpGet("All-Referrals")]
        public async Task<IActionResult> AllReferrals()
        {
            var referrals = await _referral.fetchReferrals();
            return Ok(referrals);
        }

        [HttpGet("Fetch-Referral")]
        public async Task<IActionResult> FetchReferral (string id)
        {
            var referral = await _referral.fetchReferral(id);
            return Ok(referral);
        }

        [HttpPost("Create-New-Referral")]
        public async Task<IActionResult> CreateNewReferral(CreateReferralDTO createReferral)
        {
            var (isSuccess, message) = await _referral.AddNewReferral(createReferral);

            if (!isSuccess)
                return BadRequest(message);

            return Ok(message);
        }

        [HttpPut("Update-Referral")]
        public async Task<IActionResult> UpdateReferral(string id, UpdateReferralDTO updateReferral)
        {
            var (isSuccess, message) = await _referral.UpdateReferral(id, updateReferral);

            if (!isSuccess)
                return BadRequest(message);

            return Ok(message);
        }

        [HttpDelete("Delete-Referral")]
        public async Task<IActionResult> DeleteReferral(string id)
        {
            var (isSuccess, message) = await _referral.DeleteReferral(id);

            if (!isSuccess)
                return BadRequest(message);

            return Ok(message);
        }
    }
}

