using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ApiNegosud.Models;
using static ApiNegosud.Models.Inventory;

namespace VinStore.Services
{
    public  static class InventoryService
    {
        public static async Task<(string ResponseMessage, Inventory? Inventory)> PostInventory(Inventory Inventory)
        {
            try
            {
                var response = await ApiConnexion.ApiClient.PostAsJsonAsync("https://localhost:7281/api/Inventory", Inventory);

                if (response.IsSuccessStatusCode)
                {
                    // Désérialiserla réponse pour obtenir l'objet créé avec id
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeAnonymousType(responseContent, new { newInventory  = new Inventory(), Message = "" });
                    return (responseObject.Message, responseObject.newInventory);
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
        public static async Task<string> UpdateInventory(Inventory UpdatedInventory)
        {
            try
            {
                var response = await ApiConnexion.ApiClient.PutAsJsonAsync("https://localhost:7281/api/Inventory/UpdateInventory", UpdatedInventory);

                if (response.IsSuccessStatusCode)
                {
                    return "Mise à jour réussie";
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return $"La commande avec l'ID {UpdatedInventory.Id} n'a pas été trouvée";
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
        public static async Task<List<Inventory>> GetInventoriesByStatus(InventoryEnum status, DateTime? date=null)
        {
            try
            {
                string apiUrl = $"https://localhost:7281/api/Inventory/bystatus/{status}";
                if (date.HasValue)
                {
                    apiUrl += $"?date={date.Value:yyyy-MM-dd}";
                }
                var response = await ApiConnexion.ApiClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var Inventories = JsonConvert.DeserializeObject<List<Inventory>>(responseContent);
                    return Inventories;
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
        public static async Task DeleteInventory(int idInventory)
        {
            var response = await ApiConnexion.ApiClient.DeleteAsync($"https://localhost:7281/api/Inventory/{idInventory}");

        }
    }
}
