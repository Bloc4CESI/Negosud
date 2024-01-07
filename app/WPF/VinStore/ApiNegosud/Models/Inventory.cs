using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNegosud.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]  
        public int QuantityInventory { get; set; }
        [ForeignKey("Stock")]
        public int StockId { get; set; }
        public Stock? Stock{ get; set; }
    }
}
