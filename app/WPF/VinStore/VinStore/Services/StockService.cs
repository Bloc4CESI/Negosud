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
using System.Globalization;
using System.Windows.Data;
using System.Net.Http.Json;
using Newtonsoft.Json.Serialization;
using System.ComponentModel;

namespace VinStore.Services
{
    public static class StockService
    {
        public static async Task<List<Stock>> GetAllStock()
        {
            try
            {
                var response = await ApiConnexion.ApiClient.GetStringAsync($"https://localhost:7281/api/stock/");
                if (string.IsNullOrEmpty(response))
                {
                    Console.WriteLine("Aucun produit trouvé");
                    MessageBox.Show("Aucun produit trouvé");
                    return null;
                }
                else
                {
                    var stocks = JsonConvert.DeserializeObject<List<Stock>>(response);
                    return stocks;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la requête : {ex.Message}");
                return null;
            }
        }


        public static async Task<bool> PutStock(int StockId, Stock selectedStock)
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
                var t = selectedStock.AutoOrder;
                var jsonData = JsonConvert.SerializeObject(new
                {
                    selectedStock.Id,
                    selectedStock.Quantity,
                    selectedStock.Minimum,
                    selectedStock.Maximum,
                    selectedStock.AutoOrder,
                    selectedStock.ProductId
                }, settings);

                string apiUrl = $"https://localhost:7281/api/Stock/{StockId}";

                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await ApiConnexion.ApiClient.PutAsync(apiUrl, content);

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
            catch (Exception ex)
            {
                // Gérez les erreurs ici
                MessageBox.Show($"Une erreur s'est produite lors de la modification : {ex.Message}");
                return false;
            }

        }

        public static async Task<bool> InitStock(int ProductId)
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
                var jsonData = "{\"quantity\": 0,\"minimum\": 0,\"maximum\": 0,\"autoOrder\": false,\"productId\": "+ ProductId +"}";

                string apiUrl = $"https://localhost:7281/api/Stock";

                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await ApiConnexion.ApiClient.PostAsync(apiUrl, content);

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
            catch (Exception ex)
            {
                // Gérez les erreurs ici
                MessageBox.Show($"Une erreur s'est produite lors de la modification : {ex.Message}");
                return false;
            }
        }
    }

    public class BooleanToInverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class StockItem : INotifyPropertyChanged
    {
        private bool _autoOrder;

        public bool AutoOrder
        {
            get { return _autoOrder; }
            set
            {
                if (_autoOrder != value)
                {
                    _autoOrder = value;
                    OnPropertyChanged(nameof(AutoOrder));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
