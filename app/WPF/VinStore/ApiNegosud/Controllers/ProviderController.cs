using System.Globalization;
using ApiNegosud.DataAccess;
using ApiNegosud.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiNegosud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderController : ControllerBase
    {
        private readonly ConnexionDbContext _context;

        public ProviderController(ConnexionDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Get(string Name = null)
        {
            try
            {
                var Providers = _context.Provider.Include(c => c.Address).Include(c => c.Products).AsQueryable();

                if (!string.IsNullOrEmpty(Name))
                {
                    Providers = Providers.Where(c => c.Name.ToLower() == Name.ToLower().Trim());
                }
                Providers = Providers.OrderByDescending(c => c.Id);

                var FiltredProviders = Providers.ToList();

                if (FiltredProviders.Count == 0)
                {
                    return NotFound($"Aucun fournisseur trouvé avec ce nom {Name} fourni.");
                }
                else
                {
                    return Ok(FiltredProviders);
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
                var client = _context.Provider
                    .Include(c => c.Address)
                    .Include(c => c.Products)
                    .SingleOrDefault(c => c.Id == id);

                if (client == null)
                {
                    return NotFound($"Le fournisseur avec l'ID {id} est non trouvé");
                }
                else
                {
                    return Ok(client);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("PostWithAddress")]
        public IActionResult PostWithAddress(Provider Provider)
        {
            try
            {
                // Capitaliser la première lettre des noms
                Provider.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Provider.Name.Trim());
               
                // Vérifier si l'adresse e-mail est unique
                if (_context.Provider.Any(p => p.Email == Provider.Email.Trim()))
                {
                    return BadRequest("L'adresse e-mail est déjà utilisée.");
                }

                // Si l'ID de l'adresse est fourni, vérifier qu'il est unique et associer cette adresse
                if (Provider.AddressId > 0)
                {
                    var existingAddress = _context.Address.Find(Provider.AddressId);

                    // Vérifier si l'adresse existe et qu'elle n'est pas déjà associée à un autre fournisseur ou client
                    if (existingAddress != null &&
                          !_context.Address.Any(a => a.Id == Provider.AddressId &&
                          ((a.Client != null)|| (a.Provider != null && a.Provider.Id != Provider.Id))))
                    {
                        Provider.Address = existingAddress;
                    }
                    else
                    {
                        return BadRequest("L'ID de l'adresse fourni n'est pas valide ou est déjà associé à un autre fournisseur.");
                    }
                }
                else
                {
                    if(Provider.Address != null) {
                        // Créer une nouvelle instance d'adresse avec les détails fournis
                        var address = new Address
                        {
                            Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Provider.Address.Name.Trim()),
                            Number = Provider.Address.Number,
                            Street = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Provider.Address.Street.Trim()),
                            City = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Provider.Address.City.Trim()),
                            Country = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Provider.Address.Country.Trim())
                        };
                        // Ajouter la nouvelle adresse au contexte
                        _context.Address.Add(address);

                        // Associer l'adresse au client
                        Provider.Address = address;
                    }
                    
                }
                // Ajouter le client au contexte
                _context.Add(Provider);

                // Enregistrer les modifications dans la base de données
                _context.SaveChanges();

                return Ok("Le fournisseur est ajouté avec succès!");
            }
            catch (Exception ex)
            {
                // En cas d'erreur, renvoyer un message d'erreur
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetByEmail/{Email}")]
        public IActionResult GetByEmail(string Email)
        {
            try
            {
                var Provider = _context.Provider.SingleOrDefault(c => c.Email == Email.Trim());
                if (Provider == null)
                {
                    return NotFound($"Aucun fournisseur trouvé avec cette adresse {Provider.Email} ");
                }
                else
                {
                    return Ok(Provider);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{id}")]
        public IActionResult Put(Provider UpdatedProvider)
        {
            try
            {
                var ExistingProvider = _context.Provider.Include(p => p.Address).FirstOrDefault(p => p.Id == UpdatedProvider.Id);

                if (ExistingProvider == null)
                {
                    return NotFound($"Le fournisseur avec l'ID {UpdatedProvider.Id} n'a pas été trouvé");
                }

                // Vérifier si l'e-mail mis à jour est utilisé par un autre fournisseur
                var isEmailTaken = _context.Provider.Any(p => p.Id != UpdatedProvider.Id && p.Email == UpdatedProvider.Email.Trim());

                if (isEmailTaken)
                {
                    return BadRequest($"L'e-mail {UpdatedProvider.Email} est déjà utilisé par une autre personne.");
                }

                // Mettre à jour les propriétés du fournisseur existant avec la première lettre en majuscule
                ExistingProvider.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(UpdatedProvider.Name.Trim());
                ExistingProvider.Email = UpdatedProvider.Email;
                ExistingProvider.PhoneNumber = UpdatedProvider.PhoneNumber;
                if (UpdatedProvider.AddressId > 0)
                {
                    var existingAddress = _context.Address.Find(UpdatedProvider.AddressId);

                    // Vérifier si l'adresse existe et qu'elle n'est pas déjà associée à un autre client ou fournisseur
                    if (existingAddress != null &&
                        !_context.Address.Any(a => a.Id == UpdatedProvider.AddressId &&
                        ((a.Client != null) || (a.Provider != null && a.Provider.Id != UpdatedProvider.Id))))
                    {
                        ExistingProvider.Address = existingAddress;
                    }
                    else
                    {
                        return BadRequest("L'ID de l'adresse fourni n'est pas valide ou est déjà associé à un autre fournisseur.");
                    }
                }
                else 
                {
                    if (UpdatedProvider.Address != null && ExistingProvider.Address != null)
                    {
                        ExistingProvider.Address.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(UpdatedProvider.Address.Name);
                        ExistingProvider.Address.Number = UpdatedProvider.Address.Number;
                        ExistingProvider.Address.Street = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(UpdatedProvider.Address.Street);
                        ExistingProvider.Address.City = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(UpdatedProvider.Address.City);
                        ExistingProvider.Address.Country = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(UpdatedProvider.Address.Country);
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


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var provider = _context.Provider
                    .Include(p => p.Products) 
                    .FirstOrDefault(p => p.Id == id);

                if (provider == null)
                {
                    return BadRequest("Fournisseur non trouvé");
                }

                if (provider.Products != null && provider.Products.Any())
                {
                    return BadRequest("Le fournisseur a des produits associés. Suppression annulée.");
                }

                _context.Provider.Remove(provider);
                _context.SaveChanges();

                return Ok("Suppression réussie");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetOnlyPrivdersWithProducts")]
        public IActionResult GetOnlyPrivdersWithProducts()
        {
            try
            {
                var Providers = _context.Provider.Include(c => c.Address).Include(c => c.Products)
                    .Where(c => c.Products.Any())
                    .AsQueryable();
                Providers = Providers.OrderByDescending(c => c.Id);

                var FiltredProviders = Providers.ToList();

                if (FiltredProviders.Count == 0)
                {
                    return NotFound($"Aucun fournisseur trouvé .");
                }
                else
                {
                    return Ok(FiltredProviders);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }

}
