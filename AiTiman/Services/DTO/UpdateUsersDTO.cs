namespace AiTiman_API.Services.DTO
{
    public class UpdateUsersDTO
    {
        public string? UserName { get; set; }

        public String? Email { get; set; }

        public string? Password { get; set; }

        public string? Address { get; set; }

        public DateTime Birthdate { get; set; }

        public int Age { get; set; }


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
