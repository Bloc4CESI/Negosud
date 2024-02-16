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
        public IActionResult Get(string name = null)
        {
            try
            {
                var providers = _context.Provider.Include(c => c.Address).Include(c => c.Products).AsQueryable();

                if (!string.IsNullOrEmpty(name))
                {
                    providers = providers.Where(c => c.Name.ToLower().Contains(name.ToLower().Trim()));
                }
                providers = providers.OrderByDescending(c => c.Id);

                var filtredProviders = providers.ToList();

                if (filtredProviders.Count == 0)
                {
                    return NotFound($"Aucun fournisseur trouvé avec ce nom {name} fourni.");
                }
                else
                {
                    return Ok(filtredProviders);
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
        public IActionResult PostWithAddress(Provider provider)
        {
            try
            {
                // Capitaliser la première lettre des noms
                provider.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(provider.Name.Trim());
               
                // Vérifier si l'adresse e-mail est unique
                if (_context.Provider.Any(p => p.Email == provider.Email.Trim()))
                {
                    return BadRequest("L'adresse e-mail est déjà utilisée.");
                }

                // Si l'ID de l'adresse est fourni, vérifier qu'il est unique et associer cette adresse
                if (provider.AddressId > 0)
                {
                    var existingAddress = _context.Address.Find(provider.AddressId);

                    // Vérifier si l'adresse existe et qu'elle n'est pas déjà associée à un autre fournisseur ou client
                    if (existingAddress != null &&
                          !_context.Address.Any(a => a.Id == provider.AddressId &&
                          ((a.Client != null)|| (a.Provider != null && a.Provider.Id != provider.Id))))
                    {
                        provider.Address = existingAddress;
                    }
                    else
                    {
                        return Conflict("L'ID de l'adresse fourni n'est pas valide ou est déjà associé à un autre fournisseur.");
                    }
                }
                else
                {
                    if(provider.Address != null) {
                        // Créer une nouvelle instance d'adresse avec les détails fournis
                        var address = new Address
                        {
                            Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(provider.Address.Name.Trim()),
                            Number = provider.Address.Number,
                            Street = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(provider.Address.Street.Trim()),
                            City = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(provider.Address.City.Trim()),
                            Country = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(provider.Address.Country.Trim())
                        };
                        // Ajouter la nouvelle adresse au contexte
                        _context.Address.Add(address);

                        // Associer l'adresse au client
                        provider.Address = address;
                    }
                    
                }
                // Ajouter le client au contexte
                _context.Add(provider);

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
        public IActionResult GetByEmail(string email)
        {
            try
            {
                var provider = _context.Provider.SingleOrDefault(c => c.Email == email.Trim());
                if (provider == null)
                {
                    return NotFound($"Aucun fournisseur trouvé avec cette adresse {provider.Email} ");
                }
                else
                {
                    return Ok(provider);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{id}")]
        public IActionResult Put(Provider updatedProvider)
        {
            try
            {
                var existingProvider = _context.Provider.Include(p => p.Address).FirstOrDefault(p => p.Id == updatedProvider.Id);

                if (existingProvider == null)
                {
                    return NotFound($"Le fournisseur avec l'ID {updatedProvider.Id} n'a pas été trouvé");
                }

                // Vérifier si l'e-mail mis à jour est utilisé par un autre fournisseur
                var isEmailTaken = _context.Provider.Any(p => p.Id != updatedProvider.Id && p.Email == updatedProvider.Email.Trim());

                if (isEmailTaken)
                {
                    return Conflict($"L'e-mail {updatedProvider.Email} est déjà utilisé par une autre personne.");
                }

                // Mettre à jour les propriétés du fournisseur existant avec la première lettre en majuscule
                existingProvider.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(updatedProvider.Name.Trim());
                existingProvider.Email = updatedProvider.Email;
                existingProvider.PhoneNumber = updatedProvider.PhoneNumber;
                if (updatedProvider.AddressId > 0)
                {
                    var existingAddress = _context.Address.Find(updatedProvider.AddressId);

                    // Vérifier si l'adresse existe et qu'elle n'est pas déjà associée à un autre client ou fournisseur
                    if (existingAddress != null &&
                        !_context.Address.Any(a => a.Id == updatedProvider.AddressId &&
                        ((a.Client != null) || (a.Provider != null && a.Provider.Id != updatedProvider.Id))))
                    {
                        existingProvider.Address = existingAddress;
                    }
                    else
                    {
                        return BadRequest("L'ID de l'adresse fourni n'est pas valide ou est déjà associé à un autre fournisseur.");
                    }
                }
                else 
                {
                    if (updatedProvider.Address != null && existingProvider.Address != null)
                    {
                        existingProvider.Address.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(updatedProvider.Address.Name);
                        existingProvider.Address.Number = updatedProvider.Address.Number;
                        existingProvider.Address.Street = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(updatedProvider.Address.Street);
                        existingProvider.Address.City = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(updatedProvider.Address.City);
                        existingProvider.Address.Country = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(updatedProvider.Address.Country);
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
                    .Include(p => p.Address) // S'assurer d'inclure l'adresse pour la charger
                    .FirstOrDefault(p => p.Id == id);

                if (provider == null)
                {
                    return BadRequest("Fournisseur non trouvé");
                }

                if (provider.Products != null && provider.Products.Any())
                {
                    return Conflict("Le fournisseur a des produits associés. Suppression annulée.");
                }
                if (provider.Address != null)
                {
                    _context.Address.Remove(provider.Address);
                    // Pas besoin de sauvegarder les changements ici, car SaveChanges() plus bas s'occupera de tout
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
                var providers = _context.Provider.Include(c => c.Address).Include(c => c.Products)
                    .Where(c => c.Products.Any())
                    .AsQueryable();
                providers = providers.OrderByDescending(c => c.Id);

                var filtredProviders = providers.ToList();

                if (filtredProviders.Count == 0)
                {
                    return NotFound($"Aucun fournisseur trouvé .");
                }
                else
                {
                    return Ok(filtredProviders);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }

}
