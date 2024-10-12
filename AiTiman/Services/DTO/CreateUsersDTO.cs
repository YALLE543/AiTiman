using MongoDB.Bson.Serialization.Attributes;

namespace AiTiman_API.Services.DTO
{
    public class CreateUsersDTO
    {
        public string? UserName { get; set; }

        public String? Email { get; set; }

        public string? Password { get; set; }

        public string? Address { get; set; }

        public DateTime Birthdate { get; set; }

        public int Age { get; set; }

        public string? VerificationCode { get; set; }

        public string? Role { get; set; }


        //PROFILES//
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? MiddleName { get; set; }

        public string? Religion { get; set; }

        public string? Gender { get; set; }

        public string? CivilStatus { get; set; }

        public string? GuardianName { get; set; }

        public string? ProfilePic { get; set; }

        public string? SignPic { get; set; }
    }
}