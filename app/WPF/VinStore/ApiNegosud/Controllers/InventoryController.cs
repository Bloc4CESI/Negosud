using System.Globalization;
using ApiNegosud.DataAccess;
using ApiNegosud.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiNegosud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly ConnexionDbContext _context;

        public InventoryController(ConnexionDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult Get(string nameProduct = null)
    {
            try
            {
                // Filtrer les inventaires en fonction des paramètres fournis, en castant en minuscules
                var inventories = _context.Inventory
                    .Include(i => i.Stock)
                    .ThenInclude(s => s.Product)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(nameProduct))
                {
                    // Filtrer par nom de produit
                    inventories = inventories.Where(i => i.Stock.Product!.Name.ToLower().Contains(nameProduct.Trim().ToLower()));
                }
                // Convertir les résultats en une liste
                var filteredInventories = inventories.ToList();

                if (filteredInventories.Count == 0)
                {
                    return NotFound("Aucun inventaire trouvé avec les paramètres fournis.");
                }
                else
                {
                    return Ok(filteredInventories);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var inventory = _context.Inventory.Include(i => i.Stock)
                        .ThenInclude(s => s.Product).FirstOrDefault(i => i.Id == id);
                if (inventory == null)
                {
                    return NotFound($"L'inventaire avec l'ID {id} n'a pas été trouvé");
                }
                else
                {
                    return Ok(inventory);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post(Inventory newInventory)
        {
            try
            {

                // Vérifiez si le stock associé à l'inventaire existe
                var existingStock = _context.Stock.FirstOrDefault(s => s.Id == newInventory.StockId);

                if (existingStock == null)
                {
                    return BadRequest("Stock non trouvé. Veuillez fournir un ID de stock valide.");
                }

                // Ajoutez l'inventaire à la base de données sans charger le détail du stock
                _context.Inventory.Add(new Inventory
                {
                    Date = newInventory.Date,
                    QuantityInventory = newInventory.QuantityInventory,
                    StockId = newInventory.StockId
                });

                _context.SaveChanges();

                return Ok("L'inventaire a été créé avec succès!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public IActionResult Put(Inventory updatedInventory)
        {
            try
            {
                // Récupérer l'inventaire existant de la base de données
                var existingInventory = _context.Inventory.FirstOrDefault(i => i.Id == updatedInventory.Id);

                if (existingInventory == null)
                {
                    return NotFound($"L'inventaire avec l'ID {updatedInventory.Id} n'a pas été trouvé.");
                }
                // Mettre à jour les propriétés de l'inventaire existant
                existingInventory.Date = updatedInventory.Date;
                existingInventory.QuantityInventory = updatedInventory.QuantityInventory;
                _context.SaveChanges();

                return Ok("L'inventaire a été mis à jour avec succès!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var Inventory = _context.Inventory.Find(id);
                if (Inventory == null)
                {
                    return BadRequest("Inventaire non trouvé");
                }
                _context.Inventory.Remove(Inventory);
                _context.SaveChanges();
                return Ok("Suppression réussie");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}
