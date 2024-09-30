using MongoDB.Driver;
using AiTiman_API.Models;
using AiTiman_API.Services.DTO;
using AiTiman_API.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AiTiman_API.Services.Repositories
{
    public class BookedRepository : IBooked
    {
        private readonly IMongoCollection<Booked> _bookedCollection;

        public BookedRepository(IOptions<AiTimanDatabaseSettings> aiTimanDatabaseSettings)
        {
            if (aiTimanDatabaseSettings.Value.ConnectionString == null)
            {
                throw new ArgumentNullException(nameof(aiTimanDatabaseSettings.Value.ConnectionString), "ConnectionString is null");
            }

            var mongoClient = new MongoClient(aiTimanDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(aiTimanDatabaseSettings.Value.DatabaseName);
            _bookedCollection = mongoDatabase.GetCollection<Booked>(aiTimanDatabaseSettings.Value.BookedCollectionName);
        }

        // Add a new booked appointment
        public async Task<(bool, string)> AddNewBooked(CreateBookedDTO createBooked)
        {
            if (string.IsNullOrWhiteSpace(createBooked.AppointmentName))
                return (false, "Appointment name is required.");
            if (string.IsNullOrWhiteSpace(createBooked.PatientName))
                return (false, "Patient name is required.");
            if (string.IsNullOrWhiteSpace(createBooked.BookingStatus))
                return (false, "Booking status is required.");
            if (createBooked.AppointmentScheduleDate < DateTime.Today)
                return (false, "Appointment date must be today or in the future.");

            var newBooked = new Booked
            {
                AppointmentName = createBooked.AppointmentName,
                AppointmentScheduleDate = createBooked.AppointmentScheduleDate,
                AppointmentScheduleTime = createBooked.AppointmentScheduleTime,
                AppointmentDoctorInCharge = createBooked.AppointmentDoctorInCharge,
                PatientName = createBooked.PatientName,
                Address = createBooked.Address,
                Birthdate = createBooked.Birthdate,
                Age = createBooked.Age,
                Gender = createBooked.Gender,
                ApprovedBy = createBooked.ApprovedBy,
                BookingStatus = createBooked.BookingStatus,
                GuardianName = createBooked.GuardianName,
                BookingApprovedDate = createBooked.BookingApprovedDate,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            };

            await _bookedCollection.InsertOneAsync(newBooked);
            return (true, "New booked appointment created.");
        }

        // Delete a booked appointment by ID
        public async Task<(bool, string)> DeleteBooked(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return (false, "Booked Id  is required.");

            var result = await _bookedCollection.DeleteOneAsync(x => x.Id == id);
            if (result.DeletedCount > 0)
                return (true, "Book deleted successfully.");
            else
                return (false, "Book not found or failed to delete.");
        }

        // Fetch a single booked appointment by ID
        public async Task<Booked?> fetchBooked(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            return await _bookedCollection.Find(i => i.Id == id).FirstOrDefaultAsync();
        }

        // Fetch all booked appointments
        public async Task<List<Booked>> fetchBooked() =>
            await _bookedCollection.Find(_ => true).ToListAsync();

        // Update an existing booked appointment
        public async Task<(bool, string)> UpdateBooked(string id, UpdateBookedDTO updateBooked)
        {
            if (string.IsNullOrWhiteSpace(id))
                return (false, "Booking ID is required.");

            var existingBooked = await _bookedCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (existingBooked == null)
                return (false, "Booking not found.");

            var updateDefinition = Builders<Booked>.Update.Set(x => x.DateUpdated, DateTime.UtcNow);

            if (!string.IsNullOrWhiteSpace(updateBooked.AppointmentName))
                updateDefinition = updateDefinition.Set(x => x.AppointmentName, updateBooked.AppointmentName);

            if (updateBooked.AppointmentScheduleDate.HasValue)
                updateDefinition = updateDefinition.Set(x => x.AppointmentScheduleDate, updateBooked.AppointmentScheduleDate.Value);

            if (!string.IsNullOrWhiteSpace(updateBooked.AppointmentScheduleTime))
                updateDefinition = updateDefinition.Set(x => x.AppointmentScheduleTime, updateBooked.AppointmentScheduleTime);

            if (!string.IsNullOrWhiteSpace(updateBooked.AppointmentDoctorInCharge))
                updateDefinition = updateDefinition.Set(x => x.AppointmentDoctorInCharge, updateBooked.AppointmentDoctorInCharge);

            if (!string.IsNullOrWhiteSpace(updateBooked.BookingStatus))
                updateDefinition = updateDefinition.Set(x => x.BookingStatus, updateBooked.BookingStatus);

            var result = await _bookedCollection.UpdateOneAsync(x => x.Id == id, updateDefinition);

            if (result.ModifiedCount > 0)
                return (true, "Book updated successfully.");
            else
                return (false, "Failed to update the booking.");
        }
    }
}
