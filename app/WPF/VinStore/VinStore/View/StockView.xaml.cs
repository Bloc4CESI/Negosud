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

        private async void ButtonEdite_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                // Terminer l'édition pour s'assurer que les modifications sont prises en compte
                //StockDataGrid.CommitEdit();
                //StockDataGrid.SelectionUnit = DataGridSelectionUnit.FullRow;
                //StockDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                StockDataGrid.CommitEdit();
                //StockDataGrid.SelectAll();
                // Récupérer l'objet Product associé à la ligne sélectionnée
                Stock selectedStock = StockDataGrid.SelectedItem as Stock;

                // Obtenez les nouvelles informations pour la famille (par exemple, à partir d'une boîte de dialogue)
                //StockDataGrid.SelectionUnit = DataGridSelectionUnit.Cell;

                if (selectedStock != null)
                {
                    // Appelez la méthode pour effectuer la modification via l'API
                    bool success = await StockService.PutStock(selectedStock.Id, selectedStock);

                    if (success)
                    {

                        MessageBox.Show($"Le stock '{selectedStock.Product.Name}' a été modifiée avec succès!");
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
            //StockDataGrid.SelectionUnit = DataGridSelectionUnit.FullRow;
            //StockDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            //StockDataGrid.CommitEdit();
            //StockDataGrid.SelectAll();
            // Récupérer l'objet Product associé à la ligne sélectionnée
            var selectedRow = StockDataGrid.SelectedItem as Stock; // Remplacez "VotreClasse" par le nom de votre classe de modèle de données

            // Obtenez les nouvelles informations pour la famille (par exemple, à partir d'une boîte de dialogue)
            //StockDataGrid.SelectionUnit = DataGridSelectionUnit.Cell;

            // Vérifiez si une ligne est sélectionnée
            if (selectedRow != null)
            {
                // Mettez à jour la propriété "Minimum" de votre modèle avec la valeur 10
                selectedRow.AutoOrder = true;

                
              
            }
        }

        private void myCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

            //StockDataGrid.SelectionUnit = DataGridSelectionUnit.FullRow;
           // StockDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            StockDataGrid.CommitEdit();
            //StockDataGrid.SelectAll();
            // Récupérer l'objet Product associé à la ligne sélectionnée
            var selectedRow = StockDataGrid.SelectedItem as Stock; // Remplacez "VotreClasse" par le nom de votre classe de modèle de données

            // Obtenez les nouvelles informations pour la famille (par exemple, à partir d'une boîte de dialogue)
            //StockDataGrid.SelectionUnit = DataGridSelectionUnit.Cell;

            

            // Vérifiez si une ligne est sélectionnée
            if (selectedRow != null)
            {
                // Mettez à jour la propriété "Minimum" de votre modèle avec la valeur 10
                selectedRow.AutoOrder = false;

                // Rafraîchissez la vue pour refléter les changements dans le DataGrid

            }

        }
    }


}
