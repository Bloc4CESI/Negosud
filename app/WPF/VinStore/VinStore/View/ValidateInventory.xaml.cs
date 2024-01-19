using System;
using System.Collections.Generic;
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
using ApiNegosud.Models;
using VinStore.Services;
using static ApiNegosud.Models.Inventory;

namespace VinStore.View
{
    /// <summary>
    /// Logique d'interaction pour ValidateInventory.xaml
    /// </summary>
    public partial class ValidateInventory : UserControl
    {
        private Grid _mainGrid;
        public ValidateInventory(Grid mainGrid)
        {
            InitializeComponent();
            _mainGrid = mainGrid;
            LoadValidateInventory();
        }
        private async void LoadValidateInventory()
        {
            var ValidatedInventories = await InventoryService.GetInventoriesByStatus(InventoryEnum.VALIDE);
            InventoryValidatGrid.ItemsSource = ValidatedInventories;
        }
        private async void SearchInventoryByDate(object sender, RoutedEventArgs e)
        {
            var SelectedInventoryDate = OrderDate.SelectedDate;
            if (SelectedInventoryDate.HasValue)
            {
                var inventories = await InventoryService.GetInventoriesByStatus(InventoryEnum.ENCOURSDEVALIDATION, SelectedInventoryDate);
                if (inventories != null)
                {
                    InventoryValidatGrid.ItemsSource = inventories;
                }
            }
        }
        private void ShowCommandDetails_Click(object sender, RoutedEventArgs e)
        {
            var SelectedInventory = ((FrameworkElement)sender).DataContext as Inventory;
            var DetailScreen = new DetailInventory();
            if (SelectedInventory != null)
            {
                DetailScreen.DetailInventoryGrid(SelectedInventory);
                _mainGrid.Children.Clear();
                _mainGrid.Children.Add(DetailScreen);

            }
        }
    }
}
