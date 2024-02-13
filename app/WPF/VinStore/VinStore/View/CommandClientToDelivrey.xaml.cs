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
    /// Logique d'interaction pour CommandClientToDelivrey.xaml
    /// </summary>
    public partial class CommandClientToDelivrey : UserControl
    {
        private Grid _mainGrid;
        public CommandClientToDelivrey(Grid mainGrid)
        {

            InitializeComponent();
            _mainGrid = mainGrid;
            LoadCommandClientToValidate();
        }
        private async void SearchCommandClientByDate(object sender, RoutedEventArgs e)
        {
            var SelectedDateCommand = OrderDate.SelectedDate;
            if (SelectedDateCommand.HasValue)
            {
                var commands = await CommandClientService.GetClientOrderByStatus(OrderStatus.VALIDE, SelectedDateCommand);
                if (commands != null)
                {
                    foreach (var order in commands)
                    {
                        if (order.ClientOrderLines != null)
                        {
                            order.ProductNames = string.Join(", ", order.ClientOrderLines.Select(line => line.Product?.Name));
                        }
                    }
                    CommandClientToDelivryGrid.ItemsSource = commands;
                }
            }
        }
        private async void LoadCommandClientToValidate()
        {
            var CommandsToValidate = await CommandClientService.GetClientOrderByStatus(OrderStatus.VALIDE);
            foreach (var order in CommandsToValidate)
            {
                if (order.ClientOrderLines != null)
                {
                    order.ProductNames = string.Join(", ", order.ClientOrderLines.Select(line => line.Product?.Name));
                }
            }
            CommandClientToDelivryGrid.ItemsSource = CommandsToValidate;
        }
        private async void DelivryCommand(object sender, RoutedEventArgs e)
        {
            var selectedOrderClient = ((FrameworkElement)sender).DataContext as ClientOrder;
            if (selectedOrderClient != null)
            {
                selectedOrderClient.OrderStatus = OrderStatus.LIVRE;
                var updateOrder = await CommandClientService.UpdateClientOrder(selectedOrderClient);
                if (!string.IsNullOrEmpty(updateOrder))
                {
                    var Message = MessageBox.Show(updateOrder, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    if (Message == MessageBoxResult.OK)
                    {
                        _mainGrid.Children.Clear();
                        _mainGrid.Children.Add(new CommandClientToDelivrey(_mainGrid));
                    }
                }
                else
                {
                    MessageBox.Show("Une erreur s'est produite", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private async void RefuseCommand(object sender, RoutedEventArgs e)
        {
            if (DataContext is ClientOrder ClientOrder)
            {
                ClientOrder.OrderStatus = OrderStatus.REFUSE;              
                var updateOrder = await CommandClientService.UpdateClientOrder(ClientOrder);
                if (!string.IsNullOrEmpty(updateOrder))
                {
                    var Message = MessageBox.Show(updateOrder, "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                    if (Message == MessageBoxResult.OK)
                    {
                        _mainGrid.Children.Clear();
                        _mainGrid.Children.Add(new RefusedCommandClient(_mainGrid));
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
