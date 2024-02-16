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
    /// Logique d'interaction pour RefusedOrderProvider.xaml
    /// </summary>
    public partial class RefusedOrderProvider : UserControl
    {
        private Grid _mainGrid;
        public RefusedOrderProvider(Grid mainGrid)
        {
            InitializeComponent();
            LoadRefusedOrderProvier();
            _mainGrid = mainGrid;
        }
        private async void LoadRefusedOrderProvier()
        {
            var providerOrdersToDelivry = await CommandProviderService.GetProviderOrderByStatus(ProviderOrderStatus.REFUSE);
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
                var providerOrdersToValidate = await CommandProviderService.GetProviderOrderByStatus(ProviderOrderStatus.REFUSE, providerName);
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
                LoadRefusedOrderProvier();
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
