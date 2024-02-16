
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
    public class StockController : ControllerBase
    {
        private readonly ConnexionDbContext _context;

        public StockController(ConnexionDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Get(string name = null)
        {
            try
            {
                var stocks = _context.Stock.Include(s => s.Product).ThenInclude(p => p.Family).AsQueryable();
              
                if (!string.IsNullOrEmpty(name))
                {
                    stocks = stocks.Where(s => s.Product!.Name.ToLower().Contains(name.Trim().ToLower()));
                }                
                var filtredProducts = stocks.ToList();
                if (filtredProducts.Count == 0)
                {
                    return NotFound("Aucun produit trouvé avec les paramètres fournis.");
                }
                else
                {
                    return Ok(filtredProducts);
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
                var stock = _context.Stock.Include(s => s.Product).SingleOrDefault(s => s.Id == id);
                if (stock == null)
                {
                    return NotFound($"Le stock avec l'ID {id} est non trouvé");
                }
                else
                {
                    return Ok(stock);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("getByProductId{productId}")]
        public IActionResult getByProductId(int productId)
        {
            try
            {
                var stock = _context.Stock.Include(s => s.Product).SingleOrDefault(s => s.ProductId == productId);
                if (stock == null)
                {
                    return NotFound($"Le stock avec l'ID {productId} est non trouvé");
                }
                else
                {
                    return Ok(stock);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult Post(Stock stock)
        {
            try
            {
                // Ajouter une validation pour AutoOrder, Maximum et Minimum
                if (stock.AutoOrder && (stock.Minimum == null || stock.Maximum == null || stock.Maximum ==0 || stock.Minimum==0))
                {
                    return BadRequest("Les valeurs Minimum et Maximum sont obligatoires et différents à 0 lorsque AutoOrder est true");
                }
                if (stock.ProductId > 0)
                {
                    var existingStock = _context.Stock.SingleOrDefault(s => s.ProductId == stock.ProductId);

                    if (existingStock != null)
                    {
                        return BadRequest("Un stock pour cet article existe déjà. Veuillez le mettre à jour au lieu d'en créer un nouveau.");
                    }

                    var existingProduct = _context.Product.Find(stock.ProductId);
                    if (existingProduct != null)
                    {
                        stock.Product = existingProduct;
                    }
                    else
                    {
                        return BadRequest("L'ID du produit fourni n'est pas valide");
                    }
                }
                else
                {
                    if (stock.Product != null)
                    {
                        var newProduct = new Product
                        {
                            Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(stock.Product.Name.Trim()),
                            Price = stock.Product.Price, 
                            Image = stock.Product.Image.Trim(),
                            Description = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(stock.Product.Description.Trim()),
                            DateProduction = stock.Product.DateProduction,
                            NbProductBox = stock.Product.NbProductBox,
                            Home = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(stock.Product.Home.Trim()),
                            FamilyId = stock.Product.FamilyId, 
                            ProviderId = stock.Product.ProviderId,
                        };
                        _context.Product.Add(newProduct);
                        stock.Product = newProduct;
              
                    }
                }
                _context.Add(stock);
                _context.SaveChanges();

                return Ok("Le stock du produit est ajouté avec succès!");
            }
            catch (Exception ex)
            {
                // En cas d'erreur, renvoyer un message d'erreur
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public IActionResult Put(Stock updatedStock)
        {
            try
            {
                var existingStock = _context.Stock.Include(s => s.Product).FirstOrDefault(s => s.Id == updatedStock.Id);

                if (existingStock == null)
                {
                    return NotFound($"Le stock du produit avec l'ID {updatedStock.Id} non trouvé.");
                }

                existingStock.Minimum = updatedStock.Minimum;
                existingStock.Maximum = updatedStock.Maximum;
                existingStock.Quantity = updatedStock.Quantity;
                existingStock.AutoOrder = updatedStock.AutoOrder;
                if (existingStock.AutoOrder && (existingStock.Minimum == null || existingStock.Maximum == null || existingStock.Minimum==0 || existingStock.Maximum == 0))
                {
                    return BadRequest("Les valeurs Minimum et Maximum sont obligatoires et différents à 0 lorsque AutoOrder est true.");
                }
                // Mettre à jour les propriétés du produit associé
                if (existingStock.Product != null && updatedStock.Product != null)
                {
                    existingStock.Product.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(updatedStock.Product.Name.Trim());
                    existingStock.Product.Price = updatedStock.Product.Price;
                    existingStock.Product.Image = updatedStock.Product.Image.Trim();
                    existingStock.Product.Description = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(updatedStock.Product.Description.Trim());
                    existingStock.Product.DateProduction = updatedStock.Product.DateProduction;
                    existingStock.Product.NbProductBox = updatedStock.Product.NbProductBox;
                    existingStock.Product.Home = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(updatedStock.Product.Home.Trim());
  
                }
                _context.SaveChanges();

                return Ok(existingStock);
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
                var stock = _context.Stock.Find(id);
                if (stock == null)
                {
                    return BadRequest("Produit non trouvé");
                }               
                _context.Stock.Remove(stock);
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
