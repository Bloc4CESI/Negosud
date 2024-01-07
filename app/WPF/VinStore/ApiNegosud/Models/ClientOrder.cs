using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiNegosud.Models
{
    public class ClientOrder
    {
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public Decimal Price { get; set; }
        [Required]
        public OrderStatus OrderStatus { get; set; }

        [ForeignKey("Client")]
        public int ClientId { get; set; }
        public virtual Client? Client { get; set; }
        public List<ClientOrderLine>? ClientOrderLines { get; set; }
    }
    public enum OrderStatus
    {
        REFUSE,
        LIVRE,
        ENCOURSDELIVRAISON,
        VALIDE,
        ENCOURSDEVALIDATION
    }
}
