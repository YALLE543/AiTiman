using AiTiman_API.Models;
using AiTiman_API.Services.DTO;

namespace AiTiman_API.Services.Interfaces
{
    public interface IReferral
    {
        Task<(bool, string)> AddNewReferral(CreateReferralDTO createReferral);
        Task<List<Referral>> fetchReferrals();
        Task<Referral> fetchReferral(string? id);
        Task<(bool, string)> UpdateReferral (string id, UpdateReferralDTO updateReferral);
        Task<(bool, string)> DeleteReferral(string? id);
    }
}
