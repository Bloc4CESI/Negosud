using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows;
using ApiNegosud.Models;

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


    }
}
