using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows;
using ApiNegosud.Models;
using Newtonsoft.Json.Serialization;

namespace VinStore.Services
{
    public static class ProviderService
    {
        public static async Task<List<Provider>> GetProviders()
        {
            try
            {
                var response = await ApiConnexion.ApiClient.GetStringAsync($"https://localhost:7281/api/Provider/GetOnlyPrivdersWithProducts");
                if (string.IsNullOrEmpty(response))
                {
                    Console.WriteLine($"Aucun fournisseur trouvé ");
                    MessageBox.Show($"Aucun fournisseur trouvé ");
                    return null;
                }
                else
                {
                    var providers = JsonConvert.DeserializeObject<List<Provider>>(response);
                    return providers;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la requête : {ex.Message}");
                return null;
            }
        }

        public static async Task<List<Provider>> GetProvidersAll()
        {
            try
            {
                var response = await ApiConnexion.ApiClient.GetStringAsync($"https://localhost:7281/api/Provider");
                if (string.IsNullOrEmpty(response))
                {
                    Console.WriteLine($"Aucun fournisseur trouvé ");
                    MessageBox.Show($"Aucun fournisseur trouvé ");
                    return null;
                }
                else
                {
                    var providers = JsonConvert.DeserializeObject<List<Provider>>(response);
                    return providers;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la requête : {ex.Message}");
                return null;
            }
        }

        public static async Task<bool> DeleteProvider(int providerId)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Construisez l'URL de votre API pour la suppression
                    string apiUrl = $"https://localhost:7281/api/Provider/{providerId}";

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

        public static async Task<bool> PostProvider(Provider NewProvider)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var settings = new JsonSerializerSettings
                    {
                        ContractResolver = new DefaultContractResolver
                        {
                            NamingStrategy = new CamelCaseNamingStrategy()
                        }
                    };
                    var jsonData = JsonConvert.SerializeObject(new
                    {                        
                        NewProvider.Name,
                        NewProvider.PhoneNumber,
                        NewProvider.Email,
                        NewProvider.Address,
                    });
                    string apiUrl = $"https://localhost:7281/api/Provider/PostWithAddress";
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    if (response.IsSuccessStatusCode)
                    {
                        // La requête a réussi
                        return true;
                    }
                    else
                    {
                        // La requête a échoué, examinez les détails de l'erreur si possible
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Erreur {response.StatusCode}: {errorResponse}");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Gérez les erreurs ici
                MessageBox.Show($"Une erreur s'est produite lors de la modification : {ex.Message}");
                return false;
            }

        }

        public static async Task<bool> PutProvider(int providerId, Provider putProvider)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var settings = new JsonSerializerSettings
                    {
                        ContractResolver = new DefaultContractResolver
                        {
                            NamingStrategy = new CamelCaseNamingStrategy()
                        }
                    };
                    var jsonData = JsonConvert.SerializeObject(new
                    {
                        putProvider.Id,
                        putProvider.Name,
                        putProvider.PhoneNumber,
                        putProvider.Email,
                        putProvider.Address,
                    },settings);
                    string apiUrl = $"https://localhost:7281/api/Provider/{providerId}";
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PutAsync(apiUrl, content);
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        // La requête a échoué, examinez les détails de l'erreur si possible
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Erreur {response.StatusCode}: {errorResponse}");
                        return false;
                    }
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

