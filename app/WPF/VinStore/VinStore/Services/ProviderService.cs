﻿using System;
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
    public static class ProviderService
    {
        public static async Task<List<Provider>> GetProviders(string name = null)
        {
            try
            {
                var response = await ApiConnexion.ApiClient.GetStringAsync($"https://localhost:7281/api/Provider/?Name={name}");
                if (string.IsNullOrEmpty(response))
                {
                    Console.WriteLine($"Aucun fournisseur trouvé avec le nom fourni : {name}");
                    MessageBox.Show($"Aucun fournisseur trouvé avec le nom fourni : {name}");
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

    }
}