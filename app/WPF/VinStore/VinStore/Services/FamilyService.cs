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
                Family matchingFamily = familyList.FirstOrDefault(f => f.Name == familyName);

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


        public static async Task<bool> PostFamily(string familyName)
        {
            try
            {
                // Créez un objet HttpClient
                using (HttpClient client = new HttpClient())
                {
                    // URL de votre API
                    string apiUrl = "https://localhost:7281/api/Family";

                    // Créez les données à envoyer (vous pouvez ajuster cela en fonction de votre API)
                    var data = new { Name = familyName };
                    string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(data);

                    // Convertissez les données en bytes
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    // Effectuez la requête POST
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    // Vérifiez si la requête a réussi
                    if (response.IsSuccessStatusCode)
                    {
                        // Traitez la réponse ici si nécessaire
                        return true;
                    }
                    else
                    {
                        // Affichez un message d'erreur si la requête échoue
                        MessageBox.Show($"Erreur de la requête: {response.StatusCode}");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Gérez les erreurs ici
                MessageBox.Show($"Une erreur s'est produite: {ex.Message}");
                return false;
            }
        }

        public static async Task<bool> DeleteFamily(int familyId)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Construisez l'URL de votre API pour la suppression
                    string apiUrl = $"https://localhost:7281/api/Family/{familyId}";

                    // Effectuez la requête DELETE
                    HttpResponseMessage response = await client.DeleteAsync(apiUrl);

                    // Vérifiez si la requête a réussi
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                // Gérez les erreurs ici
                MessageBox.Show($"Une erreur s'est produite lors de la suppression : {ex.Message}");
                return false;
            }
        }
        public static async Task<bool> PutFamily(int familyId, string familyNameNew)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Utilisez la classe anonyme pour faciliter la sérialisation en JSON
                    var jsonData = new { Id = familyId, Name = familyNameNew };

                    // Utilisez une bibliothèque de sérialisation JSON (par exemple, Newtonsoft.Json)
                    string jsonString = JsonConvert.SerializeObject(jsonData);
                    // Construisez l'URL de votre API pour la modification en utilisant l'ID
                    string apiUrl = $"https://localhost:7281/api/Family/{familyId}";

                    // Effectuez la requête PUT avec les données mises à jour
                    HttpResponseMessage response = await client.PutAsJsonAsync(apiUrl, jsonData);

                    // Vérifiez si la requête a réussi
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                // Gérez les erreurs ici
                MessageBox.Show($"Une erreur s'est produite lors de la modification : {ex.Message}");
                return false;
            }

        }

    }
}
