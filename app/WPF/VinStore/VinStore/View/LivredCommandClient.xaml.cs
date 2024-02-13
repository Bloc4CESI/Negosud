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
    /// Logique d'interaction pour LivredCommandClient.xaml
    /// </summary>
    public partial class LivredCommandClient : UserControl
    {
        private Grid _mainGrid;
        public LivredCommandClient(Grid mainGrid)
        {
            InitializeComponent();
            _mainGrid = mainGrid;
            LoadLivredCommandClient();
        }
        private async void SearchCommandClientByDate(object sender, RoutedEventArgs e)
        {
            var SelectedDateCommand = OrderDate.SelectedDate;
            if (SelectedDateCommand.HasValue)
            {
                var commands = await CommandClientService.GetClientOrderByStatus(OrderStatus.LIVRE, SelectedDateCommand);
                if (commands != null)
                {
                    foreach (var order in commands)
                    {
                        if (order.ClientOrderLines != null)
                        {
                            order.ProductNames = string.Join(", ", order.ClientOrderLines.Select(line => line.Product?.Name));
                        }
                    }
                    LivredCommandClientGrid.ItemsSource = commands;
                }
            }
        }
        private async void LoadLivredCommandClient()
        {
            var LivredCommands = await CommandClientService.GetClientOrderByStatus(OrderStatus.LIVRE);
            foreach (var order in LivredCommands)
            {
                if (order.ClientOrderLines != null)
                {
                    order.ProductNames = string.Join(", ", order.ClientOrderLines.Select(line => line.Product?.Name));
                }
            }
            LivredCommandClientGrid.ItemsSource = LivredCommands;
        }

        private void ShowCommandDetails_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrderClient = ((FrameworkElement)sender).DataContext as ClientOrder;
            var DetailScreen = new DetailCommandClient();
            if (selectedOrderClient != null)
            {
                DetailScreen.DetailClientOrder(selectedOrderClient);
                _mainGrid.Children.Clear();
                _mainGrid.Children.Add(DetailScreen);
            }
        }
    }
}
