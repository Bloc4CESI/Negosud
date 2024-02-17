using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public void UpdateProviderOrder(ProviderOrder providerOrder)
        {
            DataContext = providerOrder;

            TotalOrder.Text = $"Total commande: {providerOrder.Price}";
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
            var regex = new System.Text.RegularExpressions.Regex(@"^[0-9]*(?:[\.][0-9]*)?$");
            return regex.IsMatch(input);
        }
        private void TextBox_DecimalInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsDecimalAllowed(e.Text))
            {
                MessageBox.Show("Veuillez entrer un nombre décimal valide séparé par un '.' dans le prix.");
                e.Handled = true; // Bloquer la saisie si ce n'est pas un nombre décimal avec  .
            }
        }
        private void TextBox_QuantityChanged(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox quantityTextBox)
            {
                // Convertir la valeur en int
                if (int.TryParse(quantityTextBox.Text, out int quantity))
                {
                    // Mettre à jour la propriété Quantity de la ligne
                    if (quantityTextBox.DataContext is ProviderOrderLine orderLine)
                    {
                        orderLine.Quantity = quantity;
                        orderLine.TotalPrice = quantity * orderLine.Price;
                    }
                    // Appeler la méthode TotalCommand pour mettre à jour le total
                    TotalCommand();
                }
            }
        }
        private void TextBox_PriceChanged(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox priceTextBox)
            {
                // Convertir la valeur en decimal (si nécessaire)
                if (decimal.TryParse(priceTextBox.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal price))
                {
                    // Mettre à jour la propriété Price de la ligne
                    if (priceTextBox.DataContext is ProviderOrderLine orderLine)
                    {
                        orderLine.Price = price;
                        orderLine.TotalPrice = price * orderLine.Quantity;
                    }

                    // Appeler la méthode TotalCommand pour mettre à jour le total
                    TotalCommand();
                }
                // Ensuite, essayez avec le point comme séparateur décimal
                else if (decimal.TryParse(priceTextBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out price))
                {
                    // Mettre à jour la propriété Price de la ligne
                    if (priceTextBox.DataContext is ProviderOrderLine orderLine)
                    {
                        orderLine.Price = price;
                        orderLine.TotalPrice = price * orderLine.Quantity;
                    }

                    // Appeler la méthode TotalCommand pour mettre à jour le total
                    TotalCommand();
                }
            }
        }
        private void TotalCommand()
        {
            decimal totalPrice = 0;

            foreach (ProviderOrderLine orderLine in ProviderOrderLinesDataGrid.ItemsSource)
            {
                decimal TotalPriceLigne = orderLine.Quantity * orderLine.Price;
                totalPrice += TotalPriceLigne;
            }
            TotalOrder.Text = $"Total commande: {totalPrice}";
        }

        private async void UpdateCommandOrder(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProviderOrder providerOrder)
            {
                if (providerOrder.ProviderOrderLines!.Any(orderLine => orderLine.Quantity <= 0 || string.IsNullOrEmpty(orderLine.Quantity.ToString())))
                {
                    MessageBox.Show("Veuillez entrer une quantité valide (supérieure à 0).", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Vérifier que le prix est valide
                if (providerOrder.ProviderOrderLines!.Any(orderLine => orderLine.Price <= 0 || string.IsNullOrEmpty(orderLine.Price.ToString())))
                {
                    MessageBox.Show("Veuillez entrer un prix valide (supérieur à 0).", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                providerOrder.ProviderOrderStatus = ProviderOrderStatus.VALIDE;
                providerOrder.Price = providerOrder.ProviderOrderLines!.Sum(orderLine => orderLine.Quantity * orderLine.Price);
                var updateOrder = await CommandProviderService.UpdateProviderOrder(providerOrder);
                if (!string.IsNullOrEmpty(updateOrder))
                {
                    var message = MessageBox.Show(updateOrder, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    if (message == MessageBoxResult.OK)
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
            if (DataContext is ProviderOrder providerOrder)
            {
                if (providerOrder.ProviderOrderLines!.Any(orderLine => orderLine.Quantity <= 0 || string.IsNullOrEmpty(orderLine.Quantity.ToString())))
                {
                    MessageBox.Show("Veuillez entrer une quantité valide (supérieure à 0).", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Vérifier que le prix est valide
                if (providerOrder.ProviderOrderLines!.Any(orderLine => orderLine.Price <= 0 || string.IsNullOrEmpty(orderLine.Price.ToString())))
                {
                    MessageBox.Show("Veuillez entrer un prix valide (supérieur à 0).", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                providerOrder.ProviderOrderStatus = ProviderOrderStatus.REFUSE;
                providerOrder.Price = providerOrder.ProviderOrderLines!.Sum(orderLine => orderLine.Quantity * orderLine.Price);
                var updateOrder = await CommandProviderService.UpdateProviderOrder(providerOrder);
                if (!string.IsNullOrEmpty(updateOrder))
                {
                    var message = MessageBox.Show(updateOrder, "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                    if (message == MessageBoxResult.OK)
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
