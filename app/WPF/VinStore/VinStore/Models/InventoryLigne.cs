using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiNegosud.Models;

namespace VinStore.Models
{
    public class InventoryLigne
    {
        public int Id { get; set; }

        [ForeignKey("Stock")]
        public int StockId { get; set; }

        [ForeignKey("Inventory")]
        public int InventoryId { get; set; }

        [Required]
        public int QuantityInventory { get; set; }
        [Required]
        public int QuantityStock { get; set; }
        public Stock? Stock { get; set; }
        public Inventory? Inventory { get; set; }
    }
}
