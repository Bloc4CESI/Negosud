using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows;
using ApiNegosud.Models;
using System.Collections.ObjectModel;
using VinStore.View;

namespace VinStore.Services
{
    public static class ProductService
    {
        public static async Task<List<Product>> GetProductByProvider(int providerId)
        {
            try
            {
                var response = await ApiConnexion.ApiClient.GetStringAsync($"https://localhost:7281/api/Product/GetProductByProvider/{providerId}");
                if (string.IsNullOrEmpty(response))
                {
                    Console.WriteLine("Aucun produit trouvé pour ce fournisseur.");
                    MessageBox.Show("Aucun produit trouvé pour ce fournisseur.");
                    return null;
                }
                else
                {
                    var products = JsonConvert.DeserializeObject<List<Product>>(response);
                    return products;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la requête : {ex.Message}");
                return null;
            }
        }
        public static async Task<Product> GetProductById(int ProductId)
        {
            try
            {
                var response = await ApiConnexion.ApiClient.GetStringAsync($"https://localhost:7281/api/Product/{ProductId}");
                if (string.IsNullOrEmpty(response))
                {
                    Console.WriteLine("Aucun produit trouvé");
                    MessageBox.Show("Aucun produit trouvé");
                    return null;
                }
                else
                {
                    var product = JsonConvert.DeserializeObject<Product>(response);
                    return product;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la requête : {ex.Message}");
                return null;
            }
        }

        public static async Task<List<Product>> GetAllProducts()
        {
            try
            {
                var response = await ApiConnexion.ApiClient.GetStringAsync($"https://localhost:7281/api/Product/");
                if (string.IsNullOrEmpty(response))
                {
                    Console.WriteLine("Aucun produit trouvé");
                    MessageBox.Show("Aucun produit trouvé");
                    return null;
                }
                else
                {
                    List<Product> products = JsonConvert.DeserializeObject<List<Product>>(response);
                    return products;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la requête : {ex.Message}");
                return null;
            }
        }

        public static async Task EditProduct(string jsonData, int id, string productName)
        {
            try
            {
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Envoyer la requête PUT
                HttpResponseMessage response = await ApiConnexion.ApiClient.PutAsync($"https://localhost:7281/api/Product/{id}", content);

                // Vérifier la réponse
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Mise à jour réussie !");
                    MessageBox.Show($"Le produit \"{productName}\" à bien été mis à jour !");
                }
                else
                {
                    Console.WriteLine($"Erreur lors de la mise à jour. Code de statut : {response.StatusCode}");
                    // Vous pouvez également lever une exception ici si nécessaire.
                    MessageBox.Show($"La mise à jour du produit \"{productName}\" à échoué !");
                }
            }
            catch (Exception ex)
            {
                // Propager l'exception pour être gérée par l'appelant
                throw new Exception($"Une erreur s'est produite lors de la requête : {ex.Message}", ex);
            }
        }

        public static async Task DeleteProduct(int id, string productName)
        {
            try
            {
                // Envoyer la requête PUT
                HttpResponseMessage response = await ApiConnexion.ApiClient.DeleteAsync($"https://localhost:7281/api/Product/{id}");

                // Vérifier la réponse
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Suppression réussie !");
                    MessageBox.Show($"Le produit \"{productName}\" à bien été supprimé !");
                }
                else
                {
                    Console.WriteLine($"Erreur lors de la suppression. Code de statut : {response.StatusCode}");
                    // Vous pouvez également lever une exception ici si nécessaire.
                    MessageBox.Show($"La suppression du produit \"{productName}\" à échoué !");
                }
            }
            catch (Exception ex)
            {
                // Propager l'exception pour être gérée par l'appelant
                throw new Exception($"Une erreur s'est produite lors de la requête : {ex.Message}", ex);
            }
        }
        public static async Task PostProduct(string jsonData, string productName)
        {
            try
            {

                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                // Envoyer la requête PUT
                HttpResponseMessage response = await ApiConnexion.ApiClient.PostAsync($"https://localhost:7281/api/Product/", content);
               

                // Vérifier la réponse
                if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Création réussie !");
                        MessageBox.Show($"Le produit \"{productName}\" à bien été créé !");
                    }
                    else
                    {
                        Console.WriteLine($"Erreur lors de la suppression. Code de statut : {response.StatusCode}");
                        // Vous pouvez également lever une exception ici si nécessaire.
                        MessageBox.Show($"La création du produit \"{productName}\" à échoué !");
                    }

                }
            catch (Exception ex)
            {
                // Gérez les erreurs ici
                MessageBox.Show($"Une erreur s'est produite: {ex.Message}");
            }
        }
        public static async Task<List<Product>> GetAlertProducts()
        {
            try
            {
                var response = await ApiConnexion.ApiClient.GetStringAsync($"https://localhost:7281/api/Product/GetAlertProduct");
                if (string.IsNullOrEmpty(response))
                {
                    Console.WriteLine("Aucun produit trouvé");
                    MessageBox.Show("Aucun produit trouvé");
                    return null;
                }
                else
                {
                    List<Product> products = JsonConvert.DeserializeObject<List<Product>>(response);
                    return products;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la requête : {ex.Message}");
                return null;
            }
        }
        public static async Task<bool> IsProductHasAnyTransaction(int idProduct)
        {
            try
            {
                var response = await ApiConnexion.ApiClient.GetAsync($"https://localhost:7281/api/Product/IsProductHasAnyTransaction/{idProduct}");
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return Convert.ToBoolean(responseContent);
                }
                else
                {
                    return false;
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la requête : {ex.Message}");
                return false;
            }
        }
    }

}
