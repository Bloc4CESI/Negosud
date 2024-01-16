using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows;
using ApiNegosud.Models;
using System.Net.Http.Json;
using System.Net;

namespace VinStore.Services
{
    public static class CommandProviderService
    {
        public static async Task<(string ResponseMessage, ProviderOrder? CreatedOrder)> PostProviderOrder(ProviderOrder providerOrder)
        {
            try
            {
                var response = await ApiConnexion.ApiClient.PostAsJsonAsync("https://localhost:7281/api/ProviderOrder/CreateProviderOrder", providerOrder);

                if (response.IsSuccessStatusCode)
                {
                    // Désérialiserla réponse pour obtenir l'objet créé avec id
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeAnonymousType(responseContent, new { newProviderOrder = new ProviderOrder(), Message = "" });
                    return (responseObject.Message, responseObject.newProviderOrder);
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    // Récupérer le message d'erreur
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return ($"Erreur lors de la requête : {errorMessage}", null);
                }
                else
                {
                    // Gérer d'autres codes d'erreur si nécessaire
                    return ($"Erreur lors de la requête : {response.ReasonPhrase}", null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la requête : {ex.Message}");
                return ($"Une erreur s'est produite lors de la requête : {ex.Message}", null);
            }
        }

        public static async Task<string> UpdateProviderOrder(ProviderOrder updatedOrder)
        {
            try
            {
                var response = await ApiConnexion.ApiClient.PutAsJsonAsync("https://localhost:7281/api/ProviderOrder/UpdateOrder", updatedOrder);

                if (response.IsSuccessStatusCode)
                {
                    return "Mise à jour réussie";
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return $"La commande avec l'ID {updatedOrder.Id} n'a pas été trouvée";
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
        public static async Task<List<ProviderOrder>> GetProviderOrderByStatus(ProviderOrderStatus status)
        {
            try
            {
                var response = await ApiConnexion.ApiClient.GetAsync($"https://localhost:7281/api/ProviderOrder/bystatus/{status}");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var providerOrders = JsonConvert.DeserializeObject<List<ProviderOrder>>(responseContent);
                    return providerOrders;
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
        public static async Task DeleteProvider(int idProvider)
        {
            var response = await ApiConnexion.ApiClient.DeleteAsync($"https://localhost:7281/api/ProviderOrder/{idProvider}");

        }

    }
}
