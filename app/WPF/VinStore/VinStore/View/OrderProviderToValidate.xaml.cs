﻿using System;
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
            var ProviderOrdersToValidate = await CommandProviderService.GetProviderOrderByStatus(ProviderOrderStatus.ENCOURSDEVALIDATION);
            foreach (var order in ProviderOrdersToValidate)
            {
                if(order.Provider!= null && order.ProviderOrderLines != null)
                {
                    order.ProductNames = string.Join(", ", order.ProviderOrderLines.Select(line => line.Product?.Name));
                }              
            }
            CommanToValidatGrid.ItemsSource = ProviderOrdersToValidate;
        }
        private void EditOrderProvider(object sender, RoutedEventArgs e)
        {
            var selectedOrderProvider = ((FrameworkElement)sender).DataContext as ProviderOrder;
            var EditScreen = new EditOrderProvider();
            if(selectedOrderProvider != null)
            {
           
                EditScreen.UpdateProviderOrder(selectedOrderProvider);
                _mainGrid.Children.Clear();
                _mainGrid.Children.Add(EditScreen);

            }
        }

        private async void RemoveOrderProvider(object sender, RoutedEventArgs e)
        {
            var selectedOrderProvider = ((FrameworkElement)sender).DataContext as ProviderOrder;
            if (selectedOrderProvider != null)
            {
                var Message = MessageBox.Show($"Êtes-vous sûr de supprimer la commande ?", "Confirmation de suppression", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Message != MessageBoxResult.Yes)
                {
                    await CommandProviderService.DeleteProvider(selectedOrderProvider.Id);
                     LoadOrderProvierToValidate();
                }
            }
        }
    }
}
