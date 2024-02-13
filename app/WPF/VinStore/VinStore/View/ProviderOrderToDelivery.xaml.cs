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
            var ProviderOrdersToDelivry = await CommandProviderService.GetProviderOrderByStatus(ProviderOrderStatus.VALIDE);
            foreach (var order in ProviderOrdersToDelivry)
            {
                if (order.Provider != null && order.ProviderOrderLines != null)
                {
                    order.ProductNames = string.Join(", ", order.ProviderOrderLines.Select(line => line.Product?.Name));
                }
            }
            CommanToDelivryGrid.ItemsSource = ProviderOrdersToDelivry;
        }
        private async void SearchCommandWithProviderName(object sender, RoutedEventArgs e)
        {
            var ProviderName = ProviderNameTextBox.Text;

            if (!string.IsNullOrEmpty(ProviderName))
            {
                var ProviderOrdersToValidate = await CommandProviderService.GetProviderOrderByStatus(ProviderOrderStatus.ENCOURSDEVALIDATION, ProviderName);
                foreach (var order in ProviderOrdersToValidate)
                {
                    if (order.Provider != null && order.ProviderOrderLines != null)
                    {
                        order.ProductNames = string.Join(", ", order.ProviderOrderLines.Select(line => line.Product?.Name));
                    }
                }
                CommanToDelivryGrid.ItemsSource = ProviderOrdersToValidate;
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
                        _mainGrid.Children.Add(new LivredCommandClient(_mainGrid));
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
            var DetailScreen = new DetailCommand();
            if (selectedOrderProvider != null)
            {
                DetailScreen.DetailProviderOrder(selectedOrderProvider);
                _mainGrid.Children.Clear();
                _mainGrid.Children.Add(DetailScreen);

            }
        }
    }
}
