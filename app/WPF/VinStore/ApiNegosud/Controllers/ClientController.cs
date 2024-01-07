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
    public class ClientController : ControllerBase
    {
        private readonly ConnexionDbContext _context;

        public ClientController(ConnexionDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Get(string firstName = null, string lastName = null, string email = null, string phoneNumber = null,
                           string addressName = null, string city = null, string country = null)
        {
            try
            {
                // Filtrer les clients avec les champs de recherche, en castant en minuscules
                var clients = _context.Client
                    .Include(c => c.Address) // Inclure l'adresse associée
                    .AsQueryable();

                if (!string.IsNullOrEmpty(firstName))
                {
                    clients = clients.Where(c => c.FirstName.ToLower() == firstName.Trim().ToLower());
                }

                if (!string.IsNullOrEmpty(lastName))
                {
                    clients = clients.Where(c => c.LastName.ToLower() == lastName.Trim().ToLower());
                }

                if (!string.IsNullOrEmpty(email))
                {
                    clients = clients.Where(c => c.Email.ToLower() == email.Trim().ToLower());
                }

                if (!string.IsNullOrEmpty(phoneNumber))
                {
                    clients = clients.Where(c => c.PhoneNumber == phoneNumber);
                }

                // Filtrer par les champs de l'adresse
                if (!string.IsNullOrEmpty(addressName))
                {
                    clients = clients.Where(c => c.Address.Name.ToLower() == addressName.Trim().ToLower());
                }

                if (!string.IsNullOrEmpty(city))
                {
                    clients = clients.Where(c => c.Address.City.ToLower() == city.Trim().ToLower());
                }

                if (!string.IsNullOrEmpty(country))
                {
                    clients = clients.Where(c => c.Address.Country.ToLower() == country.Trim().ToLower());
                }

                clients = clients.OrderByDescending(c => c.Id);

                var filteredClients = clients.ToList();

                if (filteredClients.Count == 0)
                {
                    return NotFound("Aucun client trouvé avec les paramètres fournis.");
                }
                else
                {
                    return Ok(filteredClients);
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
                var client = _context.Client
                    .Include(c => c.Address) // Inclure les champs de l'adresse
                    .SingleOrDefault(c => c.Id == id);

                if (client == null)
                {
                    return NotFound($"Le client avec l'ID {id} est non trouvé");
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
        public IActionResult PostWithAddress(Client client)
        {
            try
            {
                // Capitaliser la première lettre des noms
                client.FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(client.FirstName.Trim());
                client.LastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(client.LastName.Trim());

                // Hasher le mot de passe avant de l'enregistrer
                client.Password = BCrypt.Net.BCrypt.HashPassword(client.Password);

                // Vérifier si l'adresse e-mail est unique
                if (_context.Client.Any(c => c.Email == client.Email.Trim()))
                {
                    return BadRequest("L'adresse e-mail est déjà utilisée.");
                }

                // Si l'ID de l'adresse est fourni, vérifier qu'il est unique et associer cette adresse
                if (client.AddressId > 0)
                {
                    var existingAddress = _context.Address.Find(client.AddressId);

                    // Vérifier si l'adresse existe et qu'elle n'est pas déjà associée à un autre client
                    if (existingAddress != null &&
                          !_context.Address.Any(a => a.Id == client.AddressId &&
                          ((a.Client != null && a.Client.Id != client.Id) || (a.Provider != null ))))
                    {
                        client.Address = existingAddress;
                    }
                    else
                    {
                        return BadRequest("L'ID de l'adresse fourni n'est pas valide ou est déjà associé à un autre client.");
                    }
                }
                else 
                {
                    if (client.Address != null) { 
                        // Créer une nouvelle instance d'adresse avec les détails fournis
                        var address = new Address
                        {
                            Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(client.Address.Name.Trim()),
                            Number = client.Address.Number,
                            Street = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(client.Address.Street.Trim()),
                            City = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(client.Address.City.Trim()),
                            Country = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(client.Address.Country.Trim())
                        };

                        // Ajouter la nouvelle adresse au contexte
                        _context.Address.Add(address);
                        _context.SaveChanges();

                        // Associer l'adresse au client
                        client.Address = address;
                    }
                }

                // Ajouter le client au contexte
                _context.Add(client);

                // Enregistrer les modifications dans la base de données
                _context.SaveChanges();

                return Ok("Le client est ajouté avec succès!");
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
                var Client = _context.Client.SingleOrDefault(c => c.Email.Trim() == Email);
                if (Client == null)
                {
                    return NotFound($"Aucun client trouvé avec cette adresse {Client.Email} ");
                }
                else
                {
                    return Ok(Client);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

       
        [HttpPut("{id}")]
        public IActionResult PutWithAddress(Client UpdatedClient)
        {
            try
            {
                var existingClient = _context.Client.Include(c => c.Address).FirstOrDefault(c => c.Id == UpdatedClient.Id);

                if (existingClient == null)
                {
                    return NotFound($"Le client avec l'ID {UpdatedClient.Id} n'a pas été trouvé");
                }

                // Vérifier si l'e-mail mis à jour est utilisé par un autre client
                var isEmailTaken = _context.Client.Any(c => c.Id != UpdatedClient.Id && c.Email.Trim() == UpdatedClient.Email.Trim());

                if (isEmailTaken)
                {
                    return BadRequest($"L'e-mail {UpdatedClient.Email} est déjà utilisé par une autre personne.");
                }
                else
                {
                    existingClient.Email= UpdatedClient.Email.Trim();
                }

                // Mettre à jour les propriétés du client existant avec la première lettre en majuscule
                existingClient.FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(UpdatedClient.FirstName.Trim());
                existingClient.LastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(UpdatedClient.LastName.Trim());
                existingClient.Email = UpdatedClient.Email.Trim();
                existingClient.PhoneNumber = UpdatedClient.PhoneNumber;
                if (!string.IsNullOrEmpty(UpdatedClient.Password))
                {
                    // Hasher le nouveau mot de passe avec BCrypt
                    existingClient.Password = BCrypt.Net.BCrypt.HashPassword(UpdatedClient.Password);
                }
                else { existingClient.Password = existingClient.Password; }
                if (UpdatedClient.AddressId > 0)
                {
                    var existingAddress = _context.Address.Find(UpdatedClient.AddressId);

                    // Vérifier si l'adresse existe et qu'elle n'est pas déjà associée à un autre client
                    if (existingAddress != null &&
                          !_context.Address.Any(a => a.Id == UpdatedClient.AddressId &&
                          ((a.Client != null && a.Client.Id != UpdatedClient.Id) || (a.Provider != null))))
                    {
                        existingClient.Address = existingAddress;
                    }
                    else
                    {
                        return BadRequest("L'ID de l'adresse fourni n'est pas valide ou est déjà associé à un autre client.");
                    }
                }
                else
                {
                    if (UpdatedClient.Address != null && existingClient.Address != null)
                    {
                        existingClient.Address.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(UpdatedClient.Address.Name.Trim());
                        existingClient.Address.Number = UpdatedClient.Address.Number;
                        existingClient.Address.Street = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(UpdatedClient.Address.Street.Trim());
                        existingClient.Address.City = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(UpdatedClient.Address.City.Trim());
                        existingClient.Address.Country = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(UpdatedClient.Address.Country.Trim());
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
                var Client = _context.Client.Find(id);
                if (Client == null)
                {
                    return BadRequest("Client non trouvé");
                }
                _context.Client.Remove(Client);
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
