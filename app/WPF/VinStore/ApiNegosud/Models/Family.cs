using System.ComponentModel.DataAnnotations;

namespace ApiNegosud.Models
{
    public class Family
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        // Propriété de navigation vers les produits de cette famille
        public List<Product>? Products { get; set; }
    }
}
