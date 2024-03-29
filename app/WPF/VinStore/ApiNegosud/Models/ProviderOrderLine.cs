﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiNegosud.Models
{
    public class ProviderOrderLine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public Decimal Price { get; set; }

        [ForeignKey("ProviderOrder")]
        public int ProviderOrderId { get; set; }
        public virtual ProviderOrder? ProviderOrder { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }
    }
}
