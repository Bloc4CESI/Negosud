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
            var InventoriesToValidate = await InventoryService.GetInventoriesByStatus(InventoryEnum.ENCOURSDEVALIDATION);
            InventoryValidatGrid.ItemsSource = InventoriesToValidate;
        }
        private void EditInventory(object sender, RoutedEventArgs e)
        {
            var SelectedInventory = ((FrameworkElement)sender).DataContext as Inventory;
            var EditScreen = new EditInventory(_mainGrid);
            if (SelectedInventory != null)
            {
                EditScreen.UpdateInventory(SelectedInventory);
                _mainGrid.Children.Clear();
                _mainGrid.Children.Add(EditScreen);

            }
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
        private async void RemoveInventory(object sender, RoutedEventArgs e)
        {
            var selectedInventory = ((FrameworkElement)sender).DataContext as Inventory;
            if (selectedInventory != null)
            {
                var Message = MessageBox.Show($"Êtes-vous sûr de supprimer l'inventaire ?", "Confirmation de suppression", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Message == MessageBoxResult.Yes)
                {
                    await InventoryService.DeleteInventory(selectedInventory.Id);
                    LoadInventoriesToValidate();
                }
            }
        }
    }
}
