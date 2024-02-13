using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ApiNegosud.Models;
using Newtonsoft.Json;

namespace VinStore.Services
{
    internal class CommandClientService
    {
        public static async Task<List<ClientOrder>> GetClientOrderByStatus(OrderStatus status, DateTime? date = null)
        {
            try
            {
                string apiUrl = $"https://localhost:7281/api/ClientOrder/bystatus/{status}";
                if (date.HasValue)
                {
                    apiUrl += $"?date={date.Value:yyyy-MM-dd}";
                }
                var response = await ApiConnexion.ApiClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var clientOrders = JsonConvert.DeserializeObject<List<ClientOrder>>(responseContent);
                    return clientOrders!;
                }
                else
                {
                    Console.WriteLine($"La requête a échoué avec le code de statut : {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la requête : {ex.Message}");
                return null;
            }
        }
        public static async Task<string> UpdateClientOrder(ClientOrder clientOrder)
        {
            try
            {
                var response = await ApiConnexion.ApiClient.PutAsJsonAsync("https://localhost:7281/api/ClientOrder/UpdateOrder", clientOrder);

                if (response.IsSuccessStatusCode)
                {
                    return "Mise à jour réussie";
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return $"La commande avec l'ID {clientOrder.Id} n'a pas été trouvée";
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return $"Erreur lors de la requête : {errorMessage}";
                }
                else
                {
                    return $"Erreur lors de la requête : {response.ReasonPhrase}";
                }
            }
            catch (Exception ex)
            {
                return $"Une erreur s'est produite lors de la requête : {ex.Message}";
            }
        }

    }
}
