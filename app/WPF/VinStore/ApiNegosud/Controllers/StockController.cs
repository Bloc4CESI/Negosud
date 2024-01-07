
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
        public IActionResult Get(string Name = null)
        {
            try
            {
                var Stocks = _context.Stock.Include(s => s.Product).AsQueryable();
              
                if (!string.IsNullOrEmpty(Name))
                {
                    Stocks = Stocks.Where(s => s.Product.Name.ToLower() == Name.Trim().ToLower());
                }                
                var FiltredProducts = Stocks.ToList();
                if (FiltredProducts.Count == 0)
                {
                    return NotFound("Aucun produit trouvé avec les paramètres fournis.");
                }
                else
                {
                    return Ok(FiltredProducts);
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
                var Stock = _context.Stock.Include(s => s.Product).SingleOrDefault(s => s.Id == id);

                if (Stock == null)
                {
                    return NotFound($"Le stock avec l'ID {id} est non trouvé");
                }
                else
                {
                    return Ok(Stock);
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
                var Stock = _context.Stock.Include(s => s.Product).SingleOrDefault(s => s.ProductId == productId);

                if (Stock == null)
                {
                    return NotFound($"Le stock avec l'ID {productId} est non trouvé");
                }
                else
                {
                    return Ok(Stock);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult Post(Stock Stock)
        {
            try
            {
                Stock.Minimum = Stock.Minimum ?? null;
                Stock.Maximum = Stock.Maximum ?? null;
                if (Stock.ProductId > 0)
                {
                    var ExistingProduct = _context.Product.Find(Stock.ProductId);
                    if (ExistingProduct != null)
                    {
                        Stock.Product = ExistingProduct;
                    }
                    else
                    {
                        return BadRequest("L'ID du produit fourni n'est pas valide");
                    }
                }
                else
                {
                    if (Stock.Product != null)
                    {

                        var NewProduct = new Product
                        {
                            Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Stock.Product.Name.Trim()),
                            Price = Stock.Product.Price, 
                            Image = Stock.Product.Image.Trim(),
                            Description = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Stock.Product.Description.Trim()),
                            DateProduction = Stock.Product.DateProduction,
                            NbProductBox = Stock.Product.NbProductBox,
                            Home = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Stock.Product.Home.Trim()),
                            FamilyId = Stock.Product.FamilyId, 
                            ProviderId = Stock.Product.ProviderId,
                        };
                        _context.Product.Add(NewProduct);
                        Stock.Product = NewProduct;
              
                    }

                }
                _context.Add(Stock);
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
        public IActionResult Put(Stock UpdatedStock)
        {
            try
            {
                var ExistingStock = _context.Stock.Include(s => s.Product).FirstOrDefault(s => s.Id == UpdatedStock.Id);

                if (ExistingStock == null)
                {
                    return NotFound($"Le stock du produit avec l'ID {UpdatedStock.Id} non trouvé.");
                }
                ExistingStock.Minimum = UpdatedStock.Minimum;
                ExistingStock.Maximum = UpdatedStock.Maximum;
                ExistingStock.Quantity = UpdatedStock.Quantity;
                ExistingStock.AutoOrder = UpdatedStock.AutoOrder;
                // Mettre à jour les propriétés du produit associé
                if (ExistingStock.Product != null && UpdatedStock.Product != null)
                {
                    ExistingStock.Product.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(UpdatedStock.Product.Name.Trim());
                    ExistingStock.Product.Price = UpdatedStock.Product.Price;
                    ExistingStock.Product.Image = UpdatedStock.Product.Image.Trim();
                    ExistingStock.Product.Description = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(UpdatedStock.Product.Description.Trim());
                    ExistingStock.Product.DateProduction = UpdatedStock.Product.DateProduction;
                    ExistingStock.Product.NbProductBox = UpdatedStock.Product.NbProductBox;
                    ExistingStock.Product.Home = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(UpdatedStock.Product.Home.Trim());
  
                }
                _context.SaveChanges();

                return Ok(ExistingStock);
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
                var Stock = _context.Stock.Find(id);
                if (Stock == null)
                {
                    return BadRequest("Produit non trouvé");
                }
                
                _context.Stock.Remove(Stock);
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
