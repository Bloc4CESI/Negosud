using System;
using System.Collections.Generic;
using System.Configuration.Provider;
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
    /// Logique d'interaction pour OrderProviderToValidate.xaml
    /// </summary>
    public partial class OrderProviderToValidate : UserControl
    {
        private Grid _mainGrid;

        public OrderProviderToValidate(Grid mainGrid)
        {
            InitializeComponent();
            _mainGrid = mainGrid;
            LoadOrderProvierToValidate();
        }
        private async void LoadOrderProvierToValidate()
        {
            var providerOrdersToValidate = await CommandProviderService.GetProviderOrderByStatus(ProviderOrderStatus.ENCOURSDEVALIDATION);
            foreach (var order in providerOrdersToValidate)
            {
                if(order.Provider!= null && order.ProviderOrderLines != null)
                {
                    order.ProductNames = string.Join(", ", order.ProviderOrderLines.Select(line => line.Product?.Name));
                }              
            }
            CommanToValidatGrid.ItemsSource = providerOrdersToValidate;
        }
        private void EditOrderProvider(object sender, RoutedEventArgs e)
        {
            var selectedOrderProvider = ((FrameworkElement)sender).DataContext as ProviderOrder;
            var editScreen = new EditOrderProvider(_mainGrid);
            if(selectedOrderProvider != null)
            {           
                editScreen.UpdateProviderOrder(selectedOrderProvider);
                _mainGrid.Children.Clear();
                _mainGrid.Children.Add(editScreen);
            }
        }
        private async void SearchCommandWithProviderName(object sender, RoutedEventArgs e)
        {
            var providerName = ProviderNameTextBox.Text;

            if (!string.IsNullOrEmpty(providerName))
            {
                var providerOrdersToValidate = await CommandProviderService.GetProviderOrderByStatus(ProviderOrderStatus.ENCOURSDEVALIDATION, providerName);
                foreach (var order in providerOrdersToValidate)
                {
                    if (order.Provider != null && order.ProviderOrderLines != null)
                    {
                        order.ProductNames = string.Join(", ", order.ProviderOrderLines.Select(line => line.Product?.Name));
                    }
                }
                CommanToValidatGrid.ItemsSource = providerOrdersToValidate;
            }
            else
            {
                LoadOrderProvierToValidate();
            }
           
        }
        private async void RemoveOrderProvider(object sender, RoutedEventArgs e)
        {
            var selectedOrderProvider = ((FrameworkElement)sender).DataContext as ProviderOrder;
            if (selectedOrderProvider != null)
            {
                var message = MessageBox.Show($"Êtes-vous sûr de supprimer la commande ?", "Confirmation de suppression", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (message == MessageBoxResult.Yes)
                {
                    await CommandProviderService.DeleteProvider(selectedOrderProvider.Id);
                     LoadOrderProvierToValidate();
                }
            }
        }
    }
}
