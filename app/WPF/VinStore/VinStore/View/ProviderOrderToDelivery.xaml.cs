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
    /// Logique d'interaction pour ProviderOrderToDelivery.xaml
    /// </summary>
    public partial class ProviderOrderToDelivery : UserControl
    {
        private Grid _mainGrid;
        public ProviderOrderToDelivery(Grid mainGrid)
        {
            InitializeComponent();
            _mainGrid = mainGrid;
            LoadOrderProvierToDelivry();
        }
        private async void LoadOrderProvierToDelivry()
        {
            var providerOrdersToDelivry = await CommandProviderService.GetProviderOrderByStatus(ProviderOrderStatus.VALIDE);
            foreach (var order in providerOrdersToDelivry)
            {
                if (order.Provider != null && order.ProviderOrderLines != null)
                {
                    order.ProductNames = string.Join(", ", order.ProviderOrderLines.Select(line => line.Product?.Name));
                }
            }
            CommanToDelivryGrid.ItemsSource = providerOrdersToDelivry;
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
                CommanToDelivryGrid.ItemsSource = providerOrdersToValidate;
            }
            else
            {
                LoadOrderProvierToDelivry();
            }
        }
        private async void DelivryCommand(object sender, RoutedEventArgs e)
        {
            var selectedOrderProvider = ((FrameworkElement)sender).DataContext as ProviderOrder;
            if (selectedOrderProvider != null)
            {
                selectedOrderProvider.ProviderOrderStatus = ProviderOrderStatus.LIVRE;
                var updateOrder = await CommandProviderService.UpdateProviderOrder(selectedOrderProvider);
                if (!string.IsNullOrEmpty(updateOrder))
                {
                    var Message = MessageBox.Show(updateOrder, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    if (Message == MessageBoxResult.OK)
                    {
                        _mainGrid.Children.Clear();
                        _mainGrid.Children.Add(new DelivredCommand(_mainGrid));
                    }
                }
                else
                {
                    MessageBox.Show("Une erreur s'est produite", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void ShowCommandDetails_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrderProvider = ((FrameworkElement)sender).DataContext as ProviderOrder;
            var detailScreen = new DetailCommand();
            if (selectedOrderProvider != null)
            {
                detailScreen.DetailProviderOrder(selectedOrderProvider);
                _mainGrid.Children.Clear();
                _mainGrid.Children.Add(detailScreen);

            }
        }
    }
}
