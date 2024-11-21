using AiTiman_API.Models;
using AiTiman_API.Services.DTO;
using AiTiman_API.Services.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AiTiman_API.Services.Respositories
{
    public class PrescriptionRepository : IPrescription
    {
        private readonly IMongoCollection<Prescription> _prescriptionCollection;
        public PrescriptionRepository(IOptions<AiTimanDatabaseSettings> aiTimanDatabaseSettings)
        {
            if (aiTimanDatabaseSettings.Value.ConnectionString == null)
            {
                throw new ArgumentNullException("ConnectionString is null");
            }

            var mongoclient = new MongoClient(aiTimanDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoclient.GetDatabase(aiTimanDatabaseSettings.Value.DatabaseName);

            _prescriptionCollection = mongoDatabase.GetCollection<Prescription>(aiTimanDatabaseSettings.Value.PrescriptionCollectionName);
        }

        public async Task<(bool, string)> AddNewPrescription(CreatePrescriptionDTO createPrescription)
        {
            // Validate PatientName
            if (string.IsNullOrWhiteSpace(createPrescription.PatientName))
            {
                return (false, "Prescription name is required.");
            }
            var newPrescription = new Prescription
            {
                PatientName = createPrescription.PatientName,
                PatientAddress = createPrescription.PatientAddress,
                PatientAge = createPrescription.PatientAge,
                PresMed = createPrescription.PresMed,
                PresDose = createPrescription.PresDose,
                PresInst = createPrescription.PresInst,
                ProviderName = createPrescription.ProviderName,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            };
            await _prescriptionCollection.InsertOneAsync(newPrescription);
            return (true, "New prescription created.");
        }


        public async Task<(bool, string)>DeletePrescription(string? id)
        {
            // Validate the ID
            if (string.IsNullOrWhiteSpace(id))
            {
                return (false, "Prescription ID is required.");
            }

            // Check if the Prescription  exists
            var prescription = await _prescriptionCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (prescription == null)
            {
                return (false, "Prescription not found.");
            }

            // Attempt to delete the Prescription 
            var result = await _prescriptionCollection.DeleteOneAsync(x => x.Id == id);

            // Check if the delete operation was successful
            if (result.DeletedCount > 0)
            {
                return (true, "Prescription deleted successfully.");
            }
            else
            {
                return (false, "Failed to delete the Prescription .");
            }
        }


        public async Task<Prescription?>fetchPrescription(string? id)
        {
            // Validate the ID
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;  // Return null if the ID is invalid
            }

            // Find the Prescription by ID and return the first match or null if not found
            return await _prescriptionCollection.Find(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Prescription>>fetchPrescriptions() =>
            await _prescriptionCollection.Find(_ => true).ToListAsync();

        public async Task<(bool, string)>UpdatePrescription(string id, UpdatePrescriptionDTO updatePrescription)
        {
            // Validate the ID
            if (string.IsNullOrWhiteSpace(id))
            {
                return (false, "Prescription ID is required.");
            }

            // Find the existing Prescription by ID
            var existingPrescription = await _prescriptionCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (existingPrescription == null)
            {
                return (false, "Prescription not found.");
            }

            // Update the relevant fields if they are provided
            var updateDefinition = Builders<Prescription>.Update
                .Set(x => x.DateUpdated, DateTime.UtcNow); // Always update the `DateUpdated`

            if (!string.IsNullOrWhiteSpace(updatePrescription.PatientName))
            {
                updateDefinition = updateDefinition.Set(x => x.PatientName, updatePrescription.PatientName);
            }

            if (!string.IsNullOrWhiteSpace(updatePrescription.PatientAge))
            {
                updateDefinition = updateDefinition.Set(x => x.PatientAge, updatePrescription.PatientAge);
            }

            if (!string.IsNullOrWhiteSpace(updatePrescription.PresMed))
            {
                updateDefinition = updateDefinition.Set(x => x.PresMed, updatePrescription.PresMed);
            }

            if (!string.IsNullOrWhiteSpace(updatePrescription.PresDose))
            {
                updateDefinition = updateDefinition.Set(x => x.PresDose, updatePrescription.PresDose);
            }

            if (!string.IsNullOrWhiteSpace(updatePrescription.PresInst))
            {
                updateDefinition = updateDefinition.Set(x => x.PresInst, updatePrescription.PresInst);
            }
            if (!string.IsNullOrWhiteSpace(updatePrescription.ProviderName))
            {
                updateDefinition = updateDefinition.Set(x => x.ProviderName, updatePrescription.ProviderName);
            }
            // Update the Prescription in the database
            var result = await _prescriptionCollection.UpdateOneAsync(x => x.Id == id, updateDefinition);

            // Check if the update was successful
            if (result.ModifiedCount > 0)
            {
                return (true, "Prescription updated successfully.");
            }
            else
            {
                return (false, "Failed to update the Prescription.");
            }
        }

    }
}


