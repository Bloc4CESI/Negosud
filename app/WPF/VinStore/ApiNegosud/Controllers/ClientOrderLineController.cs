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
                    .Include(col => col.Product)
                    .Where(col => col.ClientOrder.ClientId == ClientId && col.ClientOrder.OrderStatus == OrderStatus.ENCOURSDEVALIDATION)
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
        public IActionResult AddProductToCart(int ProductId, int ClientId, int Quantity, Decimal Price)
        {
            try
            {
                // Vérifier si la commande du client existe
                var ClientOrder = _context.ClientOrder.FirstOrDefault(co => co.ClientId == ClientId && co.OrderStatus == OrderStatus.ENCOURSDEVALIDATION);
                if (ClientOrder == null)
                {
                    // Si la commande n'existe pas, créez une nouvelle commande pour le client
                    ClientOrder = new ClientOrder
                    {
                        ClientId = ClientId,
                        Date = DateTime.Now,
                        // la primere fois c'est le prix de la ligne => Ps: n'oubliez pas à multiplier * NbProductBox -20% en cas de paquet
                        Price = Price,
                        OrderStatus = OrderStatus.ENCOURSDEVALIDATION
                    };

                    _context.ClientOrder.Add(ClientOrder);
                    _context.SaveChanges();
                }
                    // Vérifier si le produit existe déjà dans la commande
                    var existingOrderLine = _context.ClientOrderLine
                        .FirstOrDefault(col => col.ClientOrderId == ClientOrder.Id && col.ProductId == ProductId);

                    if (existingOrderLine != null)
                    {
                        // Si le produit existe déjà, mettez à jour la quantité
                        existingOrderLine.Quantity += Quantity;
                        existingOrderLine.Price += Price; // Update the total price if needed
                    }
                    else
                    {
                        // Ajouter le produit à la ligne de commande du client
                        var clientOrderLine = new ClientOrderLine
                        {
                        ClientOrder = ClientOrder,
                        ClientOrderId = ClientOrder.Id,
                        ProductId = ProductId,
                        // Ps: n'oubliez pas à multiplier * NbProductBox en cas de paquet (front)
                        Quantity = Quantity,
                        Price = Price
                        };   
                        _context.ClientOrderLine.Add(clientOrderLine);
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
    public  IActionResult UpdateCartProductQuantity(int ClientOrderLineId, int NewQuantity, int NewPrice)
    {
        try
        {
            var clientOrderLine = _context.ClientOrderLine.Find(ClientOrderLineId);

            if (clientOrderLine != null)
            {
                clientOrderLine.Quantity = NewQuantity;
                clientOrderLine.Price = NewPrice;
                _context.SaveChanges();
            }
            return Ok("La quantité et le prix de l'article sont modifiés!");
        }
        catch (Exception ex)
        {

            return BadRequest(ex.Message);
        }
    }

        [HttpDelete("RemoveProductFromCart{ClientOrderLineId}")]
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
