using System.Globalization;
using ApiNegosud.DataAccess;
using ApiNegosud.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiNegosud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ConnexionDbContext _context;

        public EmployeeController(ConnexionDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Get(string firstName = null, string lastName = null, string email = null)
        {
            try
            {
                // Filtrer les employés en fonction des paramètres fournis en castant to lower case
                var Employees = _context.Employee.AsQueryable();

                if (!string.IsNullOrEmpty(firstName))
                {
                    Employees = Employees.Where(e => e.FirstName.ToLower() == firstName.ToLower());
                }

                if (!string.IsNullOrEmpty(lastName))
                {
                    Employees = Employees.Where(e => e.LastName.ToLower() == lastName.ToLower());
                }

                if (!string.IsNullOrEmpty(email))
                {
                    Employees = Employees.Where(e => e.Email.ToLower() == email.ToLower());
                }
                Employees = Employees.OrderByDescending(e => e.Id);
                // Récupérer la liste filtrée des employés
                var FilteredEmployees = Employees.ToList();

                if (FilteredEmployees.Count == 0)
                {
                    return NotFound("Aucun employé trouvé avec les paramètres fournis.");
                }
                else
                {
                    return Ok(FilteredEmployees);
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
                var Employee = _context.Employee.Find(id);
                if (Employee == null)
                {
                    return NotFound($"L'employé avec l'email  {Employee.Email} est non trouvé");
                }
                else
                {
                    return Ok(Employee);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult Post(Employee Employee)
        {
            try
            {

                if (!string.IsNullOrEmpty(Employee.FirstName))
                {
                    Employee.FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Employee.FirstName);
                }

                if (!string.IsNullOrEmpty(Employee.LastName))
                {
                    Employee.LastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Employee.LastName);
                }
                // Hasher le mot de passe avant de l'enregistrer
                Employee.Password = BCrypt.Net.BCrypt.HashPassword(Employee.Password);

                // Vérifier si l'adresse e-mail est unique
                if (_context.Employee.Any(e => e.Email == Employee.Email))
                {
                    return BadRequest("L'adresse e-mail est déjà utilisée.");
                }

                // Ajouter l'employé au contexte
                _context.Add(Employee);

                // Enregistrer les modifications dans la base de données
                _context.SaveChanges();

                return Ok("L'employé est ajouté avec succès!");
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
                var Employee = _context.Employee.SingleOrDefault(e => e.Email == Email);
                if (Employee == null)
                {
                    return NotFound($"Aucun employé trouvé avec cette adresse  {Employee.Email} ");
                }
                else
                {
                    return Ok(Employee);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public IActionResult Put(Employee UpdatedEmployee)
        {
            try
            {
                if (UpdatedEmployee == null)
                {
                    return BadRequest("Les données de mise à jour de l'employé sont invalides");
                }

                var ExistingEmployee = _context.Employee.Find(UpdatedEmployee.Id);

                if (ExistingEmployee == null)
                {
                    return NotFound($"L'employé avec l'ID {UpdatedEmployee.Id} n'a pas été trouvé");
                }

                // Vérifier si l'e-mail mis à jour est utilisé par un autre employé
                var IsEmailTaken = _context.Employee.Any(e => e.Id != UpdatedEmployee.Id && e.Email == UpdatedEmployee.Email);

                if (IsEmailTaken)
                {
                    return BadRequest($"L'e-mail {UpdatedEmployee.Email} est déjà utilisé par un autre employé.");
                }
                // Mettre à jour les propriétés de l'employé existant avec capital lettre FirstName +LastName
                if (!string.IsNullOrEmpty(UpdatedEmployee.FirstName))
                {
                    ExistingEmployee.FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(UpdatedEmployee.FirstName);
                }

                if (!string.IsNullOrEmpty(UpdatedEmployee.LastName))
                {
                    ExistingEmployee.LastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(UpdatedEmployee.LastName);
                }
               
                ExistingEmployee.Email = UpdatedEmployee.Email;

                // Vérifier si le mot de passe a été modifié
                if (!string.IsNullOrEmpty(UpdatedEmployee.Password))
                {
                    // Hasher le nouveau mot de passe avec BCrypt
                    ExistingEmployee.Password = BCrypt.Net.BCrypt.HashPassword(UpdatedEmployee.Password);
                }
                else { ExistingEmployee.Password = ExistingEmployee.Password; }
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
                var Employee = _context.Employee.Find(id);
                if (Employee == null)
                {
                    return BadRequest("L'employé non trouvé");
                }
                _context.Employee.Remove(Employee);
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
