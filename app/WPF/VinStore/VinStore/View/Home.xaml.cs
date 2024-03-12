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
    /// Logique d'interaction pour Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        private Grid _mainGrid;
        public Home(Grid mainGrid)
        {
            InitializeComponent();
            _mainGrid = mainGrid;
            LoadProductsAlert();
            CommandToDeliver();
            LoadOrderProvierToValidate();
            CommandClientToDeliver();
        }
        private async void LoadProductsAlert()
        {
            var productsAlert = await ProductService.GetAlertProducts();
            if(productsAlert != null) {
                int count = productsAlert.Count;
                CountProductQuantityMin.Text = count.ToString();
            }
          
        }
        private async void LoadOrderProvierToValidate()
        {
            var providerOrdersToValidate = await CommandProviderService.GetProviderOrderByStatus(ProviderOrderStatus.ENCOURSDEVALIDATION);
         if(providerOrdersToValidate != null)
            {
                int count = providerOrdersToValidate.Count;
                CommandProviderToValidate.Text = count.ToString();
            }
           

        }
        private async void CommandToDeliver()
        {
            var validatedProviderOrder = await CommandProviderService.GetProviderOrderByStatus(ProviderOrderStatus.VALIDE);
            int count = validatedProviderOrder.Count;
            CommandProviderToDelivry.Text = count.ToString();
        }
        private async void CommandClientToDeliver()
        {
            var validatedCommandClients = await CommandClientService.GetClientOrderByStatus(OrderStatus.VALIDE);
            int count = validatedCommandClients.Count;
            CommandClientToDelivry.Text = count.ToString();
        }
        private  void ProductList_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _mainGrid.Children.Clear();
            _mainGrid.Children.Add(new ListProduct(true));
        }
        
        private void CommandClientToDelivry_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _mainGrid.Children.Clear();
            _mainGrid.Children.Add(new ProviderOrderToDelivery(_mainGrid));
        }
        private void CommandProviderToValidate_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _mainGrid.Children.Clear();
            _mainGrid.Children.Add(new OrderProviderToValidate(_mainGrid));
        }
        private void CommandClientToDelivrey_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _mainGrid.Children.Clear();
            _mainGrid.Children.Add(new CommandClientToDelivrey(_mainGrid));
        }
    }
}
