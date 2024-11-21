using AiTiman_API.Models;
using AiTiman_API.Services.DTO;
using AiTiman_API.Services.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AiTiman_API.Services.Respositories
{
    public class ReferralRepository : IReferral
    {
        private readonly IMongoCollection<Referral> _referralCollection;
        public ReferralRepository(IOptions<AiTimanDatabaseSettings> aiTimanDatabaseSettings)
        {
            if (aiTimanDatabaseSettings.Value.ConnectionString == null)
            {
                throw new ArgumentNullException("ConnectionString is null");
            }

            var mongoclient = new MongoClient(aiTimanDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoclient.GetDatabase(aiTimanDatabaseSettings.Value.DatabaseName);

            _referralCollection = mongoDatabase.GetCollection<Referral>(aiTimanDatabaseSettings.Value.ReferralCollectionName);
        }

        public async Task<(bool, string)>AddNewReferral(CreateReferralDTO createReferral)
        {
            // Validate PatientName
            if (string.IsNullOrWhiteSpace(createReferral.PatientName))
            {
                return (false, "Referral name is required.");
            }
            var newReferral = new Referral
            {
                PatientName = createReferral.PatientName,
                Address = createReferral.Address,
                Age = createReferral.Age,
                Diagnosis = createReferral.Diagnosis,
                RefferTo = createReferral.RefferTo,
                DoctorInCharge = createReferral.DoctorInCharge,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            };
            await _referralCollection.InsertOneAsync(newReferral);
            return (true, "New Referral created.");
        }


        public async Task<(bool, string)>DeleteReferral(string? id)
        {
            // Validate the ID
            if (string.IsNullOrWhiteSpace(id))
            {
                return (false, "Referral ID is required.");
            }

            // Check if the Referral  exists
            var referral = await _referralCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (referral == null)
            {
                return (false, "Referral  not found.");
            }

            // Attempt to delete the Referral
            var result = await _referralCollection.DeleteOneAsync(x => x.Id == id);

            // Check if the delete operation was successful
            if (result.DeletedCount > 0)
            {
                return (true, "Referral deleted successfully.");
            }
            else
            {
                return (false, "Failed to delete the Referral  .");
            }
        }


        public async Task<Referral>fetchReferral(string? id)
        {
            // Validate the ID
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;  // Return null if the ID is invalid
            }

            // Find the Referral by ID and return the first match or null if not found
            return await _referralCollection.Find(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Referral>>fetchReferrals() =>
            await _referralCollection.Find(_ => true).ToListAsync();

        public async Task<(bool, string)> UpdateReferral(string id, UpdateReferralDTO updateReferral)
        {
            // Validate the ID
            if (string.IsNullOrWhiteSpace(id))
            {
                return (false, "Referral ID is required.");
            }

            // Find the existing Referral by ID
            var existingReferral = await _referralCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (existingReferral == null)
            {
                return (false, "Referral Not found.");
            }
                
            // Update the relevant fields if they are provided
            var updateDefinition = Builders<Referral>.Update
                .Set(x => x.DateUpdated, DateTime.UtcNow); // Always update the `DateUpdated`

            if (!string.IsNullOrWhiteSpace(updateReferral.PatientName))
            {
                updateDefinition = updateDefinition.Set(x => x.PatientName, updateReferral.PatientName);
            }

            if (!string.IsNullOrWhiteSpace(updateReferral.Age))
            {
                updateDefinition = updateDefinition.Set(x => x.Age, updateReferral.Age);
            }

            if (!string.IsNullOrWhiteSpace(updateReferral.Diagnosis))
            {
                updateDefinition = updateDefinition.Set(x => x.Diagnosis, updateReferral.Diagnosis);
            }

            if (!string.IsNullOrWhiteSpace(updateReferral.RefferTo))
            {
                updateDefinition = updateDefinition.Set(x => x.RefferTo, updateReferral.RefferTo);
            }

            if (!string.IsNullOrWhiteSpace(updateReferral.DoctorInCharge))
            {
                updateDefinition = updateDefinition.Set(x => x.DoctorInCharge, updateReferral.DoctorInCharge);
            }

            // Update the Referral in the database
            var result = await _referralCollection.UpdateOneAsync(x => x.Id == id, updateDefinition);

            // Check if the update was successful
            if (result.ModifiedCount > 0)
            {
                return (true, "Referral updated successfully.");
            }
            else
            {
                return (false, "Failed to update the Referral.");
            }
        }

    }
}


