﻿using ApiNegosud.DataAccess;
using ApiNegosud.Models;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Http.Extensions;
using System.Linq;

namespace ApiNegosud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ConnexionDbContext _context;

        public ProductController(ConnexionDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Get(int? page = null, int? pageSize = null, int? idFamily = null, string Home = null, string Name = null,
                           DateOnly? dateProduction = null, Decimal? Price= null, string sortOrder = null)
        {
            try
            {
                var products = _context.Product.Include(f => f.Family).Include(p => p.Stock).Include(p => p.Provider).AsQueryable();
                if (idFamily.HasValue)
                {
                    products = products.Where(p => p.FamilyId == idFamily.Value);
                }
                if (!string.IsNullOrEmpty(Home))
                {
                    products = products.Where(c => c.Home.ToLower() == Home.Trim().ToLower());
                }
                if (!string.IsNullOrEmpty(Name))
                {
                    products = products.Where(c => c.Name.ToLower().Contains(Name.Trim().ToLower()));
                }
                if (dateProduction.HasValue)
                {
                    products = products.Where(c => c.DateProduction == dateProduction.Value);
                }
                if (Price.HasValue)
                {
                    products = products.Where(c => c.Price == Price.Value);
                }

                switch (sortOrder)
                {
                    case "price_asc":
                        products = products.OrderBy(c => c.Price);
                        break;
                    case "price_desc":
                        products = products.OrderByDescending(c => c.Price);
                        break;
                    default:
                        products = products.OrderByDescending(c => c.Id);
                        break;
                }
                if (page.HasValue && pageSize.HasValue)
                {
                    products = products.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }
                var filtredProducts = products.ToList();
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
        [HttpGet("GetAlertProduct")]
        public IActionResult GetAlertProduct()
        {
            try
            {
                var Products = _context.Product.Include(f => f.Family).Include(p => p.Stock).Include(p => p.Provider).AsQueryable();
                Products = Products.Where(p => p.Stock != null &&
                                      (p.Stock.Minimum.HasValue ? p.Stock.Quantity < p.Stock.Minimum.Value : p.Stock.Quantity < 4));

                var FiltredProducts = Products.ToList();
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
                var Product = _context.Product
                    .Include(p => p.Family)
                    .Include(p => p.Stock)
                    .Include(p => p.Provider)
                    .SingleOrDefault(p => p.Id == id);

                if (Product == null)
                {
                    return NotFound($"Le produit avec l'ID {id} est non trouvé");
                }
                else
                {
                    return Ok(Product);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult Post(Product Product)
        {
            try
            {
                // Capitaliser la première lettre des noms
                Product.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Product.Name.Trim());
                Product.Home = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Product.Home.Trim());
                if (Product.FamilyId > 0)
                {
                    var ExistingFamily = _context.Family.Find(Product.FamilyId);
                    if (ExistingFamily != null )
                    {
                        Product.Family = ExistingFamily;
                    }
                    else
                    {
                        return BadRequest("L'ID de la famille fournie n'est pas valide");
                    }
                }
                else 
                {
                    if(Product.Family!= null)
                    {
                        // verifier si la famille existe déja ( name famille unique)
                        var ExistingFamily = _context.Family.FirstOrDefault(f => f.Name.ToLower() == Product.Family.Name.Trim().ToLower());
                        if (ExistingFamily != null)
                        {
                            // si elle existe on utilise le me nom sans ajouter
                            Product.Family = ExistingFamily;
                        }
                        else
                        {
                            //sinon on va creer une nouvelle famille par l'jout du produit
                            var NewFamily = new Family
                            {
                                Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Product.Family.Name.Trim())
                            };
                            _context.Family.Add(NewFamily);
                            Product.Family = NewFamily;
                        }
                    }
                   
                }
                if(Product.Stock != null) {
                    var stock = new Stock
                    {
                        Quantity = Product.Stock.Quantity,
                        AutoOrder = Product.Stock.AutoOrder,
                        Minimum = Product.Stock.Minimum,
                        Maximum = Product.Stock.Maximum
                    };
                    //  pour AutoOrder => max et min soient obligatoires
                    if (stock.AutoOrder && (stock.Minimum == null || stock.Maximum == null || stock.Maximum == 0 || stock.Minimum == 0))
                    {
                        return BadRequest("Les valeurs Minimum et Maximum sont obligatoires lorsque AutoOrder est true.");
                    }

                }
                _context.Add(Product);
                _context.SaveChanges();

                return Ok("Le produit est ajouté avec succès!");
            }
            catch (Exception ex)
            {
                // En cas d'erreur, renvoyer un message d'erreur
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public IActionResult Put( Product updatedProduct)
        {
            try
            {
                var existingProduct = _context.Product
                    .Include(p => p.Family)
                    .Include(p => p.Stock)
                    .FirstOrDefault(p => p.Id == updatedProduct.Id);

                if (existingProduct == null)
                {
                    return NotFound($"Produit avec l'ID {updatedProduct.Id} non trouvé.");
                }
                // Update the existing product properties
                existingProduct.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(updatedProduct.Name.Trim());
                existingProduct.Price = updatedProduct.Price;
                existingProduct.Image = updatedProduct.Image;
                existingProduct.Description = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(updatedProduct.Description.Trim()); 
                existingProduct.DateProduction = updatedProduct.DateProduction;
                existingProduct.NbProductBox = updatedProduct.NbProductBox;
                existingProduct.Home = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(updatedProduct.Home.Trim());

                // Update the family if FamilyId is provided
                if (updatedProduct.FamilyId > 0)
                {
                    var existingFamily = _context.Family.Find(updatedProduct.FamilyId);
                    if (existingFamily != null)
                    {
                        existingProduct.Family = existingFamily;
                    }
                    else
                    {
                        return BadRequest("L'ID de la famille fournie n'est pas valide");
                    }
                }
               else 
                {
                    // il peut modifier que avec un nom de famille existant
                    if (updatedProduct.Family != null)
                    {
                        var existingFamily = _context.Family.FirstOrDefault(f =>f.Name.ToLower() == updatedProduct.Family.Name.Trim().ToLower());
                        if (existingFamily != null)
                        {
                            // Update the family name
                            existingProduct.Family = existingFamily;
                        }
                        else
                        {
                            return BadRequest("Le nom de la famille n'existe pas");
                        }
                    }                       
                }
                // Update the stock if Stock is provided
                if (updatedProduct.Stock != null)
                {
                    var existingStock = _context.Stock.FirstOrDefault(s => s.ProductId == existingProduct.Id);

                    if (existingStock != null)
                    {
                        // Update stock properties
                        existingStock.Quantity = updatedProduct.Stock.Quantity;
                        existingStock.AutoOrder = updatedProduct.Stock.AutoOrder;
                        existingStock.Minimum = updatedProduct.Stock.Minimum;
                        existingStock.Maximum = updatedProduct.Stock.Maximum;
                        if (updatedProduct.Stock.AutoOrder && (updatedProduct.Stock.Minimum == null || updatedProduct.Stock.Maximum == null || updatedProduct.Stock.Maximum==0 || updatedProduct.Stock.Minimum==0))
                        {
                            return BadRequest("Les valeurs Minimum et Maximum sont obligatoires lorsque AutoOrder est true.");
                        }
                    }
                    else
                    {
                        return BadRequest("Le produit n'a pas de stock existant.");
                    }
                }
                // Save changes to the database
                _context.SaveChanges();

                return Ok(existingProduct);
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

                var Product = _context.Product
                    .Include(p => p.Stock)
                    .FirstOrDefault(p => p.Id == id);

                if (Product == null)
                {
                    return BadRequest("Produit non trouvé");
                }
                // si on supprime un produit on supprime son stock aussi
                if (Product.Stock != null)
                {
                    _context.Stock.Remove(Product.Stock);
                }
                _context.Product.Remove(Product);
                _context.SaveChanges();
                return Ok("Suppression réussie");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetProductByProvider/{ProviderId}")]
        public IActionResult GetProductByProvider(int ProviderId)
        {
            try
            {
                var Products = _context.Product.Include(f => f.Family).Include(p => p.Stock).Include(p => p.Provider).AsQueryable();
                Products = Products.Where(p => p.ProviderId == ProviderId);
                var ProductsList = Products.ToList();
                if (ProductsList.Count == 0)
                {
                    return NotFound("Aucun produit trouvé pour ce fournisseur.");
                }
                else
                {
                    return Ok(ProductsList);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetProductByFamily/{familyId}")]
        public IActionResult GetProductByFamily(int familyId, int? page = null, int? pageSize = null, string Home = null, string Name = null,
                        DateOnly? dateProduction = null, Decimal? Price = null, string sortOrder = null)
        {
            try
            {
                var Products = _context.Product.Include(f => f.Family).Include(p => p.Stock).Include(p => p.Provider).AsQueryable();
                // Filter products by family
                Products = Products.Where(p => p.FamilyId == familyId);
                //les filtres (home /nom / date /annéé de production)
                if (!string.IsNullOrEmpty(Home))
                {
                    Products = Products.Where(c => c.Home.ToLower() == Home.Trim().ToLower());
                }
                if (!string.IsNullOrEmpty(Name))
                {
                    Products = Products.Where(c => c.Name.ToLower() == Name.Trim().ToLower());
                }
                if (dateProduction.HasValue)
                {
                    Products = Products.Where(c => c.DateProduction.Year >= dateProduction.Value.Year);
                }
                if (Price.HasValue)
                {
                    Products = Products.Where(c => c.Price == Price.Value);
                }
                // pour le tri selon le prix sinon desc par id (par defaut)
                switch (sortOrder)
                {
                    case "price_asc":
                        Products = Products.OrderBy(c => c.Price);
                        break;
                    case "price_desc":
                        Products = Products.OrderByDescending(c => c.Price);
                        break;
                    default:
                        Products = Products.OrderByDescending(c => c.Id);
                        break;
                }
                // pour pagination 
                if (page.HasValue && pageSize.HasValue)
                {
                    Products = Products.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }
                var FiltredProducts = Products.ToList();
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
        [HttpGet("IsProductHasAnyTransaction/{idProduct}")]
        public IActionResult IsProductHasAnyTransaction(int idProduct)
        {
            try
            {
                // Vérifier si le produit a un inventaire
                var hasInventory = _context.InventoryLigne.Any(il => il.Stock != null && il.Stock.ProductId == idProduct);
                // Vérifier si le produit a une commande  fournisseur 
                var hasProviderOrder = _context.ProviderOrderLine.Any(pol => pol.ProductId == idProduct);
                // Vérifier si le produit a une commande  client 
                var hasClientOrder = _context.ClientOrderLine.Any(col => col.ProductId == idProduct);
                var isHasAnyTransaction = hasInventory || hasProviderOrder || hasClientOrder;
                return Ok(isHasAnyTransaction);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
