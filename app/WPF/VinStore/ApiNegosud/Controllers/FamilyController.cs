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
        public IActionResult Get(string Name = null)
        {
            try
            {
                // Filtrer les employés en fonction des paramètres fournis en castant to lower case
                var Families = _context.Family.Include(f =>f.Products).AsQueryable();

                if (!string.IsNullOrEmpty(Name))
                {
                    Families = Families.Where(e => e.Name.ToLower() == Name.ToLower());
                }
                Families = Families.OrderByDescending(e => e.Id);
                var FilteredFamilies = Families.ToList();

                if (FilteredFamilies.Count == 0)
                {
                    return NotFound("Aucune famille trouvée avec les paramètres fournis.");
                }
                else
                {
                    return Ok(FilteredFamilies);
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
                var Family = _context.Family.Find(id);
                if (Family == null)
                {
                    return NotFound($" La famille {Family.Name} n'a pas été trouvée.");
                }
                else
                {
                    return Ok(Family);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult Post(Family Family)
        {
            try
            {
                // Convertir le nom existant en minuscules pour la comparaison insensible à la casse
                var ExistingFamily = _context.Family.FirstOrDefault(f => f.Name.ToLower() == Family.Name.ToLower());

                if (ExistingFamily != null)
                {
                    return Conflict($"Une famille avec le nom '{Family.Name}' existe déjà.");
                }
                // Mettre la première lettre en majuscule
                Family.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Family.Name);
                _context.Add(Family);
                _context.SaveChanges();

                return Ok("Famille ajoutée avec succès!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(Family UpdatedFamily)
        {
            if (UpdatedFamily == null)
            {
                return BadRequest("Famille non trouvée");
            }
            else if (UpdatedFamily.Id == 0)
            {
                return BadRequest("la famille est invalid");
            }
            try
            {
                // Convertir le nom existant en minuscules pour la comparaison avec casse
                var existingFamily = _context.Family.FirstOrDefault(f => f.Name.ToLower() == UpdatedFamily.Name.ToLower() && f.Id != UpdatedFamily.Id);

                if (existingFamily != null)
                {
                    return Conflict($"Une famille avec le nom '{UpdatedFamily.Name}' existe déjà.");
                }                
                var FamilySearched = _context.Family.Find(UpdatedFamily.Id);
                if (FamilySearched == null)
                {
                    return NotFound("Famille non trouvée");
                }
                FamilySearched.Name = UpdatedFamily.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(UpdatedFamily.Name);
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
                var Family = _context.Family.Find(id);
                if (Family == null)
                {
                    return BadRequest("Famille non trouvée");
                }
                // Vérifier si des produits utilisent cette famille
                var productsWithFamily = _context.Product.Any(p => p.FamilyId == id);

                if (productsWithFamily)
                {
                    return Conflict("La famille ne peut pas être supprimée car elle est utilisée par au moins un produit.");
                }

                _context.Family.Remove(Family);
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
