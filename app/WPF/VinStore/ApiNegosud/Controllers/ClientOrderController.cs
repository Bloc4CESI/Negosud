using ApiNegosud.DataAccess;
using ApiNegosud.Models;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiNegosud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientOrderController : ControllerBase
    {
        private readonly ConnexionDbContext _context;

        public ClientOrderController(ConnexionDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetOrdersByClient/{ClientId}")]
        public IActionResult GetOrdersByClient(int ClientId)
        {
            try
            {           
                var ordersClientQuery = _context.ClientOrder.Include(co => co.ClientOrderLines!)
                    .ThenInclude(line => line.Product)
                           .ThenInclude(p => p.Stock)
                    .Where(co => co.ClientId == ClientId).ToList();

                var clientOrders = ordersClientQuery.OrderByDescending(oc => oc.Id).ToList();

                if (clientOrders == null)
                {
                    return NotFound($"Aucune commande trouvée pour le client.");
                }
                else
                {
                    return Ok(clientOrders);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("Order/{ClientId}")]
        public IActionResult Order(int ClientId)
        {
            try
            {
                var clientOrder = _context.ClientOrder
                    .Include(co => co.ClientOrderLines)
                    .SingleOrDefault(co => co.ClientId == ClientId && co.OrderStatus == OrderStatus.ENCOURSDEVALIDATION);

                if (clientOrder == null)
                {
                    return NotFound($"Aucune commande trouvée pour le client avec l'ID {ClientId}.");
                }

                // mettre à jour le prix total de la commande en fonction des lignes de commande
                clientOrder.Price = clientOrder.ClientOrderLines!.Sum(col => col.Price);

                // Changer le statut de la commande à "VALIDE" après avoir commandé
                clientOrder.OrderStatus = OrderStatus.VALIDE;
                // Sauvegarder les changements dans la base de données
                _context.SaveChanges();
                // Structure pour regrouper les informations par fournisseur et produit
                Dictionary<int, Dictionary<int, int>> providerProductQuantities = new Dictionary<int, Dictionary<int, int>>();
                // Mettre à jour la quantité des produits dans le stock
                foreach (var clientOrderLine in clientOrder.ClientOrderLines!)
                {
                    var product = _context.Product
                        .Include(p => p.Stock)
                        .FirstOrDefault(p => p.Id == clientOrderLine.ProductId);

                    if (product != null && product.Stock != null)
                    {
                        // Déduire la quantité commandée du stock
                        product.Stock.Quantity -= clientOrderLine.Quantity;
                        if (product.Stock.AutoOrder &&
                             product.Stock.Minimum.HasValue && (product.Stock.Quantity < clientOrderLine.Quantity || product.Stock.Minimum < clientOrderLine.Quantity)
                             && product.Stock.Maximum.HasValue && product.Stock.Minimum.HasValue)
                        {
                            // Calculer la quantité à commander
                            var quantityToOrder = product.Stock.Maximum.Value - product.Stock.Minimum.Value;
                            if (!providerProductQuantities.ContainsKey(product.ProviderId))
                            {
                                providerProductQuantities[product.ProviderId] = new Dictionary<int, int>();
                            }
                            if (!providerProductQuantities[product.ProviderId].ContainsKey(product.Id))
                            {
                                providerProductQuantities[product.ProviderId][product.Id] = quantityToOrder;
                            }
                        }
                    }
                }
                foreach (var providerEntry in providerProductQuantities)
                {
                    var providerId = providerEntry.Key;
                    var productsQuantities = providerEntry.Value;
                    // Vérifier s'il existe déjà une commande fournisseur en cours de validation pour ce fournisseur
                    var existingProviderOrder = _context.ProviderOrder.FirstOrDefault(po => po.ProviderId == providerId && po.ProviderOrderStatus == ProviderOrderStatus.ENCOURSDEVALIDATION);
                    // Si aucune commande fournisseur n'existe, créer une nouvelle commande fournisseur
                    if (existingProviderOrder == null)
                    {                        
                        var providerOrder = new ProviderOrder
                        {
                            Date = DateTime.Now,
                            Price = 0, // Le prix total sera mis à jour plus tard
                            ProviderOrderStatus = ProviderOrderStatus.ENCOURSDEVALIDATION,
                            ProviderId = providerId
                        };

                        _context.ProviderOrder.Add(providerOrder);
                        _context.SaveChanges();
                        existingProviderOrder = providerOrder;
                    }

                    foreach (var productQuantity in productsQuantities)
                    {
                        var productId = productQuantity.Key;
                        var quantity = productQuantity.Value;
                        // Vérifier si une ligne de commande fournisseur existe déjà pour ce produit
                        var existingProviderOrderLine = _context.ProviderOrderLine
                            .FirstOrDefault(pol => pol.ProductId == productId && pol.ProviderOrderId == existingProviderOrder.Id);
                        if (existingProviderOrderLine == null)
                        {
                            // Ajouter une ligne de commande fournisseur pour chaque produit insuffisant
                            var providerOrderLine = new ProviderOrderLine
                            {
                                Quantity = quantity,
                                Price = (_context.Product.FirstOrDefault(p => p.Id == productId)!.Price * (1m / 3m)), // 1/3 du prix produit
                                ProductId = productId,
                                ProviderOrderId = existingProviderOrder.Id
                            };
                            _context.ProviderOrderLine.Add(providerOrderLine);
                            _context.SaveChanges();
                        }
                    }
                    // Mettre à jour le prix total de la commande fournisseur en fonction des lignes de commande fournisseur
                    existingProviderOrder.Price = _context.ProviderOrderLine.Where(col => col.ProviderOrderId == existingProviderOrder.Id).Sum(col => col.Price * col.Quantity);
                    // Sauvegarder les changements dans la base de données
                    _context.SaveChanges();
                }
                return Ok("Commande passée avec succès! Des commandes fournisseurs ont été créées pour les produits insuffisants.");
            }            
            catch (Exception ex)
            {
                return BadRequest($"Une erreur s'est produite lors de la commande. Détails de l'erreur : {ex.Message}");
            }
        }

        [HttpGet("bystatus/{status}")]
        public IActionResult GetByStatus(OrderStatus status, DateTime? date = null)
        {
            try
            {
                var clientOrderQuery = _context.ClientOrder
                    .Include(co => co.Client)
                        .ThenInclude(c => c.Address)
                    .Include(co => co.ClientOrderLines!)
                        .ThenInclude(line => line.Product)
                            .ThenInclude(p => p.Stock)
                    .Where(co => co.OrderStatus == status);
                if (date.HasValue)
                {
                    clientOrderQuery = clientOrderQuery.Where(co => co.Date >= date.Value.Date);
                }              
                 var clientOrders = clientOrderQuery.OrderByDescending(po => po.Id).ToList();

                return Ok(clientOrders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("UpdateOrder")]
        public IActionResult UpdateOrder(ClientOrder updatedClientOrder)
        {
            try
            {
                // Rechercher la commande existante par ID
                var existingOrder = _context.ClientOrder
                    .Include(co => co.ClientOrderLines)
                    .FirstOrDefault(co => co.Id == updatedClientOrder.Id);
                if (existingOrder == null)
                {
                    // Si aucune commande existante n'est trouvée avec cet ID, retourner un NotFound
                    return NotFound($"Commande avec ID {updatedClientOrder.Id} non trouvée.");
                }

                // Mettre à jour les champs de la commande existante avec ceux de UpdatedClientOrder
                existingOrder.OrderStatus = updatedClientOrder.OrderStatus;
                existingOrder.Price = updatedClientOrder.Price;
                if (updatedClientOrder.ClientOrderLines != null)
                {
                    foreach (var updatedLine in updatedClientOrder.ClientOrderLines)
                    {
                        var existingLine = existingOrder.ClientOrderLines!
                            .FirstOrDefault(col => col.Id == updatedLine.Id);

                        if (existingLine != null)
                        {
                            // Mise à jour de la ligne existante
                            existingLine.ProductId = updatedLine.ProductId;
                            existingLine.Quantity = updatedLine.Quantity;
                            existingLine.Price = updatedLine.Price;
                        }
                        if (updatedClientOrder.OrderStatus == OrderStatus.REFUSE)
                        {
                            var productStock = _context.Stock.FirstOrDefault(s => s.ProductId == updatedLine.ProductId);
                            if (productStock != null)
                            {
                                productStock.Quantity += updatedLine.Quantity;
                            }
                        }
                     }
                }
                _context.SaveChanges();

                return Ok($"Commande avec ID {updatedClientOrder.Id} mise à jour avec succès.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erreur lors de la mise à jour de la commande: {ex.Message}");
            }
        }
        [HttpGet("GetOrderClientById/{idOrderClient}")]
        public IActionResult GetOrderClientById(int idOrderClient)
        {
            try
            {
                var clientOrder = _context.ClientOrder
                    .Include(co => co.Client!)
                        .ThenInclude(c => c.Address)
                    .Include(co => co.ClientOrderLines!)
                        .ThenInclude(line => line.Product)
                            .ThenInclude(p => p.Stock)
                    .FirstOrDefault(co => co.Id == idOrderClient); 
                if (clientOrder == null)
                {
                    return NotFound($"Commande avec l'ID {idOrderClient} non trouvée.");
                }

                return Ok(clientOrder);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erreur lors de la récupération de la commande : {ex.Message}");
            }
        }
    }
    

}
