using System.Globalization;
using ApiNegosud.DataAccess;
using ApiNegosud.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;

namespace ApiNegosud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientOrderLineController : ControllerBase
    {
        private readonly ConnexionDbContext _context;

        public ClientOrderLineController(ConnexionDbContext context)
        {
            _context = context;
        }
     

        [HttpGet("GetClientCart/{ClientId}")]
        public IActionResult GetClientCart(int ClientId)
        {
            try
            {
                var clientCart = _context.ClientOrderLine
                    .Include(co =>co.ClientOrder)
                    .Include(col => col.Product)
                     .ThenInclude(p => p.Stock)
                    .Where(col => col.ClientOrder!.ClientId == ClientId && col.ClientOrder.OrderStatus == OrderStatus.ENCOURSDEVALIDATION)
                    .ToList();

                if (clientCart == null || !clientCart.Any())
                {
                    return NotFound($"Aucun produit trouvé dans le panier du client avec l'ID {ClientId}.");
                }
                else
                {
                    return Ok(clientCart);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Une erreur s'est produite lors de la récupération du panier du client avec l'ID {ClientId}. Détails de l'erreur : {ex.Message}");
            }
        }
        [HttpPost("AddProductToCart")]
        public IActionResult AddProductToCart(int productId, int clientId, int quantity, Decimal price)
        {
            try
            {
                // Vérifier si la commande du client existe
                var clientOrder = _context.ClientOrder.FirstOrDefault(co => co.ClientId == clientId && co.OrderStatus == OrderStatus.ENCOURSDEVALIDATION);
                if (clientOrder == null)
                {
                    // Si la commande n'existe pas, créez une nouvelle commande pour le client
                    clientOrder = new ClientOrder
                    {
                        ClientId = clientId,
                        Date = DateTime.Now,
                        Price = 0,
                        OrderStatus = OrderStatus.ENCOURSDEVALIDATION
                    };

                    _context.ClientOrder.Add(clientOrder);
                    _context.SaveChanges();
                }
                // Vérifier si le produit existe déjà dans la commande
                var existingOrderLine = _context.ClientOrderLine
                    .FirstOrDefault(col => col.ClientOrderId == clientOrder.Id && col.ProductId == productId);

                if (existingOrderLine != null)
                {
                    // Si le produit existe déjà, mettez à jour la quantité
                    existingOrderLine.Quantity += quantity;
                    existingOrderLine.Price += price; // Update the total price if needed
                }
                else
                {
                    // Ajouter le produit à la ligne de commande du client
                    var clientOrderLine = new ClientOrderLine
                    {
                    ClientOrder = clientOrder,
                    ClientOrderId = clientOrder.Id,
                    ProductId = productId,
                    // Ps: n'oubliez pas à multiplier * NbProductBox en cas de paquet (front)
                    Quantity = quantity,
                    Price = price
                    };   
                    _context.ClientOrderLine.Add(clientOrderLine);
                    // Ajuster le prix total de la commande en ajoutant le prix de cette nouvelle ligne
                    clientOrder.Price += clientOrderLine.Price;
                }          
                _context.SaveChanges();

                return Ok("La ligne de la commande est ajoutée avec succès!");
            }
            catch (Exception ex)
            {
                // En cas d'erreur, renvoyer un message d'erreur
                return BadRequest(ex.Message);
            }
        }

    [HttpPut("UpdateCartProductQuantity")]
    public  IActionResult UpdateCartProductQuantity(int clientOrderLineId, int newQuantity, int newPrice)
    {
        try
        {
            var clientOrderLine = _context.ClientOrderLine.Find(clientOrderLineId);

            if (clientOrderLine != null)
            {
                clientOrderLine.Quantity = newQuantity;
                clientOrderLine.Price = newPrice;
                _context.SaveChanges();
            }
            return Ok("La quantité et le prix de l'article sont modifiés!");
        }
        catch (Exception ex)
        {

            return BadRequest(ex.Message);
        }
    }

        [HttpDelete("RemoveProductFromCart/{ClientOrderLineId}")]
        public IActionResult RemoveProductFromCart(int ClientOrderLineId)
        {
            try
            {
                var clientOrderLine = _context.ClientOrderLine.Find(ClientOrderLineId);

                if (clientOrderLine != null)
                {
                    _context.ClientOrderLine.Remove(clientOrderLine);
                    _context.SaveChanges();

                    // Vérifier si la commande n'a plus de lignes de commande, puis supprimez la commande
                    var clientOrder = _context.ClientOrder
                        .Include(co => co.ClientOrderLines)
                        .SingleOrDefault(co => co.Id == clientOrderLine.ClientOrderId);

                    if (clientOrder != null && !clientOrder.ClientOrderLines.Any())
                    {
                        _context.ClientOrder.Remove(clientOrder);
                        _context.SaveChanges();
                    }

                    return Ok("Suppression réussie!");
                }else
                {
                    return NotFound($"Aucune ligne de commande trouvée avec l'ID {ClientOrderLineId}.");
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
