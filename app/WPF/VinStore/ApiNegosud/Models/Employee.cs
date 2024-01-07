using System.ComponentModel.DataAnnotations;

namespace ApiNegosud.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }

    }
}
