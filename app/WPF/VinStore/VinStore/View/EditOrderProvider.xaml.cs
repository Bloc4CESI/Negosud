using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Logique d'interaction pour EditOrderProvider.xaml
    /// </summary>
    public partial class EditOrderProvider : UserControl
    {
        private Grid _mainGrid;
        public EditOrderProvider(Grid mainGrid)
        {
            InitializeComponent();
            _mainGrid = mainGrid;
            DataContext = new ProviderOrder();
        }
        public void UpdateProviderOrder(ProviderOrder ProviderOrder)
        {
            DataContext = ProviderOrder;
  
            TotalOrder.Text = $"Total commande: {ProviderOrder.Price}";
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
        private bool IsDecimalAllowed(string input)
        {
            var regex = new System.Text.RegularExpressions.Regex(@"^[0-9]*(?:[\.,][0-9]*)?$");
            return regex.IsMatch(input);
        }
        private void TextBox_DecimalInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsDecimalAllowed(e.Text))
            {
                MessageBox.Show("Veuillez entrer un nombre décimal valide dans le prix.");
                e.Handled = true; // Bloquer la saisie si ce n'est pas un nombre décimal avec , ou .
            }
        }
        private void TextBox_QuantityChanged(object sender, RoutedEventArgs e)
        {
            TotalCommand();
        }

        private void TextBox_PriceChanged(object sender, RoutedEventArgs e)
        {
            TotalCommand();
        }
        private void TotalCommand()
        {           
            if (DataContext is ProviderOrder ProviderOrder)
            {
                var Total= ProviderOrder.ProviderOrderLines!.Sum(orderLine => orderLine.Quantity * orderLine.Price);

                TotalOrder.Text = $"Total commande: {Total}";
            }
        }
        private async void UpdateCommandOrder(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProviderOrder ProviderOrder)
            {
                if (ProviderOrder.ProviderOrderLines!.Any(orderLine => orderLine.Quantity <= 0 || string.IsNullOrEmpty(orderLine.Quantity.ToString())))
                {
                    MessageBox.Show("Veuillez entrer une quantité valide (supérieure à 0).", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Vérifier que le prix est valide
                if (ProviderOrder.ProviderOrderLines!.Any(orderLine => orderLine.Price <= 0 || string.IsNullOrEmpty(orderLine.Price.ToString())))
                {
                    MessageBox.Show("Veuillez entrer un prix valide (supérieur à 0).", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                ProviderOrder.ProviderOrderStatus = ProviderOrderStatus.VALIDE;
                ProviderOrder.Price = ProviderOrder.ProviderOrderLines!.Sum(orderLine => orderLine.Quantity * orderLine.Price);
                var updateOrder = await CommandProviderService.UpdateProviderOrder(ProviderOrder);
                if (!string.IsNullOrEmpty(updateOrder))
                {
                    var Message = MessageBox.Show(updateOrder, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    if (Message == MessageBoxResult.OK)
                    {
                        _mainGrid.Children.Clear();
                        _mainGrid.Children.Add(new ProviderOrderToDelivery(_mainGrid));
                    }
                }
                else
                {
                    MessageBox.Show("Une erreur s'est produite", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private async void RefuseOrderCommand(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProviderOrder ProviderOrder)
            {
                if (ProviderOrder.ProviderOrderLines!.Any(orderLine => orderLine.Quantity <= 0 || string.IsNullOrEmpty(orderLine.Quantity.ToString())))
                {
                    MessageBox.Show("Veuillez entrer une quantité valide (supérieure à 0).", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Vérifier que le prix est valide
                if (ProviderOrder.ProviderOrderLines!.Any(orderLine => orderLine.Price <= 0 || string.IsNullOrEmpty(orderLine.Price.ToString())))
                {
                    MessageBox.Show("Veuillez entrer un prix valide (supérieur à 0).", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                ProviderOrder.ProviderOrderStatus = ProviderOrderStatus.REFUSE;
                ProviderOrder.Price = ProviderOrder.ProviderOrderLines!.Sum(orderLine => orderLine.Quantity * orderLine.Price);
                var updateOrder = await CommandProviderService.UpdateProviderOrder(ProviderOrder);
                if (!string.IsNullOrEmpty(updateOrder))
                {
                    var Message = MessageBox.Show(updateOrder, "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                    if (Message == MessageBoxResult.OK)
                    {
                        _mainGrid.Children.Clear();
                        _mainGrid.Children.Add(new RefusedOrderProvider(_mainGrid));
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
