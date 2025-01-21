using System.ComponentModel.DataAnnotations;

namespace Test1LearnNewVersion.Models.Entities
{
    public class user
    {
        [Key] // Primary Key
        public int UserId { get; set; } // Auto-increment by default with EF

        [Required] // Email is mandatory
        [EmailAddress] // Validates email format
        [StringLength(255)] // Optional: Limit email length
        public string Email { get; set; } // Unique constraint will be defined via Fluent API

        [Required] // Password is mandatory
        [StringLength(255)] // Optional: Limit hashed password length
        public string PasswordHash { get; set; }

        [Required] // Ensure created_at is mandatory
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
