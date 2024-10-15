using AiTiman_API.Models;
using AiTiman_API.Services.DTO;

namespace AiTiman_API.Services.Interfaces
{
    public interface IPrescription
    {
        Task<(bool, string)> AddNewPrescription(CreatePrescriptionDTO createPrescription);
        Task<List<Prescription>> fetchPrescriptions();
        Task<Prescription> fetchPrescription(string? id);
        Task<(bool, string)> UpdatePrescription(string id, UpdatePrescriptionDTO updatePrescription);
        Task<(bool, string)> DeletePrescription(string? id);
    }
}
