using ApiNegosud.DataAccess;
using ApiNegosud.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiNegosud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderOrderLineController : ControllerBase
    {
        private readonly ConnexionDbContext _context;

        public ProviderOrderLineController(ConnexionDbContext context)
        {
            _context = context;
        }
        [HttpPost("CreateProviderOrderLine")]
        public IActionResult CreateProviderOrderLine(ProviderOrderLine providerOrderLine)
        {
            try
            {
                // Vérifier si le produit appartient au même fournisseur que celui de la commande principale
                var product = _context.Product
                    .Include(p => p.Provider)
                    .FirstOrDefault(p => p.Id == providerOrderLine.ProductId);

                var order = _context.ProviderOrder
                    .Include(o => o.Provider)
                    .FirstOrDefault(o => o.Id == providerOrderLine.ProviderOrderId);

                if (product != null && order != null && product.ProviderId == order.ProviderId)
                {
                    // Vérifier si une ligne pour ce produit existe déjà dans la commande fournisseur
                    var existingLine = _context.ProviderOrderLine
                        .FirstOrDefault(line => line.ProviderOrderId == providerOrderLine.ProviderOrderId
                                               && line.ProductId == providerOrderLine.ProductId);

                    if (existingLine != null)
                    {
                        // Mise à jour de la quantité et du prix si la ligne existe déjà
                        existingLine.Quantity += providerOrderLine.Quantity;
                        existingLine.Price += providerOrderLine.Price;
                    }
                    else
                    {
                        // Ajouter une nouvelle ligne si elle n'existe pas déjà
                        _context.ProviderOrderLine.Add(providerOrderLine);
                    }

                    _context.SaveChanges();

                    return Ok("La ligne de commande fournisseur est ajoutée avec succès!");
                }
                else
                {
                    // Produit n'appartient pas au même fournisseur
                    return BadRequest("Le produit dans la ligne de commande fournisseur n'appartient pas au même fournisseur que la commande principale.");
                }
            }
            catch (Exception ex)
            {
                // En cas d'erreur, renvoyer un message d'erreur
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetLinesByOrderId/{orderId}")]
        public IActionResult GetLinesByOrderId(int orderId)
        {
            try
            {
                var providerOrderLines = _context.ProviderOrderLine
                    .Where(line => line.ProviderOrderId == orderId)
                    .ToList();

                if (providerOrderLines.Count == 0)
                {
                    return NotFound($"Aucune ligne de commande fournisseur trouvée pour la commande fournisseur avec l'ID {orderId}");
                }
                else
                {
                    return Ok(providerOrderLines);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("UpdateOrderLine")]
        public IActionResult UpdateOrderLine(ProviderOrderLine updatedOrderLine)
        {
            try
            {
                // Vérifier si la ligne de commande fournisseur existe
                var existingLine = _context.ProviderOrderLine
                    .Include(line => line.ProviderOrder)
                    .FirstOrDefault(line => line.Id == updatedOrderLine.Id);

                if (existingLine == null)
                {
                    return NotFound($"La ligne de commande fournisseur avec l'ID {updatedOrderLine.Id} n'a pas été trouvée");
                }

                // Vérifier si le produit appartient au même fournisseur que celui de la commande principale
                var product = _context.Product
                    .Include(p => p.Provider)
                    .FirstOrDefault(p => p.Id == updatedOrderLine.ProductId);

                if (product != null && product.ProviderId == existingLine.ProviderOrder!.ProviderId)
                {
                    // Mise à jour de la quantité et du prix
                    existingLine.Quantity = updatedOrderLine.Quantity;
                    existingLine.Price = updatedOrderLine.Price;

                    _context.SaveChanges();

                    return Ok("Mise à jour de la ligne de commande fournisseur réussie!");
                }
                else
                {
                    // Produit n'appartient pas au même fournisseur
                    return BadRequest("Le produit dans la ligne de commande fournisseur n'appartient pas au même fournisseur que la commande principale.");
                }
            }
            catch (Exception ex)
            {
                // En cas d'erreur, renvoyer un message d'erreur
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteProviderOrderLine/{id}")]
        public IActionResult DeleteProviderOrderLine(int id)
        {
            try
            {
                var orderLineToDelete = _context.ProviderOrderLine.Find(id);

                if (orderLineToDelete == null)
                {
                    return NotFound($"La ligne de commande fournisseur avec l'ID {id} n'a pas été trouvée");
                }

                _context.ProviderOrderLine.Remove(orderLineToDelete);
                _context.SaveChanges();

                return Ok("Suppression de la ligne de commande fournisseur réussie!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
