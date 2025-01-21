using System.ComponentModel.DataAnnotations;

namespace Test1LearnNewVersion.Models.DTO
{
    public class UpdateuserRequestDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Password length must be greater than 3 characters")]

        public string PasswordHash { get; set; }
    }
}
