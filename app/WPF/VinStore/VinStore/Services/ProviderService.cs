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

        public static async Task<List<Provider>> GetProvidersAll(string name = null)
        {
            try
            {
                string url = $"https://localhost:7281/api/Provider";
                // Ajouter le paramètre de requête pour le filtre par nom si non null
                if (!string.IsNullOrEmpty(name))
                {
                    url += $"?name={Uri.EscapeDataString(name)}";
                }
                var response = await ApiConnexion.ApiClient.GetStringAsync(url);
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

        public static async Task<(bool Success, string Message)> DeleteProvider(int providerId)
        {
            try
            {
                string apiUrl = $"https://localhost:7281/api/Provider/{providerId}";
                // Effectuez la requête DELETE
                HttpResponseMessage response = await ApiConnexion.ApiClient.DeleteAsync(apiUrl);
                // Vérifiez si la requête a réussi

                if (response.IsSuccessStatusCode)
                {
                    return (true, "Le fournisseur a été supprimée avec succès.");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    string message = await response.Content.ReadAsStringAsync();
                    return (false, "Le fournisseur a des produits associés. Suppression annulée.");
                }
                else
                {
                    return (false, $"La suppression du fournisseur a échoué avec le code d'erreur : {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Gérez les erreurs ici
                return (false, $"Une erreur s'est produite lors de la suppression : {ex.Message}");
            }
        }
        public static async Task<(bool Success, string Message)> PostProvider(Provider newProvider)
        {
            try
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
                    newProvider.Name,
                    newProvider.PhoneNumber,
                    newProvider.Email,
                    newProvider.Address,
                });
                string apiUrl = $"https://localhost:7281/api/Provider/PostWithAddress";
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await ApiConnexion.ApiClient.PostAsync(apiUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Le fournisseur est ajouté avec succès!");
                }
                else
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    return (false, $"Erreur {response.StatusCode}: {errorResponse}");
                }
            }
            catch (Exception ex)
            {
                // Gérez les erreurs ici
                return (false, $"Une erreur s'est produite lors de la modification : {ex.Message}");
            }
        }
        public static async Task<(bool Success, string Message)> PutProvider(int providerId, Provider putProvider)
        {
            try
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
                }, settings);
                string apiUrl = $"https://localhost:7281/api/Provider/{providerId}";
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await ApiConnexion.ApiClient.PutAsync(apiUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Le fournisseur a été mis à jour avec succès!");
                }
                else
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    return (false, $"Erreur {response.StatusCode}: {errorResponse}");
                }

            }
            catch (Exception ex)
            {
                return (false, $"Une erreur s'est produite lors de la modification : {ex.Message}");
            }
        }
    }
}

