using ApiNegosud.DataAccess;
using ApiNegosud.Models;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiNegosud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderOrderController : ControllerBase
    {
        private readonly ConnexionDbContext _context;

        public ProviderOrderController(ConnexionDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Get(string  provider = null, DateOnly? date = null)
        {
            try
            {
                // Filtrer les commandes avec les champs de recherche, en castant en minuscules
                var providerOrders = _context.ProviderOrder
                    .Include(po => po.Provider)
                    .Include(po => po.ProviderOrderLines) // Inclure les lignes de commande fournisseur
                    .AsQueryable();

                if (!string.IsNullOrEmpty(provider))
                {
                    providerOrders = providerOrders.Where(po => po.Provider!.Name.ToLower() == provider.Trim().ToLower());
                }

                if (date.HasValue)
                {
                    // Filtrer par date 
                    providerOrders = providerOrders.Where(po => DateOnly.FromDateTime(po.Date) == date.Value);
                }

                providerOrders = providerOrders.OrderByDescending(c => c.Id);

                var filteredProviderOrders = providerOrders.ToList();

                if (filteredProviderOrders.Count == 0)
                {
                    return NotFound("Aucune commande fournisseur trouvée avec les paramètres fournis.");
                }
                else
                {
                    return Ok(filteredProviderOrders);
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
                var ProviderOrder = _context.ProviderOrder.Include(po => po.Provider).Include(po=>po.ProviderOrderLines).SingleOrDefault(po => po.Id == id);

                if (ProviderOrder == null)
                {
                    return NotFound($"La commande fournisseur avec l'ID {id} est non trouvé");
                }
                else
                {
                    return Ok(ProviderOrder);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("bystatus/{status}")]
        public IActionResult GetByStatus(ProviderOrderStatus status)
        {
            try
            { 
                var providerOrders = _context.ProviderOrder
                    .Include(po => po.Provider)
                         .ThenInclude(p => p.Address)
                    .Include(po => po.Provider!.Products)
                    .Include(po => po.ProviderOrderLines!)
                        .ThenInclude(line => line.Product)
                            .ThenInclude(p =>p.Stock)
                    .Where(po => po.ProviderOrderStatus == status)
                    .OrderByDescending(po => po.Id)
                    .ToList();
                return Ok(providerOrders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateProviderOrder")]
        public IActionResult CreateProviderOrder(ProviderOrder providerOrder)
        {
            try
            {
                _context.Add(providerOrder);
                // Ajouter les lignes de commande fournisseur associées
                if (providerOrder.ProviderOrderLines != null && providerOrder.ProviderOrderLines.Any())
                {
                    foreach (var orderLine in providerOrder.ProviderOrderLines)
                    {
                        // Assurer que la ligne est associée à la même commande fournisseur
                        orderLine.ProviderOrderId = providerOrder.Id;

                        // Vérifier si le produit appartient au même fournisseur que celui de la commande principale
                        var product = _context.Product
                            .Include(p => p.Provider)
                            .FirstOrDefault(p => p.Id == orderLine.ProductId);

                        if (product != null && product.ProviderId == providerOrder.ProviderId)
                        {
                            // Vérifier si une ligne pour ce produit existe déjà dans la commande fournisseur
                            var existingLine = _context.ProviderOrderLine
                                .FirstOrDefault(line => line.ProviderOrderId == providerOrder.Id
                                                        && line.ProductId == orderLine.ProductId && line.Id != orderLine.Id);

                            if (existingLine != null)
                            {
                                // Mise à jour de la quantité et du prix si la ligne existe déjà
                                existingLine.Quantity += orderLine.Quantity;
                                existingLine.Price += orderLine.Price;
                            }
                            else
                            {
                                // Ajouter une nouvelle ligne si elle n'existe pas déjà
                                _context.ProviderOrderLine.Add(orderLine);
                                _context.SaveChanges();
                            }
                        }
                        else
                        {
                            // Produit n'appartient pas au même fournisseur
                            return BadRequest("Le produit dans la ligne de commande fournisseur n'appartient pas au même fournisseur que la commande principale.");
                        }
                    }

                    _context.SaveChanges();
                }
                var newProviderOrder = _context.ProviderOrder.Include(p => p.ProviderOrderLines)
                            .FirstOrDefault(p => p.Id == providerOrder.Id);
                if (newProviderOrder != null)
                {
                    return Ok( new { newProviderOrder = newProviderOrder , Message= "La commande est ajoutée avec succès!"});
                }
                else
                {
                    return BadRequest("La commande n'a pas pu être récupérée après l'ajout.");
                }
            }
            catch (Exception ex)
            {
                // En cas d'erreur, renvoyer un message d'erreur
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateOrder")]
        public IActionResult UpdateOrder(ProviderOrder UpdatedProviderOrder)
        {
            try
            {
                var ExistingOrderProvider = _context.ProviderOrder.Include(po => po.Provider).FirstOrDefault(c => c.Id == UpdatedProviderOrder.Id);

                if (ExistingOrderProvider == null)
                {
                    return NotFound($"La commande avec l'ID {UpdatedProviderOrder.Id} n'a pas été trouvée");
                }

                // Mettre à jour les propriétés de la commande existante 
                ExistingOrderProvider.ProviderOrderStatus = UpdatedProviderOrder.ProviderOrderStatus;
                ExistingOrderProvider.Date = UpdatedProviderOrder.Date;
                ExistingOrderProvider.ProviderId = UpdatedProviderOrder.ProviderId;
                ExistingOrderProvider.Price = UpdatedProviderOrder.Price;

                // Vérifier les lignes de commande fournisseur associées
                if (UpdatedProviderOrder.ProviderOrderLines != null && UpdatedProviderOrder.ProviderOrderLines.Any())
                {
                    foreach (var updatedOrderLine in UpdatedProviderOrder.ProviderOrderLines)
                    {
                        // Assurer que la ligne est associée à la même commande fournisseur
                        updatedOrderLine.ProviderOrderId = UpdatedProviderOrder.Id;

                        // Vérifier si le produit appartient au même fournisseur que celui de la commande principale
                        var product = _context.Product
                            .Include(p => p.Provider)
                            .FirstOrDefault(p => p.Id == updatedOrderLine.ProductId);

                        if (product != null && product.ProviderId == UpdatedProviderOrder.ProviderId)
                        {
                            // Vérifier si une ligne pour ce produit existe déjà dans la commande fournisseur
                            var existingLine = _context.ProviderOrderLine
                                .FirstOrDefault(line => line.ProviderOrderId == UpdatedProviderOrder.Id
                                                        && line.ProductId == updatedOrderLine.ProductId);

                            if (existingLine != null)
                            {
                                // Mise à jour de la quantité et du prix si la ligne existe déjà
                                existingLine.Quantity = updatedOrderLine.Quantity;
                                existingLine.Price = updatedOrderLine.Price;
                            }
                            else
                            {
                                // Ajouter une nouvelle ligne si elle n'existe pas déjà
                                _context.ProviderOrderLine.Add(updatedOrderLine);
                            }
                        }
                        else
                        {
                            // Produit n'appartient pas au même fournisseur
                            return BadRequest("Le produit dans la ligne de commande fournisseur n'appartient pas au même fournisseur que la commande principale.");
                        }
                    }
                }

                _context.SaveChanges();

                // Si le statut est "LIVRE", mettre à jour la quantité du stock
                if (UpdatedProviderOrder.ProviderOrderStatus == ProviderOrderStatus.LIVRE)
                {
                    foreach (var updatedOrderLine in UpdatedProviderOrder.ProviderOrderLines!)
                    {
                        var stock = _context.Stock.FirstOrDefault(s => s.ProductId == updatedOrderLine.ProductId);
                        if (stock != null)
                        {
                            // Mettre à jour la quantité du stock
                            stock.Quantity += updatedOrderLine.Quantity;
                        }
                    }
                    _context.SaveChanges();
                }

                return Ok("Mise à jour réussie");
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
                var providerOrder = _context.ProviderOrder
                    .Include(po => po.ProviderOrderLines) // Inclure les lignes de commande fournisseur
                    .SingleOrDefault(po => po.Id == id);

                if (providerOrder == null)
                {
                    return BadRequest($"La commande fournisseur avec l'ID {id} est non trouvée");
                }

                // Vérifier si des lignes de commande fournisseur existent
                if (providerOrder.ProviderOrderLines != null && providerOrder.ProviderOrderLines.Any())
                {
                    // Supprimer toutes les lignes de commande fournisseur associées
                    _context.ProviderOrderLine.RemoveRange(providerOrder.ProviderOrderLines);
                }
                // Supprimer la commande fournisseur elle-même
                _context.ProviderOrder.Remove(providerOrder);

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
