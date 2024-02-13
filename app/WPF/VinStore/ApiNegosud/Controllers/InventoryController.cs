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
        public IActionResult Post(Inventory NewInventory)
        {
            try
            {
                _context.Add(NewInventory);
                // Ajouter les lignes inventaires associées
                if (NewInventory.InventoryLignes != null && NewInventory.InventoryLignes.Any())
                {
                    foreach (var InventoryLigne in NewInventory.InventoryLignes)
                    {
                        InventoryLigne.InventoryId = NewInventory.Id;

                        _context.InventoryLigne.Add(InventoryLigne);
                    }

                    _context.SaveChanges();
                }
                var newInventory = _context.Inventory.Include(p => p.InventoryLignes)
                            .FirstOrDefault(p => p.Id == NewInventory.Id);
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
        public IActionResult UpdateInventory(Inventory UpdatedInventory)
        {
            try
            {
                var ExistingInventory = _context.Inventory.Include(i => i.InventoryLignes).FirstOrDefault(i => i.Id == UpdatedInventory.Id);

                if (ExistingInventory == null)
                {
                    return NotFound($"L'inventaire {UpdatedInventory.Id} n'a pas été trouvé");
                }

                // Mettre à jour les propriétés de l'inventaire
                ExistingInventory.StatusInventory = UpdatedInventory.StatusInventory;
                ExistingInventory.Date = UpdatedInventory.Date;
                if (UpdatedInventory.InventoryLignes != null && UpdatedInventory.InventoryLignes.Any())
                {
                    foreach (var UpdatedInventoryLigne in UpdatedInventory.InventoryLignes)
                    {
                        UpdatedInventoryLigne.InventoryId = UpdatedInventory.Id;

                        var ExistingLigne = ExistingInventory.InventoryLignes!.FirstOrDefault(il => il.Id == UpdatedInventoryLigne.Id);

                        if (ExistingLigne != null)
                        {
                            ExistingLigne.QuantityInventory = UpdatedInventoryLigne.QuantityInventory;
                        }
                        else
                        {
                            ExistingInventory.InventoryLignes!.Add(UpdatedInventoryLigne);
                        }
                    }
                }
                // si le status est validé on update les quantités des articles selon les inventaires
                if (UpdatedInventory.StatusInventory == Inventory.InventoryEnum.VALIDE)
                {
                    if (UpdatedInventory.InventoryLignes != null && UpdatedInventory.InventoryLignes.Any())
                    {
                        foreach (var UpdatedInventoryLigne in UpdatedInventory.InventoryLignes)
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
                var InventoryQuery = _context.Inventory
                    .Include(i => i.InventoryLignes!)
                        .ThenInclude(il => il.Stock)
                            .ThenInclude(s => s.Product)
                    .Where(po => po.StatusInventory == status);
                if (date.HasValue)
                {
                    InventoryQuery = InventoryQuery.Where(po => po.Date >= date.Value.Date);
                }
                var Inventories = InventoryQuery.OrderByDescending(po => po.Id).ToList();

                return Ok(Inventories);
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
                var Inventory = _context.Inventory
                    .Include(i => i.InventoryLignes) // Inclure les lignes d'inventaire
                    .SingleOrDefault(i => i.Id == id);

                if (Inventory == null)
                {
                    return BadRequest($"L'inventaire l'ID {id} est non trouvée");
                }
                if (Inventory.InventoryLignes != null && Inventory.InventoryLignes.Any())
                {
                    // Supprimer toutes les lignes d'inventaire associées
                    _context.InventoryLigne.RemoveRange(Inventory.InventoryLignes);
                }
                // Supprimer la commande fournisseur elle-même
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
