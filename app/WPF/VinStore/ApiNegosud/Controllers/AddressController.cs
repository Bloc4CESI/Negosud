using System.Globalization;
using System.Net;
using ApiNegosud.DataAccess;
using ApiNegosud.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiNegosud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly ConnexionDbContext _context;

        public AddressController(ConnexionDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Get(string name = null, string city = null, string country = null, string street = null)
        {
            try
            {
                // Filtrer les adresses en fonction des paramètres fournis en castannt to lower case ( pour faire un champ de recherche)
                var addresses = _context.Address
                                .Include(a => a.Client)
                                .Include(a => a.Provider)
                                .AsQueryable();
                if (!string.IsNullOrEmpty(name))
                {
                    addresses = addresses.Where(e => e.Name.ToLower() == name.ToLower());
                }

                if (!string.IsNullOrEmpty(city))
                {
                    addresses = addresses.Where(e => e.City.ToLower() == city.ToLower());
                }

                if (!string.IsNullOrEmpty(country))
                {
                    addresses = addresses.Where(e => e.Country.ToLower() == country.ToLower());
                }
                if (!string.IsNullOrEmpty(street))
                {
                    addresses = addresses.Where(e => e.Street.ToLower() == street.ToLower());
                }
                addresses = addresses.OrderByDescending(e => e.Id);
                var filteredAddresses = addresses.ToList();

                if (filteredAddresses.Count == 0)
                {
                    return NotFound("Aucune  adresse a été trouvé avec les paramètres fournis.");
                }
                else
                {
                    return Ok(filteredAddresses);
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
                var address = _context.Address.Find(id);
                if (address == null)
                {
                    return NotFound($"Adresse non trouvé");
                }
                else
                {
                    return Ok(address);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult Post(Address address)
        {
            try
            {
                // capital first lettre
                address.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(address.Name);
                address.Street = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(address.Street);
                address.City = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(address.City);
                address.Country = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(address.Country);
                // Ajouter l'adresse au contexte
                _context.Add(address);

                // Enregistrer les modifications dans la base de données
                _context.SaveChanges();

                return Ok("L'adresse est ajoutée avec succès!");
            }
            catch (Exception ex)
            {
                // En cas d'erreur, renvoyer un message d'erreur
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public IActionResult Put(Address updatedAddress)
        {
            try
            {
                if (updatedAddress == null)
                {
                    return BadRequest("Les données de mise à jour de l'adresse sont invalides");
                }

                var existingAddress = _context.Address.Find(updatedAddress.Id);

                if (existingAddress == null)
                {
                    return NotFound($"L'adresse avec l'ID {updatedAddress.Id} n'a pas été trouvé");
                }

                // Mettre à jour les propriétés de l'adresse
                existingAddress.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(updatedAddress.Name);
                existingAddress.Number = updatedAddress.Number;
                existingAddress.Street = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(updatedAddress.Street);
                existingAddress.City = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(updatedAddress.City);
                existingAddress.Country = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(updatedAddress.Country);

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
                var address = _context.Address.Find(id);
                if (address == null)
                {
                    return BadRequest("L'adresse non trouvé");
                }

                // Vérifier si des fournisseurs utilisent cette adresse
                var providersWithAddress = _context.Provider.Any(p => p.AddressId == id);

                // Vérifier si des clients utilisent cette adresse
                var clientsWithAddress = _context.Client.Any(c => c.AddressId == id);
                if (providersWithAddress || clientsWithAddress)
                {
                    return Conflict("L'adresse ne peut pas être supprimée car elle est utilisée par au moins un fournisseur ou client.");
                }
                _context.Address.Remove(address);
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
