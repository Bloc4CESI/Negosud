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

namespace VinStore.View
{
    /// <summary>
    /// Logique d'interaction pour EditInventory.xaml
    /// </summary>
    public partial class EditInventory : UserControl
    {
        private Grid _mainGrid;
        public EditInventory(Grid mainGrid)
        {
            InitializeComponent();
            _mainGrid = mainGrid;
            DataContext = new Inventory();
        }
        public void UpdateInventory(Inventory Inventory)
        {
            DataContext = Inventory;
        }
        private void TextBox_IntegerInput(object sender, TextCompositionEventArgs e)
        {
            // Vérifier si le texte est un entier
            if (!int.TryParse(e.Text, out _))
            {
                MessageBox.Show("Veuillez entrer un nombre entier valide dans la quantité.");
                e.Handled = true; // Ignorer si pas un entier
            }
        }
        private async void UpdateInventory(object sender, RoutedEventArgs e)
        {
            if (DataContext is Inventory inventory)
            {
                if (inventory.InventoryLignes!.Any(InventoryLigne => InventoryLigne.QuantityInventory <= 0 || string.IsNullOrEmpty(InventoryLigne.QuantityInventory.ToString())))
                {
                    MessageBox.Show("Veuillez entrer une quantité valide (supérieure à 0).", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                inventory.StatusInventory = Inventory.InventoryEnum.VALIDE;
                var updateInventory = await InventoryService.UpdateInventory(inventory);
                if (!string.IsNullOrEmpty(updateInventory))
                {
                    var message = MessageBox.Show(updateInventory, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    if (message == MessageBoxResult.OK)
                    {
                        _mainGrid.Children.Clear();
                        _mainGrid.Children.Add(new ValidateInventory(_mainGrid));
                    }
                }
                else
                {
                    MessageBox.Show("Une erreur s'est produite", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private async void RefuseInventory(object sender, RoutedEventArgs e)
        {
            if (DataContext is Inventory inventory)
            {
                if (inventory.InventoryLignes!.Any(InventoryLigne => string.IsNullOrEmpty(InventoryLigne.QuantityInventory.ToString())))
                {
                    MessageBox.Show("Veuillez entrer une quantité valide.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                inventory.StatusInventory = Inventory.InventoryEnum.REFUSE;
            var UpdatedInventory = await InventoryService.UpdateInventory(inventory);
                if (!string.IsNullOrEmpty(UpdatedInventory))
                {
                    var Message = MessageBox.Show(UpdatedInventory, "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                    if (Message == MessageBoxResult.OK)
                    {
                        _mainGrid.Children.Clear();
                        _mainGrid.Children.Add(new RefusedInventory(_mainGrid));
                    }
                }
                else
                {
                    MessageBox.Show("Une erreur s'est produite", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
