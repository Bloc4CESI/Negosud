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
        public InventoryEnum StatusInventory { get; set; }

        public List<InventoryLigne>? InventoryLignes { get; set; }

        public enum InventoryEnum
        {
            ENCOURSDEVALIDATION,
            VALIDE,
            REFUSE
        }

    }
}
