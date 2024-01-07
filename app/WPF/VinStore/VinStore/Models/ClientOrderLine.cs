﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace ApiNegosud.Models
{
    public class ClientOrderLine
    {
        public int Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public Decimal Price { get; set; }

        [ForeignKey("ClientOrder")]
        public int ClientOrderId { get; set; }
        public virtual required ClientOrder ClientOrder { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }
    }

}
