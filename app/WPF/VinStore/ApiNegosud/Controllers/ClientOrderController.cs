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
                var OrdersClient = _context.ClientOrder.Include(co => co.ClientOrderLines!)
                    .ThenInclude(line => line.Product)
                           .ThenInclude(p => p.Stock)
                    .Where(co => co.ClientId == ClientId).ToList();

                if (OrdersClient == null)
                {
                    return NotFound($"Aucune commande trouvée pour le client.");
                }
                else
                {
                    return Ok(OrdersClient);
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
                var ClientOrder = _context.ClientOrder
                    .Include(co => co.ClientOrderLines)
                    .SingleOrDefault(co => co.ClientId == ClientId && co.OrderStatus == OrderStatus.ENCOURSDEVALIDATION);

                if (ClientOrder == null)
                {
                    return NotFound($"Aucune commande trouvée pour le client avec l'ID {ClientId}.");
                }

                // mettre à jour le prix total de la commande en fonction des lignes de commande
                ClientOrder.Price = ClientOrder.ClientOrderLines.Sum(col => col.Price);

                // Changer le statut de la commande à "VALIDE" après avoir commandé
                ClientOrder.OrderStatus = OrderStatus.VALIDE;
                // Sauvegarder les changements dans la base de données
                _context.SaveChanges();
                // Utiliser un Dictionary pour stocker les informations de quantité manquante par fournisseur
                var quantityMissingByProvider = new Dictionary<int, (int QuantityMissing, List<int> ProductIds)>();
                // Mettre à jour la quantité des produits dans le stock
                foreach (var clientOrderLine in ClientOrder.ClientOrderLines)
                {
                    var product = _context.Product
                        .Include(p => p.Stock)
                        .FirstOrDefault(p => p.Id == clientOrderLine.ProductId);

                    if (product != null && product.Stock != null)
                    {
                        // Déduire la quantité commandée du stock
                        product.Stock.Quantity -= clientOrderLine.Quantity;
                        if (product.Stock.AutoOrder &&
                             product.Stock.Minimum.HasValue && product.Stock.Minimum.Value < clientOrderLine.Quantity &&
                             product.Stock.Maximum.HasValue && product.Stock.Minimum.HasValue)
                        {
                            // Calculer la quantité manquante
                            var quantityMissing = product.Stock.Maximum.Value - product.Stock.Minimum.Value;
                            // Vérifier si le fournisseur existe déjà dans le Dictionary
                            if (quantityMissingByProvider.TryGetValue(product.ProviderId, out var existingInfo))
                            {
                                // Ajouter la quantité manquante et l'ID du produit au Dictionary existant
                                existingInfo.QuantityMissing += quantityMissing;
                                existingInfo.ProductIds.Add(product.Id);
                            }
                            else
                            {
                                // Créer une nouvelle entrée pour le fournisseur dans le Dictionary
                                var newInfo = (QuantityMissing: quantityMissing, ProductIds: new List<int> { product.Id });
                                quantityMissingByProvider.Add(product.ProviderId, newInfo);
                            }
                        }
                    }
                }
                foreach (var (providerId, info) in quantityMissingByProvider)
                {
                    // Vérifier s'il existe déjà une commande fournisseur en cours de validation pour ce fournisseur
                    var existingProviderOrder = _context.ProviderOrder
                        .FirstOrDefault(po => po.ProviderId == providerId && po.ProviderOrderStatus == ProviderOrderStatus.ENCOURSDEVALIDATION);

                    if (existingProviderOrder == null)
                    {
                        // Si aucune commande fournisseur n'existe, créer une nouvelle commande fournisseur
                        var providerOrder = new ProviderOrder
                        {
                            Date = DateTime.Now,
                            Price = 0, // Le prix total sera mis à jour plus tard
                            ProviderOrderStatus = ProviderOrderStatus.ENCOURSDEVALIDATION,
                            ProviderId = providerId
                        };

                        _context.ProviderOrder.Add(providerOrder);
                        _context.SaveChanges();  // Sauvegarder pour générer l'ID de la commande fournisseur
                        existingProviderOrder = providerOrder; // Set the existingProviderOrder to the newly created order
                    }


                    foreach (var productId in info.ProductIds)
                    {
                        // Vérifier si une ligne de commande fournisseur existe déjà pour ce produit
                        var existingProviderOrderLine = _context.ProviderOrderLine
                            .FirstOrDefault(pol => pol.ProductId == productId && pol.ProviderOrderId == existingProviderOrder.Id);

                        /*if (existingProviderOrderLine != null)
                        {
                            // Si une ligne de commande fournisseur existe, mettre à jour la quantité
                            existingProviderOrderLine.Quantity += info.QuantityMissing;
                            existingProviderOrder.Price +=  (_context.Product.FirstOrDefault(p => p.Id == productId)!.Price * (1m / 3m)) * info.QuantityMissing;

                        }*/
                        if (existingProviderOrderLine == null)
                        {
                            // Ajouter une ligne de commande fournisseur pour chaque produit insuffisant
                            var providerOrderLine = new ProviderOrderLine
                            {
                                Quantity = info.QuantityMissing,
                                Price = (_context.Product.FirstOrDefault(p => p.Id == productId)!.Price * (1m / 3m)) * info.QuantityMissing, // 1/3 du prix produit
                                ProductId = productId,
                                ProviderOrderId = existingProviderOrder.Id

                            };

                            _context.ProviderOrderLine.Add(providerOrderLine);
                            _context.SaveChanges();  // Sauvegarder chaque ligne de commande fournisseur individuellement
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
        public IActionResult UpdateOrder(ClientOrder UpdatedClientOrder)
        {
            try
            {
                // Rechercher la commande existante par ID
                var existingOrder = _context.ClientOrder
                    .Include(co => co.ClientOrderLines)
                    .FirstOrDefault(co => co.Id == UpdatedClientOrder.Id);

                if (existingOrder == null)
                {
                    // Si aucune commande existante n'est trouvée avec cet ID, retourner un NotFound
                    return NotFound($"Commande avec ID {UpdatedClientOrder.Id} non trouvée.");
                }

                // Mettre à jour les champs de la commande existante avec ceux de UpdatedClientOrder
                existingOrder.OrderStatus = UpdatedClientOrder.OrderStatus;
                existingOrder.Price = UpdatedClientOrder.Price;
                if (UpdatedClientOrder.ClientOrderLines != null)
                {
                    foreach (var updatedLine in UpdatedClientOrder.ClientOrderLines)
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
                    }
                }
                _context.SaveChanges();

                return Ok($"Commande avec ID {UpdatedClientOrder.Id} mise à jour avec succès.");
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
