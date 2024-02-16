using System;
using ApiNegosud.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using VinStore.Services;
using System.ComponentModel;
using Newtonsoft.Json;

namespace VinStore.View
{
    /// <summary>
    /// Logique d'interaction pour StockView.xaml
    /// </summary>
    public partial class StockView : UserControl
    {
        private ObservableCollection<Stock> stocks;
        public StockView()
        {
            InitializeComponent();
            stocks = new ObservableCollection<Stock>();
            InitializeDataStock();
        }
        private async void InitializeDataStock()
        {
            // Créez une liste de stock
            List<Stock> stockList = await StockService.GetAllStock();
            // Assigne la liste à la propriété ItemsSource du DataGrid
            StockDataGrid.ItemsSource = stockList;
            Dispatcher.Invoke(() => { }, DispatcherPriority.Render);

        }
        private void TextBox_IntegerInput(object sender, TextCompositionEventArgs e)
        {
            // Vérifier si le texte est un entier
            if (!int.TryParse(e.Text, out _))
            {
                MessageBox.Show("Veuillez entrer un nombre entier valide.");
                e.Handled = true; // Ignorer si pas un entier
            }
        }
        private async void ButtonEdite_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StockDataGrid.CommitEdit();
                // Récupérer l'objet Product associé à la ligne sélectionnée
                var selectedStock = StockDataGrid.SelectedItem as Stock;
                if (selectedStock != null)
                {
                    // Appelez la méthode pour effectuer la modification via l'API
                    bool success = await StockService.PutStock(selectedStock.Id, selectedStock);
                    if (success)
                    {
                        MessageBox.Show($"Le stock '{selectedStock.Product!.Name}' a été modifiée avec succès!");
                        // Rafraîchissez la liste après la modification
                        InitializeDataStock();
                    }
                    else
                    {
                        MessageBox.Show($"La modification de le stock '{selectedStock.Product}' a échoué.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite lors de la modification : {ex.Message}");
            }
        }
        private void myCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Récupérer l'objet Product associé à la ligne sélectionnée
            var selectedRow = StockDataGrid.SelectedItem as Stock; 
            // Vérifiez si une ligne est sélectionnée
            if (selectedRow != null)
            {
                // Mettez à jour la propriété "Minimum" de votre modèle avec la valeur 10
                selectedRow.AutoOrder = true;              
            }
        }

        private void myCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

            StockDataGrid.CommitEdit();
            // Récupérer l'objet Product associé à la ligne sélectionnée
            var selectedRow = StockDataGrid.SelectedItem as Stock; 
            // Vérifiez si une ligne est sélectionnée
            if (selectedRow != null)
            {
                // Mettez à jour la propriété "Minimum" de votre modèle avec la valeur 10
                selectedRow.AutoOrder = false;
            }
        }
        private async void SearchProductWithName(object sender, RoutedEventArgs e)
        {
            var productName = ProductNameTextBox.Text;

            if (!string.IsNullOrEmpty(productName))
            {
                List<Stock> stockList = await StockService.GetAllStock(productName);
                // Assigne la liste à la propriété ItemsSource du DataGrid
                StockDataGrid.ItemsSource = stockList;
            }
            else
            {
                List<Stock> stockList = await StockService.GetAllStock();
                StockDataGrid.ItemsSource = stockList;
            }
        }
    }


}
