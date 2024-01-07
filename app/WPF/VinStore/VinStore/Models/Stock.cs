using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNegosud.Models
{
    public class Stock
    {
        public int Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        public int? Minimum { get; set; }
        public int? Maximum { get; set; }
        [Required]
        public bool AutoOrder { get; set; } =  true;
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        // Propriété de navigation vers produit
        public Product? Product { get; set; }
    }
}
