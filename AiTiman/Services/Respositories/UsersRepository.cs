using MongoDB.Driver;
using AiTiman_API.Models;
using AiTiman_API.Services.DTO;
using AiTiman_API.Services.Interfaces;
using Microsoft.Extensions.Options;


namespace AiTiman_API.Services.Respositories
{
    public class UsersRepository : IUsers
    {
        private readonly IMongoCollection<Users> _UsersCollection;
        public UsersRepository(IOptions<AiTimanDatabaseSettings> aiTimanDatabaseSettings)
        {

            if (aiTimanDatabaseSettings.Value.ConnectionString == null)
            {
                throw new ArgumentNullException("ConnectionString is null");
            }

            var mongoclient = new MongoClient(aiTimanDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoclient.GetDatabase(aiTimanDatabaseSettings.Value.DatabaseName);

            _UsersCollection = mongoDatabase.GetCollection<Users>(aiTimanDatabaseSettings.Value.UsersCollectionName);
        }

        public async Task<(bool, string)> AddNewUsers(CreateUsersDTO createUsers)
        {
            // Validate UsersName
            if (string.IsNullOrWhiteSpace(createUsers.UserName))
            {
                return (false, "UserName name is required.");
            }

            // Validate UsersStatus
            if (string.IsNullOrWhiteSpace(createUsers.Password))
            {
                return (false, "Password status is required.");
            }

            // Validate UsersSetter
            if (string.IsNullOrWhiteSpace(createUsers.Email))
            {
                return (false, "Email setter is required.");
            }

            // If all validations pass, create the new Users
            var newUsers = new Users
            {
                UserName = createUsers.UserName,
                Email = createUsers.Email,
                Password = createUsers.Password,
                Address = createUsers.Address,
                Birthdate = createUsers.Birthdate,
                VerificationCode = createUsers.VerificationCode,
                Role = createUsers.Role,


                FirstName = createUsers.FirstName,
                LastName = createUsers.LastName,
                MiddleName = createUsers.MiddleName,
                Religion = createUsers.Religion,
                Gender = createUsers.Gender,
                CivilStatus = createUsers.CivilStatus,
                GuardianName = createUsers.GuardianName,
                ProfilePic = createUsers.ProfilePic,
                SignPic = createUsers.SignPic,


                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            };

            // Insert the new Users into the database
            await _UsersCollection.InsertOneAsync(newUsers);

            return (true, "New Users Created!");
        }

        public async Task<Users> GetUserByUserNameAsync(string userName)
        {
            return await _UsersCollection.Find(user => user.UserName == userName).FirstOrDefaultAsync();
        }

        public async Task<Users?> GetUserProfileByUsername(string username)
        {
            // Check if the username is null or empty
            if (string.IsNullOrWhiteSpace(username))
            {
                // Optionally, you could log this or throw an exception
                return null; // Return null if the username is invalid
            }

            // Fetch the user profile by username
            var user = await _UsersCollection.Find(user => user.UserName == username).FirstOrDefaultAsync();

            // Optionally, log the retrieval or handle a specific case if user is not found

            return user; // Return the found user or null if not found
        }

        public async Task<(bool, string)> DeleteUsers(string? id)
        {
            // Validate the ID
            if (string.IsNullOrWhiteSpace(id))
            {
                return (false, "Users ID is required.");
            }

            // Check if the Users exists
            var Users = await _UsersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (Users == null)
            {
                return (false, "Users not found.");
            }

            // Attempt to delete the Users
            var result = await _UsersCollection.DeleteOneAsync(x => x.Id == id);

            // Check if the delete operation was successful
            if (result.DeletedCount > 0)
            {
                return (true, "Users deleted successfully.");
            }
            else
            {
                return (false, "Failed to delete the Users.");
            }
        }


        public async Task<Users?> fetchUsers(string? id)
        {
            // Validate the ID
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;  // Return null if the ID is invalid
            }

            // Find the Users by ID and return the first match or null if not found
            return await _UsersCollection.Find(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Users>> fetchUsers()
        {
            // Return a list of all users from the database
            return await _UsersCollection.Find(_ => true).ToListAsync();
        }
        public async Task<List<Users>> fetchUserss() =>
            await _UsersCollection.Find(_ => true).ToListAsync();

        public async Task<(bool, string)> UpdateUsers(string id, UpdateUsersDTO updateUsers)
        {
            // Validate the ID
            if (string.IsNullOrWhiteSpace(id))
            {
                return (false, "Users ID is required.");
            }

            // Find the existing Users by ID
            var existingUsers = await _UsersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (existingUsers == null)
            {
                return (false, "Users not found.");
            }

            // Update the relevant fields if they are provided
            var updateDefinition = Builders<Users>.Update
                .Set(x => x.DateUpdated, DateTime.UtcNow); // Always update the `DateUpdated`

            if (!string.IsNullOrWhiteSpace(updateUsers.UserName))
            {
                updateDefinition = updateDefinition.Set(x => x.UserName, updateUsers.UserName);
            }

            if (!string.IsNullOrWhiteSpace(updateUsers.Email))
            {
                updateDefinition = updateDefinition.Set(x => x.Email, updateUsers.Email);
            }

            if (!string.IsNullOrWhiteSpace(updateUsers.Password))
            {
                updateDefinition = updateDefinition.Set(x => x.Password, updateUsers.Password);
            }

            if (!string.IsNullOrWhiteSpace(updateUsers.Address))
            {
                updateDefinition = updateDefinition.Set(x => x.Address, updateUsers.Address);
            }
            if (updateUsers.Birthdate != null)
            {
                updateDefinition = updateDefinition.Set(x => x.Birthdate, updateUsers.Birthdate);
            }

            if (!string.IsNullOrWhiteSpace(updateUsers.FirstName))
            {
                updateDefinition = updateDefinition.Set(x => x.FirstName, updateUsers.FirstName);
            }

            if (!string.IsNullOrWhiteSpace(updateUsers.LastName))
            {
                updateDefinition = updateDefinition.Set(x => x.LastName, updateUsers.LastName);
            }

            if (!string.IsNullOrWhiteSpace(updateUsers.MiddleName))
            {
                updateDefinition = updateDefinition.Set(x => x.MiddleName, updateUsers.MiddleName);
            }

            if (!string.IsNullOrWhiteSpace(updateUsers.Religion))
            {
                updateDefinition = updateDefinition.Set(x => x.Religion, updateUsers.Religion);
            }

            if (!string.IsNullOrWhiteSpace(updateUsers.Gender))
            {
                updateDefinition = updateDefinition.Set(x => x.Gender, updateUsers.Gender);
            }

            if (!string.IsNullOrWhiteSpace(updateUsers.CivilStatus))
            {
                updateDefinition = updateDefinition.Set(x => x.CivilStatus, updateUsers.CivilStatus);
            }

            if (!string.IsNullOrWhiteSpace(updateUsers.GuardianName))
            {
                updateDefinition = updateDefinition.Set(x => x.GuardianName, updateUsers.GuardianName);
            }

            if (!string.IsNullOrWhiteSpace(updateUsers.ProfilePic))
            {
                updateDefinition = updateDefinition.Set(x => x.ProfilePic, updateUsers.ProfilePic);
            }

            if (!string.IsNullOrWhiteSpace(updateUsers.SignPic))
            {
                updateDefinition = updateDefinition.Set(x => x.SignPic, updateUsers.SignPic);
            }



            // Update the Users in the database
            var result = await _UsersCollection.UpdateOneAsync(x => x.Id == id, updateDefinition);

            // Check if the update was successful
            if (result.ModifiedCount > 0)
            {
                return (true, "Users updated successfully.");
            }
            else
            {
                return (false, "Failed to update the Users.");
            }
        }

        public async Task<Users> ValidateUser(string username, string password)
        {
            // Query MongoDB to find the user by username
            var user = await _UsersCollection.Find(u => u.UserName == username).FirstOrDefaultAsync();

            if (user != null)
            {
                // Directly compare the plain text password
                if (user.Password == password) // Avoid this for security reasons
                {
                    return user;
                }
            }

            // Return null if user not found or password invalid
            return null;
        }
    }
}