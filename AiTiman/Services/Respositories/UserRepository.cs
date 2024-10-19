using MongoDB.Driver;
using AiTiman_API.Models;
using AiTiman_API.Services.DTO;
using AiTiman_API.Services.Interfaces;
using Microsoft.Extensions.Options;


namespace AiTiman_API.Services.Respositories
{
    public class UserRepository : IUser
    {
        private readonly IMongoCollection<User> _UserCollection;
        public UserRepository(IOptions<AiTimanDatabaseSettings> aiTimanDatabaseSettings)
        {

            if (aiTimanDatabaseSettings.Value.ConnectionString == null)
            {
                throw new ArgumentNullException("ConnectionString is null");
            }

            var mongoclient = new MongoClient(aiTimanDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoclient.GetDatabase(aiTimanDatabaseSettings.Value.DatabaseName);

            _UserCollection = mongoDatabase.GetCollection<User>(aiTimanDatabaseSettings.Value.UsersCollectionName);
        }

        public async Task<(bool, string)> AddNewUser(CreateUserDTO createUser)
        {
            // Validate UserName
            if (string.IsNullOrWhiteSpace(createUser.UserName))
            {
                return (false, "UserName name is required.");
            }

            // Validate Usertatus
            if (string.IsNullOrWhiteSpace(createUser.Password))
            {
                return (false, "Password status is required.");
            }

            // Validate Useretter
            if (string.IsNullOrWhiteSpace(createUser.Email))
            {
                return (false, "Email setter is required.");
            }

            // If all validations pass, create the new User
            var newUser = new User
            {
                UserName = createUser.UserName,
                Email = createUser.Email,
                Password = createUser.Password,
                Address = createUser.Address,
                Birthdate = createUser.Birthdate,
                VerificationCode = createUser.VerificationCode,
                Role = createUser.Role,


                FirstName = createUser.FirstName,
                LastName = createUser.LastName,
                MiddleName = createUser.MiddleName,
                Religion = createUser.Religion,
                Gender = createUser.Gender,
                CivilStatus = createUser.CivilStatus,
                GuardianName = createUser.GuardianName,
                ProfilePic = createUser.ProfilePic,
                SignPic = createUser.SignPic,


                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            };

            // Insert the new User into the database
            await _UserCollection.InsertOneAsync(newUser);

            return (true, "New User Created!");
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await _UserCollection.Find(user => user.UserName == userName).FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserProfileByUsername(string username)
        {
            // Check if the username is null or empty
            if (string.IsNullOrWhiteSpace(username))
            {
                // Optionally, you could log this or throw an exception
                return null; // Return null if the username is invalid
            }

            // Fetch the user profile by username
            var user = await _UserCollection.Find(user => user.UserName == username).FirstOrDefaultAsync();

            // Optionally, log the retrieval or handle a specific case if user is not found

            return user; // Return the found user or null if not found
        }

        public async Task<(bool, string)> DeleteUser(string? id)
        {
            // Validate the ID
            if (string.IsNullOrWhiteSpace(id))
            {
                return (false, "User ID is required.");
            }

            // Check if the User exists
            var User = await _UserCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (User == null)
            {
                return (false, "User not found.");
            }

            // Attempt to delete the User
            var result = await _UserCollection.DeleteOneAsync(x => x.Id == id);

            // Check if the delete operation was successful
            if (result.DeletedCount > 0)
            {
                return (true, "User deleted successfully.");
            }
            else
            {
                return (false, "Failed to delete the User.");
            }
        }


        public async Task<User?> fetchUser(string? id)
        {
            // Validate the ID
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;  // Return null if the ID is invalid
            }

            // Find the User by ID and return the first match or null if not found
            return await _UserCollection.Find(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<User>> fetchUser()
        {
            // Return a list of all User from the database
            return await _UserCollection.Find(_ => true).ToListAsync();
        }
        public async Task<List<User>> fetchUsers() =>
            await _UserCollection.Find(_ => true).ToListAsync();

        public async Task<(bool, string)> UpdateUser(string id, UpdateUserDTO updateUser)
        {
            // Validate the ID
            if (string.IsNullOrWhiteSpace(id))
            {
                return (false, "User ID is required.");
            }

            // Find the existing User by ID
            var existingUser = await _UserCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (existingUser == null)
            {
                return (false, "User not found.");
            }

            // Update the relevant fields if they are provided
            var updateDefinition = Builders<User>.Update
                .Set(x => x.DateUpdated, DateTime.UtcNow); // Always update the `DateUpdated`

            if (!string.IsNullOrWhiteSpace(updateUser.UserName))
            {
                updateDefinition = updateDefinition.Set(x => x.UserName, updateUser.UserName);
            }

            if (!string.IsNullOrWhiteSpace(updateUser.Email))
            {
                updateDefinition = updateDefinition.Set(x => x.Email, updateUser.Email);
            }

            if (!string.IsNullOrWhiteSpace(updateUser.Password))
            {
                updateDefinition = updateDefinition.Set(x => x.Password, updateUser.Password);
            }

            if (!string.IsNullOrWhiteSpace(updateUser.Address))
            {
                updateDefinition = updateDefinition.Set(x => x.Address, updateUser.Address);
            }
            if (updateUser.Birthdate != null)
            {
                updateDefinition = updateDefinition.Set(x => x.Birthdate, updateUser.Birthdate);
            }

            if (!string.IsNullOrWhiteSpace(updateUser.FirstName))
            {
                updateDefinition = updateDefinition.Set(x => x.FirstName, updateUser.FirstName);
            }

            if (!string.IsNullOrWhiteSpace(updateUser.LastName))
            {
                updateDefinition = updateDefinition.Set(x => x.LastName, updateUser.LastName);
            }

            if (!string.IsNullOrWhiteSpace(updateUser.MiddleName))
            {
                updateDefinition = updateDefinition.Set(x => x.MiddleName, updateUser.MiddleName);
            }

            if (!string.IsNullOrWhiteSpace(updateUser.Religion))
            {
                updateDefinition = updateDefinition.Set(x => x.Religion, updateUser.Religion);
            }

            if (!string.IsNullOrWhiteSpace(updateUser.Gender))
            {
                updateDefinition = updateDefinition.Set(x => x.Gender, updateUser.Gender);
            }

            if (!string.IsNullOrWhiteSpace(updateUser.CivilStatus))
            {
                updateDefinition = updateDefinition.Set(x => x.CivilStatus, updateUser.CivilStatus);
            }

            if (!string.IsNullOrWhiteSpace(updateUser.GuardianName))
            {
                updateDefinition = updateDefinition.Set(x => x.GuardianName, updateUser.GuardianName);
            }
            if (updateUser.ProfilePic != null && updateUser.ProfilePic.Length > 0)
            {
                updateDefinition = updateDefinition.Set(x => x.ProfilePic, updateUser.ProfilePic);
            }

            if (updateUser.SignPic != null && updateUser.SignPic.Length > 0)
            {
                updateDefinition = updateDefinition.Set(x => x.SignPic, updateUser.SignPic);
            }



            // Update the User in the database
            var result = await _UserCollection.UpdateOneAsync(x => x.Id == id, updateDefinition);

            // Check if the update was successful
            if (result.ModifiedCount > 0)
            {
                return (true, "User updated successfully.");
            }
            else
            {
                return (false, "Failed to update the User.");
            }
        }

        public async Task<User> ValidateUser(string username, string password)
        {
            // Query MongoDB to find the user by username
            var user = await _UserCollection.Find(u => u.UserName == username).FirstOrDefaultAsync();

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