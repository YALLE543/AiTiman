using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel;

namespace AiTiman_API.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonRequired]
        [BsonElement("User Name")]
        public string? UserName { get; set; }

        [BsonElement("Email")]
        public String? Email { get; set; }

        [BsonElement("Password")]
        public string? Password { get; set; }

        [BsonElement("Address")]
        public string? Address { get; set; }

        [BsonElement("Birthdate")]
        public DateTime Birthdate { get; set; }

        [BsonIgnore]
        [BsonElement("Age")]
        private int? _age;

        public int? Age
        {
            get
            {
                if (_age.HasValue)
                {
                    return _age;
                }

                // Auto-calculate age from Birthdate if not explicitly set
                var today = DateTime.UtcNow.Date;
                var age = today.Year - Birthdate.Year;

                if (Birthdate > today.AddYears(-age))
                    age--;

                return age;
            }
            set
            {
                // Set age explicitly if provided
                _age = value;
            }
        }


        [BsonElement("Verification Code")]
        public string? VerificationCode { get; set; }

        [BsonElement("Role")]
        public string? Role { get; set; }


        //PROFILES//
        [BsonElement("First Name")]
        public string? FirstName { get; set; }

        [BsonElement("Last Name")]
        public string? LastName { get; set; }

        [BsonElement("Middle Name")]
        public string? MiddleName { get; set; }

        [BsonElement("Religion")]
        public string? Religion { get; set; }

        [BsonElement("Gender")]
        public string? Gender { get; set; }

        [BsonElement("Civil Status")]
        public string? CivilStatus { get; set; }

        [BsonElement("Guardian Name")]
        public string? GuardianName { get; set; }

        [BsonElement("Profile Picture")]
        public byte[]? ProfilePic { get; set; }

        [BsonElement("Signature Picture")]
        public byte[]? SignPic { get; set; }




        [BsonElement("Date Created")]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        [BsonElement("Date Updated")]
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;
    }
}