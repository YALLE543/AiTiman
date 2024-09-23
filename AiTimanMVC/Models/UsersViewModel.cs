using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AiTimanMVC.Models
{
    public class UsersViewModel
    {
        
        public string? Id { get; set; }
        [Required]
        [DisplayName("User Name")]
        public string? UserName { get; set; }

        [DisplayName("Email")]
        public String? Email { get; set; }

        [DisplayName("Password")]
        public string? Password { get; set; }

        [DisplayName("Address")]
        public string? Address { get; set; }

        [DisplayName("Birthdate")]
        public DateTime Birthdate { get; set; }

        [DisplayName("Age")]
        public int? Age
        {
            get
            {
                var today = DateTime.UtcNow.Date;
                var age = today.Year - Birthdate.Year;

                if (Birthdate > today.AddYears(-age))
                    age--;

                return age;
            }
        }

        [DisplayName("Verification Code")]
        public string? VerificationCode { get; set; }

        [DisplayName("Role")]
        public string? Role { get; set; }


        //PROFILES//
        [DisplayName("First Name")]
        public string? FirstName { get; set; }

        [DisplayName("Last Name")]
        public string? LastName { get; set; }

        [DisplayName("Middle Name")]
        public string? MiddleName { get; set; }

        [DisplayName("Religion")]
        public string? Religion { get; set; }

        [DisplayName("Gender")]
        public string? Gender { get; set; }

        [DisplayName("Civil Status")]
        public string? CivilStatus { get; set; }

        [DisplayName("Guardian Name")]
        public string? GuardianName { get; set; }

        [DisplayName("Profile Picture")]
        public string? ProfilePic { get; set; }

        [DisplayName("Signature Picture")]
        public string? SignPic { get; set; }




        [DisplayName("Date Created")]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        [DisplayName("Date Updated")]
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;
    }
}
