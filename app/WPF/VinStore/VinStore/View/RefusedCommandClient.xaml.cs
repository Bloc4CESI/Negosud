﻿using System;
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
    /// Logique d'interaction pour RefusedCommandClient.xaml
    /// </summary>
    public partial class RefusedCommandClient : UserControl
    {
        private Grid _mainGrid;
        public RefusedCommandClient (Grid mainGrid)
        {
            InitializeComponent();
            _mainGrid = mainGrid;
            LoadRefusedCommandClient();
        }
        private async void SearchCommandClientByDate(object sender, RoutedEventArgs e)
        {
            var selectedDateCommand = OrderDate.SelectedDate;
            if (selectedDateCommand.HasValue)
            {
                var commands = await CommandClientService.GetClientOrderByStatus(OrderStatus.REFUSE, selectedDateCommand);
                if (commands != null)
                {
                    foreach (var order in commands)
                    {
                        if (order.ClientOrderLines != null)
                        {
                            order.ProductNames = string.Join(", ", order.ClientOrderLines.Select(line => line.Product?.Name));
                        }
                    }
                    RefusedCommandClientGrid.ItemsSource = commands;
                }
            }
            else
            {
                LoadRefusedCommandClient();
            }
        }
        private async void LoadRefusedCommandClient()
        {
            var livredCommands = await CommandClientService.GetClientOrderByStatus(OrderStatus.REFUSE);
            foreach (var order in livredCommands)
            {
                if (order.ClientOrderLines != null)
                {
                    order.ProductNames = string.Join(", ", order.ClientOrderLines.Select(line => line.Product?.Name));
                }
            }
            RefusedCommandClientGrid.ItemsSource = livredCommands;
        }

        private void ShowCommandDetails_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrderClient = ((FrameworkElement)sender).DataContext as ClientOrder;
            var detailScreen = new DetailCommandClient();
            if (selectedOrderClient != null)
            {
                detailScreen.DetailClientOrder(selectedOrderClient);
                _mainGrid.Children.Clear();
                _mainGrid.Children.Add(detailScreen);
            }
        }
    }
}
