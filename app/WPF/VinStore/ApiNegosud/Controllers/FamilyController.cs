using System.Globalization;
using ApiNegosud.DataAccess;
using ApiNegosud.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiNegosud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamilyController : ControllerBase
    {
        private readonly ConnexionDbContext _context;

        public FamilyController(ConnexionDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Get(string name = null)
        {
            try
            {
                // Filtrer les employés en fonction des paramètres fournis en castant to lower case
                var families = _context.Family.Include(f =>f.Products).AsQueryable();

                if (!string.IsNullOrEmpty(name))
                {
                    families = families.Where(e => e.Name.ToLower() == name.ToLower());
                }
                families = families.OrderByDescending(e => e.Id);
                var filteredFamilies = families.ToList();

                if (filteredFamilies.Count == 0)
                {
                    return NotFound("Aucune famille trouvée avec les paramètres fournis.");
                }
                else
                {
                    return Ok(filteredFamilies);
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
                var family = _context.Family.Find(id);
                if (family == null)
                {
                    return NotFound($" La famille {family.Name} n'a pas été trouvée.");
                }
                else
                {
                    return Ok(family);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult Post(Family family)
        {
            try
            {
                // Convertir le nom existant en minuscules pour la comparaison insensible à la casse
                var existingFamily = _context.Family.FirstOrDefault(f => f.Name.ToLower() == family.Name.ToLower());

                if (existingFamily != null)
                {
                    return Conflict($"Une famille avec le nom '{family.Name}' existe déjà.");
                }
                // Mettre la première lettre en majuscule
                family.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(family.Name);
                _context.Add(family);
                _context.SaveChanges();

                return Ok("Famille ajoutée avec succès!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(Family updatedFamily)
        {
            if (updatedFamily == null)
            {
                return BadRequest("Famille non trouvée");
            }
            else if (updatedFamily.Id == 0)
            {
                return BadRequest("la famille est invalid");
            }
            try
            {
                // Convertir le nom existant en minuscules pour la comparaison avec casse
                var existingFamily = _context.Family.FirstOrDefault(f => f.Name.ToLower() == updatedFamily.Name.ToLower() && f.Id != updatedFamily.Id);

                if (existingFamily != null)
                {
                    return Conflict($"Une famille avec le nom '{updatedFamily.Name}' existe déjà.");
                }                
                var familySearched = _context.Family.Find(updatedFamily.Id);
                if (familySearched == null)
                {
                    return NotFound("Famille non trouvée");
                }
                familySearched.Name = updatedFamily.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(updatedFamily.Name);
                _context.SaveChanges();
                return Ok("Mise à jour du nom réussie");
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
                var family = _context.Family.Find(id);
                if (family == null)
                {
                    return BadRequest("Famille non trouvée");
                }
                // Vérifier si des produits utilisent cette famille
                var productsWithFamily = _context.Product.Any(p => p.FamilyId == id);

                if (productsWithFamily)
                {
                    return Conflict("La famille ne peut pas être supprimée car elle est utilisée par au moins un produit.");
                }

                _context.Family.Remove(family);
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
