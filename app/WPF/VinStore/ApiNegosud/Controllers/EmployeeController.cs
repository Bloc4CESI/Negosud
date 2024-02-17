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
                var employees = _context.Employee.AsQueryable();

                if (!string.IsNullOrEmpty(firstName))
                {
                    employees = employees.Where(e => e.FirstName.ToLower() == firstName.ToLower());
                }

                if (!string.IsNullOrEmpty(lastName))
                {
                    employees = employees.Where(e => e.LastName.ToLower() == lastName.ToLower());
                }

                if (!string.IsNullOrEmpty(email))
                {
                    employees = employees.Where(e => e.Email.ToLower() == email.ToLower());
                }
                employees = employees.OrderByDescending(e => e.Id);
                // Récupérer la liste filtrée des employés
                var filteredEmployees = employees.ToList();

                if (filteredEmployees.Count == 0)
                {
                    return NotFound("Aucun employé trouvé avec les paramètres fournis.");
                }
                else
                {
                    return Ok(filteredEmployees);
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
                var employee = _context.Employee.Find(id);
                if (employee == null)
                {
                    return NotFound($"L'employé avec l'email  {employee.Email} est non trouvé");
                }
                else
                {
                    return Ok(employee);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult Post(Employee employee)
        {
            try
            {

                if (!string.IsNullOrEmpty(employee.FirstName))
                {
                    employee.FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(employee.FirstName);
                }

                if (!string.IsNullOrEmpty(employee.LastName))
                {
                    employee.LastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(employee.LastName);
                }
                // Hasher le mot de passe avant de l'enregistrer
                employee.Password = BCrypt.Net.BCrypt.HashPassword(employee.Password);

                // Vérifier si l'adresse e-mail est unique
                if (_context.Employee.Any(e => e.Email == employee.Email))
                {
                    return BadRequest("L'adresse e-mail est déjà utilisée.");
                }

                // Ajouter l'employé au contexte
                _context.Add(employee);

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
                var employee = _context.Employee.SingleOrDefault(e => e.Email == Email);
                if (employee == null)
                {
                    return NotFound($"Aucun employé trouvé avec cette adresse  {employee.Email} ");
                }
                else
                {
                    return Ok(employee);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public IActionResult Put(Employee updatedEmployee)
        {
            try
            {
                if (updatedEmployee == null)
                {
                    return BadRequest("Les données de mise à jour de l'employé sont invalides");
                }

                var existingEmployee = _context.Employee.Find(updatedEmployee.Id);

                if (existingEmployee == null)
                {
                    return NotFound($"L'employé avec l'ID {updatedEmployee.Id} n'a pas été trouvé");
                }

                // Vérifier si l'e-mail mis à jour est utilisé par un autre employé
                var isEmailTaken = _context.Employee.Any(e => e.Id != updatedEmployee.Id && e.Email == updatedEmployee.Email);

                if (isEmailTaken)
                {
                    return BadRequest($"L'e-mail {updatedEmployee.Email} est déjà utilisé par un autre employé.");
                }
                // Mettre à jour les propriétés de l'employé existant avec capital lettre FirstName +LastName
                if (!string.IsNullOrEmpty(updatedEmployee.FirstName))
                {
                    existingEmployee.FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(updatedEmployee.FirstName);
                }

                if (!string.IsNullOrEmpty(updatedEmployee.LastName))
                {
                    existingEmployee.LastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(updatedEmployee.LastName);
                }
               
                existingEmployee.Email = updatedEmployee.Email;

                // Vérifier si le mot de passe a été modifié
                if (!string.IsNullOrEmpty(updatedEmployee.Password))
                {
                    // Hasher le nouveau mot de passe avec BCrypt
                    existingEmployee.Password = BCrypt.Net.BCrypt.HashPassword(updatedEmployee.Password);
                }
                else { existingEmployee.Password = existingEmployee.Password; }
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
                var employee = _context.Employee.Find(id);
                if (employee == null)
                {
                    return BadRequest("L'employé non trouvé");
                }
                _context.Employee.Remove(employee);
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
