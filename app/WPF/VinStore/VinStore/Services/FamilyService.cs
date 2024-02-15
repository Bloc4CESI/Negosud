using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows;
using ApiNegosud.Models;
using System.Net.Http.Json;
using System.Xml.Linq;

namespace VinStore.Services
{
    public static class FamilyService
    {
        public static async Task<List<Family>> GetFamily()
        {
            try
            {
                var response = await ApiConnexion.ApiClient.GetStringAsync($"https://localhost:7281/api/Family");

                var families = JsonConvert.DeserializeObject<List<Family>>(response);

                return families;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la requête : {ex.Message}");
                return null;
            }
        }
        public static async Task<int> GetFamilyIdByNameAsync(string familyName)
        {
            try
            {
                // Obtenez la liste complète des familles
                List<Family> familyList = await GetFamily();
                // Recherchez l'ID associé au nom
                var matchingFamily = familyList.FirstOrDefault(f => f.Name == familyName);
                // Vérifiez si une famille correspondante a été trouvée
                if (matchingFamily != null)
                {
                    return matchingFamily.Id; // Retournez l'ID de la famille correspondante
                }
                else
                {
                    return -1; 
                }
            }
            catch (Exception ex)
            {
                return -1; 
            }
        }

        public static async Task<(bool Success, string Message)> DeleteFamily(int familyId)
        {
            try
            {
                // Construire l'URL de votre API pour la suppression
                string apiUrl = $"https://localhost:7281/api/Family/{familyId}";
                HttpResponseMessage response = await ApiConnexion.ApiClient.DeleteAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    return (true, "La famille a été supprimée avec succès.");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    string message = await response.Content.ReadAsStringAsync();
                    return (false, "La famille ne peut pas être supprimée car elle est utilisée par au moins un produit.");
                }
                else
                {
                    return (false, $"La suppression de la famille a échoué avec le code d'erreur : {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Une erreur s'est produite lors de la suppression : {ex.Message}");
            }
        }
        public static async Task<(bool Success, string Message)> PostFamily(string familyName)
        {
            try
            {
                string apiUrl = "https://localhost:7281/api/Family";
                var data = new { Name = familyName };
                string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response =  await ApiConnexion.ApiClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return (true, "Famille ajoutée avec succès!");
                }
                else
                {
                    // Lisez le contenu de la réponse pour obtenir le message d'erreur
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    return (false, $"Erreur lors de l'ajout de la famille: {errorMessage}");
                }               
            }
            catch (Exception ex)
            {
                return (false, $"Une erreur s'est produite: {ex.Message}");
            }
        }
        public static async Task<(bool Success, string Message)> PutFamily(int familyId, string familyNameNew)
        {
            try
            {
                var jsonData = new { Id = familyId, Name = familyNameNew };
                string jsonString = JsonConvert.SerializeObject(jsonData);
                string apiUrl = $"https://localhost:7281/api/Family/{familyId}";
                HttpResponseMessage response = await ApiConnexion.ApiClient.PutAsJsonAsync(apiUrl, jsonData);

                if (response.IsSuccessStatusCode)
                {
                    return (true, $"La famille '{familyNameNew}' a été modifiée avec succès!");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    var conflictResponse = await response.Content.ReadAsStringAsync();
                    return (false, conflictResponse); // Utilisez le message de conflit retourné par le serveur
                }
                else
                {
                    return (false, "Une erreur s'est produite lors de la mise à jour.");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Une erreur s'est produite lors de la modification : {ex.Message}");
            }
        }

    }
}
