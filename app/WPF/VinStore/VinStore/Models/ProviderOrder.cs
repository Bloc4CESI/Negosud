using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace ApiNegosud.Models
{
    public class ProviderOrder
    {
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public Decimal Price { get; set; }
        [Required]
        public ProviderOrderStatus ProviderOrderStatus { get; set; }

        [ForeignKey("Provider")]
        public int ProviderId { get; set; }
        public virtual Provider? Provider { get; set; }
        public List<ProviderOrderLine>? ProviderOrderLines { get; set; }
        public string? ProductNames { get; set; }

    }
    public enum ProviderOrderStatus
    {
        REFUSE,
        LIVRE,
        ENCOURSDELIVRAISON,
        VALIDE,
        ENCOURSDEVALIDATION
    }

}
