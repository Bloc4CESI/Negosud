using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNegosud.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public Decimal Price { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public string Description { get; set; }

        [Required]
        public DateOnly DateProduction { get; set; } 
        [Required]
        public int NbProductBox { get; set; }

        [Required]
        public string Home { get; set; }
        // Clé étrangère vers Family
        [ForeignKey("Family")]
        public int FamilyId { get; set; }

        // Propriété de navigation vers la famille de ce produit
        public Family? Family { get; set; }

        [ForeignKey("Provider")]
        public int ProviderId { get; set; }

        // Propriété de navigation vers le fournisseur de ce produit
        public Provider? Provider { get; set; }
        // Propriété de navigation vers le stock
        public Stock? Stock { get; set; }
    }
}
