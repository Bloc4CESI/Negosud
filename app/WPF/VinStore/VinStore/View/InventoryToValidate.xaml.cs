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
    /// Logique d'interaction pour InventoryToValidate.xaml
    /// </summary>
    public partial class InventoryToValidate : UserControl
    {
        private Grid _mainGrid;
        public InventoryToValidate(Grid mainGrid)
        {
            InitializeComponent();
            _mainGrid = mainGrid;
            LoadInventoriesToValidate();
        }
        private async void LoadInventoriesToValidate()
        {
            var inventoriesToValidate = await InventoryService.GetInventoriesByStatus(InventoryEnum.ENCOURSDEVALIDATION);
            InventoryValidatGrid.ItemsSource = inventoriesToValidate;
        }
        private void EditInventory(object sender, RoutedEventArgs e)
        {
            var selectedInventory = ((FrameworkElement)sender).DataContext as Inventory;
            var EditScreen = new EditInventory(_mainGrid);
            if (selectedInventory != null)
            {
                EditScreen.UpdateInventory(selectedInventory);
                _mainGrid.Children.Clear();
                _mainGrid.Children.Add(EditScreen);
            }
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
                LoadInventoriesToValidate();
            }
        }
        private async void RemoveInventory(object sender, RoutedEventArgs e)
        {
            var selectedInventory = ((FrameworkElement)sender).DataContext as Inventory;
            if (selectedInventory != null)
            {
                var message = MessageBox.Show($"Êtes-vous sûr de supprimer l'inventaire ?", "Confirmation de suppression", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (message == MessageBoxResult.Yes)
                {
                    await InventoryService.DeleteInventory(selectedInventory.Id);
                    LoadInventoriesToValidate();
                }
            }
        }
    }
}
