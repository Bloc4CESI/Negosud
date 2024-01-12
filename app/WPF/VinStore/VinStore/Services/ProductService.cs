﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows;
using ApiNegosud.Models;
using System.Collections.ObjectModel;

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
                var response = await ApiConnexion.ApiClient.GetStringAsync($"https://localhost:7281/api/product/");
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

    }
}
