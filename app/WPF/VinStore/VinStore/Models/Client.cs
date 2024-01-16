using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiNegosud.Models
{
    public class Client
    {
        public int Id { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }

        [ForeignKey("Address")]
        public int? AddressId { get; set; }
        /*Propriété de navigation vers l'adresse du client*/

        public virtual Address? Address { get; set; }

    }
}
