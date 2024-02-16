using System.Globalization;
using ApiNegosud.DataAccess;
using ApiNegosud.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static ApiNegosud.Models.Inventory;

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

        [HttpPost]
        public IActionResult Post(Inventory inventory)
        {
            try
            {
                _context.Add(inventory);
                // Ajouter les lignes inventaires associées
                if (inventory.InventoryLignes != null && inventory.InventoryLignes.Any())
                {
                    foreach (var inventoryLigne in inventory.InventoryLignes)
                    {
                        inventoryLigne.InventoryId = inventory.Id;

                        _context.InventoryLigne.Add(inventoryLigne);
                    }

                    _context.SaveChanges();
                }
                var newInventory = _context.Inventory.Include(p => p.InventoryLignes)
                            .FirstOrDefault(p => p.Id == inventory.Id);
                if (newInventory != null)
                {
                    return Ok(new { newInventory = newInventory, Message = "L'inventaire est ajoutée avec succès!" });
                }
                else
                {
                    return BadRequest("L'inventaire n'a pas pu être récupérée après l'ajout.");
                }
            }
            catch (Exception ex)
            {
                // En cas d'erreur, renvoyer un message d'erreur
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("UpdateInventory")]
        public IActionResult UpdateInventory(Inventory updatedInventory)
        {
            try
            {
                var existingInventory = _context.Inventory.Include(i => i.InventoryLignes).FirstOrDefault(i => i.Id == updatedInventory.Id);

                if (existingInventory == null)
                {
                    return NotFound($"L'inventaire {updatedInventory.Id} n'a pas été trouvé");
                }

                // Mettre à jour les propriétés de l'inventaire
                existingInventory.StatusInventory = updatedInventory.StatusInventory;
                existingInventory.Date = updatedInventory.Date;
                if (updatedInventory.InventoryLignes != null && updatedInventory.InventoryLignes.Any())
                {
                    foreach (var updatedInventoryLigne in updatedInventory.InventoryLignes)
                    {
                        updatedInventoryLigne.InventoryId = updatedInventory.Id;

                        var ExistingLigne = existingInventory.InventoryLignes!.FirstOrDefault(il => il.Id == updatedInventoryLigne.Id);

                        if (ExistingLigne != null)
                        {
                            ExistingLigne.QuantityInventory = updatedInventoryLigne.QuantityInventory;
                        }
                        else
                        {
                            existingInventory.InventoryLignes!.Add(updatedInventoryLigne);
                        }
                    }
                }
                // si le status est validé on update les quantités des articles selon les inventaires
                if (updatedInventory.StatusInventory == Inventory.InventoryEnum.VALIDE)
                {
                    if (updatedInventory.InventoryLignes != null && updatedInventory.InventoryLignes.Any())
                    {
                        foreach (var UpdatedInventoryLigne in updatedInventory.InventoryLignes)
                        {
                            var productStock = _context.Stock.FirstOrDefault(s => s.Id == UpdatedInventoryLigne.StockId);
                            if (productStock != null)
                            {
                                // Mettre à jour la quantité de stock du produit
                                productStock.Quantity = UpdatedInventoryLigne.QuantityInventory;
                            }
                        }
                    }
                }
                _context.SaveChanges();
                return Ok("Mise à jour réussie");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("bystatus/{status}")]
        public IActionResult GetByStatus(InventoryEnum  status, DateTime? date = null)
        {
            try
            {
                var inventoryQuery = _context.Inventory
                    .Include(i => i.InventoryLignes!)
                        .ThenInclude(il => il.Stock)
                            .ThenInclude(s => s.Product)
                    .Where(po => po.StatusInventory == status);
                if (date.HasValue)
                {
                    inventoryQuery = inventoryQuery.Where(po => po.Date >= date.Value.Date);
                }
                var inventories = inventoryQuery.OrderByDescending(po => po.Id).ToList();

                return Ok(inventories);
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
                var inventory = _context.Inventory
                    .Include(i => i.InventoryLignes) // Inclure les lignes d'inventaire
                    .SingleOrDefault(i => i.Id == id);

                if (inventory == null)
                {
                    return BadRequest($"L'inventaire l'ID {id} est non trouvée");
                }
                if (inventory.InventoryLignes != null && inventory.InventoryLignes.Any())
                {
                    // Supprimer toutes les lignes d'inventaire associées
                    _context.InventoryLigne.RemoveRange(inventory.InventoryLignes);
                }
                // Supprimer la commande fournisseur elle-même
                _context.Inventory.Remove(inventory);

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
