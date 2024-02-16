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
            var calidatedInventories = await InventoryService.GetInventoriesByStatus(InventoryEnum.VALIDE);
            InventoryValidatGrid.ItemsSource = calidatedInventories;
        }
        private async void SearchInventoryByDate(object sender, RoutedEventArgs e)
        {
            var selectedInventoryDate = OrderDate.SelectedDate;
            if (selectedInventoryDate.HasValue)
            {
                var inventories = await InventoryService.GetInventoriesByStatus(InventoryEnum.ENCOURSDEVALIDATION, selectedInventoryDate);
                if (inventories != null)
                {
                    InventoryValidatGrid.ItemsSource = inventories;
                }
            }
            else
            {
                LoadValidateInventory();
            }
        }
        private void ShowCommandDetails_Click(object sender, RoutedEventArgs e)
        {
            var selectedInventory = ((FrameworkElement)sender).DataContext as Inventory;
            var detailScreen = new DetailInventory();
            if (selectedInventory != null)
            {
                detailScreen.DetailInventoryGrid(selectedInventory);
                _mainGrid.Children.Clear();
                _mainGrid.Children.Add(detailScreen);

            }
        }
    }
}
