using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiNegosud.Models
{
    public class Provider
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }

        [ForeignKey("Address")]
        public int AddressId { get; set; }
        /*Propriété de navigation vers l'adresse: un fournisseur peut avoir une seule adresse */
        public Address? Address { get; set; }
        // Propriété de navigation vers les produits de ce fournisseur
        public List<Product>? Products { get; set; }


    }
}
