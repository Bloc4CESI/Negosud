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
        public IActionResult Get(string Name = null, string City = null, string Country = null, string Street = null)
        {
            try
            {
                // Filtrer les adresses en fonction des paramètres fournis en castannt to lower case ( pour faire un champ de recherche)
                var Addresses = _context.Address
                                .Include(a => a.Client)
                                .Include(a => a.Provider)
                                .AsQueryable();
                if (!string.IsNullOrEmpty(Name))
                {
                    Addresses = Addresses.Where(e => e.Name.ToLower() == Name.ToLower());
                }

                if (!string.IsNullOrEmpty(City))
                {
                    Addresses = Addresses.Where(e => e.City.ToLower() == City.ToLower());
                }

                if (!string.IsNullOrEmpty(Country))
                {
                    Addresses = Addresses.Where(e => e.Country.ToLower() == Country.ToLower());
                }
                if (!string.IsNullOrEmpty(Street))
                {
                    Addresses = Addresses.Where(e => e.Street.ToLower() == Street.ToLower());
                }
                Addresses = Addresses.OrderByDescending(e => e.Id);
                var FilteredAddresses = Addresses.ToList();

                if (FilteredAddresses.Count == 0)
                {
                    return NotFound("Aucune  adresse a été trouvé avec les paramètres fournis.");
                }
                else
                {
                    return Ok(FilteredAddresses);
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
                var Address = _context.Address.Find(id);
                if (Address == null)
                {
                    return NotFound($"Adresse non trouvé");
                }
                else
                {
                    return Ok(Address);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult Post(Address Address)
        {
            try
            {
                // capital first lettre
                Address.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Address.Name);
                Address.Street = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Address.Street);
                Address.City = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Address.City);
                Address.Country = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Address.Country);
                // Ajouter l'adresse au contexte
                _context.Add(Address);

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
        public IActionResult Put(Address UpdatedAddress)
        {
            try
            {
                if (UpdatedAddress == null)
                {
                    return BadRequest("Les données de mise à jour de l'adresse sont invalides");
                }

                var ExistingAddress = _context.Address.Find(UpdatedAddress.Id);

                if (ExistingAddress == null)
                {
                    return NotFound($"L'adresse avec l'ID {UpdatedAddress.Id} n'a pas été trouvé");
                }

                // Mettre à jour les propriétés de l'adresse
                ExistingAddress.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(UpdatedAddress.Name);
                ExistingAddress.Number = UpdatedAddress.Number;
                ExistingAddress.Street = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(UpdatedAddress.Street);
                ExistingAddress.City = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(UpdatedAddress.City);
                ExistingAddress.Country = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(UpdatedAddress.Country);

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
                var Address = _context.Address.Find(id);
                if (Address == null)
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
                _context.Address.Remove(Address);
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
