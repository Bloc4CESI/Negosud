using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ApiNegosud.Models;
using MaterialDesignThemes.Wpf;
using VinStore.Services;

namespace VinStore.View
{
    /// <summary>
    /// Logique d'interaction pour AddCommandProvider.xaml
    /// </summary>
    public partial class AddCommandProvider : UserControl
    {
        private Grid _mainGrid;
        public AddCommandProvider(Grid mainGrid)
        {
            InitializeComponent();
            _mainGrid = mainGrid;
            OrderDate.SelectedDate= DateTime.Now;
            LoadProviders();
            DeleteProduct0.Visibility= Visibility.Collapsed;
            

        }
        private async void LoadProviders()
        {
            try
            {
                var providers = await ProviderService.GetProviders();
                ProvidersName.ItemsSource= providers;
                ProvidersName.DisplayMemberPath = "Name";
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la requête : {ex.Message}");
                MessageBox.Show($"{ex.Message}");
            }
        }
        private int productComboBoxIndex = 1;
        private async void AddProduct(object sender, RoutedEventArgs e)
        {
            // Créer une nouvelle ligne à ajouter à la grille
            RowDefinition newRow = new RowDefinition();
            newRow.Height = GridLength.Auto;
            ProductLigneGrid.RowDefinitions.Add(newRow);
            int currentRow = ProductLigneGrid.RowDefinitions.Count - 1;

            // Ajouter les contrôles à la nouvelle ligne
            ComboBox ComboxProduct = new ComboBox
            {
                Margin = new Thickness(20),
                Background = Brushes.White,
                Style = (Style)FindResource("MaterialDesignFilledComboBox"),
                Padding = new Thickness(10),
                Name = "ProductsComboBox_" + productComboBoxIndex
            };
            // Enregistrez le nom du ComboBox
            RegisterName(ComboxProduct.Name, ComboxProduct);
            Grid.SetColumn(ComboxProduct, 0);
            Grid.SetColumnSpan(ComboxProduct, 2);
            Grid.SetRow(ComboxProduct, currentRow);

            if (ProvidersName.SelectedItem is Provider selectedProvider)
            {
                ComboxProduct.ItemsSource = await ProductService.GetProductByProvider(selectedProvider.Id);
                ComboxProduct.DisplayMemberPath = "Name";
            }
            // Configurer les propriétés Material Design
            HintAssist.SetHint(ComboxProduct, "Nom de l'article");
            TextFieldAssist.SetUnderlineBrush(ComboxProduct, Brushes.Black);
            HintAssist.SetHintOpacity(ComboxProduct, 0.26);
            HintAssist.SetForeground(ComboxProduct, Brushes.Black);
            ComboxProduct.MaxHeight = 50;
            ComboxProduct.Foreground = Brushes.Black;
            ComboxProduct.IsEditable = true;
            // ajouter un evennt listener à chaque combox 
            ComboxProduct.SelectionChanged += ProductNewComboBox_SelectionChanged;
            TextBox TextBoxQuantityStock = new TextBox
            {
                Margin = new Thickness(20),
                Background = Brushes.White,
                Padding = new Thickness(10),
                IsReadOnly = true,
                Style = (Style)FindResource("MaterialDesignFilledTextBox"),
                Name = "StockQuantity" + productComboBoxIndex
            };
            Grid.SetColumn(TextBoxQuantityStock, 2);
            Grid.SetRow(TextBoxQuantityStock, currentRow);
            RegisterName(TextBoxQuantityStock.Name, TextBoxQuantityStock);
            HintAssist.SetHint(TextBoxQuantityStock, "Quantité stock");
            TextFieldAssist.SetUnderlineBrush(TextBoxQuantityStock, Brushes.Black);
            HintAssist.SetForeground(TextBoxQuantityStock, Brushes.Black);
            HintAssist.SetHintOpacity(TextBoxQuantityStock, 0.26);
            TextBoxQuantityStock.MaxHeight = 50;
            TextBoxQuantityStock.Foreground = Brushes.Black;

            TextBox TextBoxQuantityLigne = new TextBox
            {
                Margin = new Thickness(20),
                Background = Brushes.White,
                Padding = new Thickness(10),
                Style = (Style)FindResource("MaterialDesignFilledTextBox"),
                Name = "QuantityLigneOrder" + productComboBoxIndex
            };
            TextBoxQuantityLigne.PreviewTextInput += TextBox_IntegerInput;
            TextBoxQuantityLigne.TextChanged += TextBox_QuantityChanged;
            Grid.SetColumn(TextBoxQuantityLigne, 3);
            Grid.SetRow(TextBoxQuantityLigne, currentRow);
            RegisterName(TextBoxQuantityLigne.Name, TextBoxQuantityLigne);
            HintAssist.SetHint(TextBoxQuantityLigne, "Quantité à commander");
            TextFieldAssist.SetUnderlineBrush(TextBoxQuantityLigne, Brushes.Black);
            HintAssist.SetHintOpacity(TextBoxQuantityLigne, 0.26);
            HintAssist.SetForeground(TextBoxQuantityLigne, Brushes.Black);
            TextBoxQuantityLigne.MaxHeight = 50;
            TextBoxQuantityLigne.Foreground = Brushes.Black;

            TextBox TextBoxPriceLigne = new TextBox
            {
                Margin = new Thickness(20),
                Background = Brushes.White,
                Padding = new Thickness(10),
                Style = (Style)FindResource("MaterialDesignFilledTextBox"),
                Name = "PriceLigneOrder" + productComboBoxIndex

            };
            TextBoxPriceLigne.PreviewTextInput += TextBox_DecimalInput;
            TextBoxPriceLigne.TextChanged += TextBox_PriceChanged;
            Grid.SetColumn(TextBoxPriceLigne, 4);
            Grid.SetRow(TextBoxPriceLigne, currentRow);
            RegisterName(TextBoxPriceLigne.Name, TextBoxPriceLigne);
            HintAssist.SetHint(TextBoxPriceLigne, "Prix article unitaire");
            TextFieldAssist.SetUnderlineBrush(TextBoxPriceLigne, Brushes.Black);
            HintAssist.SetHintOpacity(TextBoxPriceLigne, 0.26);
            HintAssist.SetForeground(TextBoxPriceLigne, Brushes.Black);
            TextBoxPriceLigne.MaxHeight = 50;
            TextBoxPriceLigne.Foreground = Brushes.Black;

            TextBox TextBoxPriceLigneTotal = new TextBox
            {
                Margin = new Thickness(20),
                IsEnabled = false,
                Background = Brushes.White,
                Padding = new Thickness(10),
                Style = (Style)FindResource("MaterialDesignFilledTextBox"),
                Name = "PriceTotalLigne" + productComboBoxIndex

            };
            Grid.SetColumn(TextBoxPriceLigneTotal, 5);
            Grid.SetRow(TextBoxPriceLigneTotal, currentRow);
            RegisterName(TextBoxPriceLigneTotal.Name, TextBoxPriceLigneTotal);
            HintAssist.SetHint(TextBoxPriceLigneTotal, "Prix article total");
            TextFieldAssist.SetUnderlineBrush(TextBoxPriceLigneTotal, Brushes.Black);
            HintAssist.SetHintOpacity(TextBoxPriceLigneTotal, 0.26);
            HintAssist.SetForeground(TextBoxPriceLigneTotal, Brushes.Black);
            TextBoxPriceLigneTotal.MaxHeight = 50;
            TextBoxPriceLigneTotal.Foreground = Brushes.Black;

            Button addButton = new Button
            {
                Margin = new Thickness(10),
                Content = "+",
                Background = Brushes.DarkRed,
                BorderBrush = Brushes.DarkRed,
                IsEnabled = true,
                ToolTip = "Ajouter un article"
            };
            addButton.Click += AddProduct;
            Grid.SetColumn(addButton, 6);
            Grid.SetRow(addButton, currentRow);

            Button deleteButton = new Button
            {
                Margin = new Thickness(10),
                Content = "X",
                Background = Brushes.DarkRed,
                BorderBrush = Brushes.DarkRed,
                IsEnabled = true,
                ToolTip = "Supprimer la ligne article"
            };
            Grid.SetColumn(deleteButton, 7);
            Grid.SetRow(deleteButton, currentRow);
            deleteButton.Click += DeleteProductRow;
            ProductLigneGrid.Children.Add(ComboxProduct);
            ProductLigneGrid.Children.Add(TextBoxQuantityStock);
            ProductLigneGrid.Children.Add(TextBoxQuantityLigne);
            ProductLigneGrid.Children.Add(TextBoxPriceLigne);
            ProductLigneGrid.Children.Add(TextBoxPriceLigneTotal);
            ProductLigneGrid.Children.Add(addButton);
            ProductLigneGrid.Children.Add(deleteButton);
            productComboBoxIndex++;
            ProductLigneGrid.UpdateLayout();
        }
        private bool CheckIfProductSelected()
        {
            bool isProductSelected = false;
            // Vérifier les ComboBox dans la grille
            for (int i = 0; i < productComboBoxIndex; i++)
            {
                var comboBoxName = "ProductsComboBox_" + i;
                ComboBox? existingComboBox = ProductLigneGrid.FindName(comboBoxName) as ComboBox;

                if (existingComboBox != null && !string.IsNullOrWhiteSpace(existingComboBox.Text))
                {
                    isProductSelected = true;
                    break; // Sortir de la boucle dès qu'un article est sélectionné pour ce fournisseur
                }
            }
            ProvidersName.IsReadOnly = !isProductSelected;
            return isProductSelected;
        }
        private void TextBox_QuantityChanged(object sender, TextChangedEventArgs e)
        {
            TotalCommand();
        }

        private void TextBox_PriceChanged(object sender, TextChangedEventArgs e)
        {
            TotalCommand();
        }
        private void TotalCommand()
        {
            decimal TotalPrice = 0;

            for (int i = 0; i < ProductLigneGrid.RowDefinitions.Count; i++)
            {
                var QuantityTextBox = ProductLigneGrid.FindName("QuantityLigneOrder" + i) as TextBox;
                var PriceTextBox = ProductLigneGrid.FindName("PriceLigneOrder" + i) as TextBox;
                var PriceTextBoxLigne = ProductLigneGrid.FindName("PriceTotalLigne" + i) as TextBox;

                if (QuantityTextBox != null && PriceTextBox != null)
                {
                    // Essayez d'abord avec la virgule comme séparateur décimal
                    if (int.TryParse(QuantityTextBox.Text, out var QuantityLigne) &&
                        Decimal.TryParse(PriceTextBox.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out var PriceLigne))
                    {
                        decimal TotalPriceLigne = QuantityLigne * PriceLigne;
                        PriceTextBoxLigne.Text = TotalPriceLigne.ToString();
                        TotalPrice += TotalPriceLigne;
                    }
                    // Ensuite, essayez avec le point comme séparateur décimal
                    else if (int.TryParse(QuantityTextBox.Text, out QuantityLigne) &&
                             Decimal.TryParse(PriceTextBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out PriceLigne))
                    {
                        decimal TotalPriceLigne = QuantityLigne * PriceLigne;
                        PriceTextBoxLigne.Text = TotalPriceLigne.ToString();
                        TotalPrice += TotalPriceLigne;
                    }
                }
            }

            TotalOrder.Text = $"Total commande: {TotalPrice}";
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
        private async void ProviderSelected(object sender, SelectionChangedEventArgs e)
        {
            if (CheckIfProductSelected())
            {
               
                MessageBox.Show("Veuillez supprimer les articles avant de changer de fournisseur");
                // Annuler le changement de sélection
                ProvidersName.SelectionChanged -= ProviderSelected;
                ProvidersName.SelectedItem = e.RemovedItems.Count > 0 ? e.RemovedItems[0] : null;
                ProvidersName.SelectionChanged += ProviderSelected;
                return;
            }
            if (ProvidersName.SelectedItem is Provider selectedProvider)
            {
                
                var Products = await ProductService.GetProductByProvider(selectedProvider.Id);
                for (int i = 0; i < productComboBoxIndex; i++)
                {
                    var comboBoxName = "ProductsComboBox_" + i;
                    ComboBox? existingComboBox = ProductLigneGrid.FindName(comboBoxName) as ComboBox;
                    if (existingComboBox != null)
                    {
                        existingComboBox.ItemsSource = Products;
                        existingComboBox.DisplayMemberPath = "Name";
                    }
                }
                AddressProvider.Text = selectedProvider.Address?.ToString() ?? string.Empty;
            }
        }
        private async void ProductNewComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox ComboBox)
            {
                if (ComboBox.SelectedItem is Product selectedProduct)
                {
                    var ProductDetails = await ProductService.GetProductById(selectedProduct.Id);
                    // Trouver le nom du TextBox associé à cette ComboBox
                    var TextBoxName = "StockQuantity" + ComboBox.Name.Substring("ProductsComboBox_".Length);
                    var PriceProduct = "PriceLigneOrder" + ComboBox.Name.Substring("ProductsComboBox_".Length);
                    // Utiliser FindName pour récupérer le TextBox quantité + prix
                    TextBox? QuantityTextBox = ProductLigneGrid.FindName(TextBoxName) as TextBox;
                    TextBox? PriceTextBox = ProductLigneGrid.FindName(PriceProduct) as TextBox;
                    if (QuantityTextBox != null && PriceTextBox != null)
                    {
                        // Mettre à jour le texte du TextBox
                        QuantityTextBox.Text = ProductDetails.Stock?.Quantity.ToString() ?? "N/A";
                        PriceTextBox.Text= Math.Round((ProductDetails.Price/ 3),2).ToString();
                    }
                }
                else
                {
                    // Si aucun article n'est sélectionné, effacer la valeur du TextBox
                    var TextBoxName = "StockQuantity" + ComboBox.Name.Substring("ProductsComboBox_".Length);
                    var PriceProduct = "PriceLigneOrder" + ComboBox.Name.Substring("ProductsComboBox_".Length);
                    var PriceProductLigne = "PriceTotalLigne" + ComboBox.Name.Substring("ProductsComboBox_".Length);
                    TextBox? QuantityTextBox = ProductLigneGrid.FindName(TextBoxName) as TextBox;
                    TextBox? PriceTextBox = ProductLigneGrid.FindName(PriceProduct) as TextBox;
                    TextBox? PriceLigneTextBox = ProductLigneGrid.FindName(PriceProductLigne) as TextBox;
                    if (QuantityTextBox != null && PriceTextBox != null)
                    {
                        QuantityTextBox.Text = string.Empty;
                        PriceTextBox.Text =string.Empty;
                        PriceLigneTextBox.Text = string.Empty;
                    }
                }
            }
        }
        private async void DeleteProductRow(object sender, RoutedEventArgs e)
        {
            Button deleteButton = (Button)sender;
            // 'indice de la ligne à supprimer
            int rowIndex = Grid.GetRow(deleteButton);
            // Supprimee tous les columns
            for (int i = 0; i < ProductLigneGrid.ColumnDefinitions.Count; i++)
            {
                //recherche les elments qui se trouve à la ligne rowIndex et à la colonne i dans la grille
                //conversion de l'élément dans la collection à type UIElement
                //le row correspond au row et le column worrspond column de children i spécifié
                var element = ProductLigneGrid.Children.Cast<UIElement>()
                    .FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == i);

                if (element != null)
                {
                    if (element is FrameworkElement frameworkElement)
                    {
                        // s'il y a un nom enregistré on le supprimer
                        if (!string.IsNullOrEmpty(frameworkElement.Name))
                        {
                            UnregisterName(frameworkElement.Name);
                        }
                    }
                    ProductLigneGrid.Children.Remove(element);
                }
            }
            ProductLigneGrid.RowDefinitions.RemoveAt(rowIndex);
            productComboBoxIndex--;


            // Réorganisez les indices de ligne pour les boutons deleteButton restants
            for (int i = rowIndex; i < ProductLigneGrid.RowDefinitions.Count; i++)
            {
                foreach (UIElement child in ProductLigneGrid.Children)
                {
                    if (Grid.GetRow(child) == i + 1)
                    {
                        Grid.SetRow(child, i);
                    }
                }
            }
            TotalCommand();
        }
        private async Task<(string ResponseMessage, ProviderOrder? Order)> SaveOrderProviderStatusInProgress()
        {
            // Enregistrer clé valeur 
            List<Dictionary<string, object>> orderDataList = new List<Dictionary<string, object>>();
            bool isDataValid = true;
            for (int i = 0; i < ProductLigneGrid.RowDefinitions.Count; i++)
            {
                var comboBox = ProductLigneGrid.FindName("ProductsComboBox_" + i) as ComboBox;
                var quantityTextBox = ProductLigneGrid.FindName("QuantityLigneOrder" + i) as TextBox;
                var priceTextBox = ProductLigneGrid.FindName("PriceLigneOrder" + i) as TextBox;

                if (comboBox != null && quantityTextBox != null && priceTextBox != null)
                {
                    // Ajouter les valeurs
                    var selectedProductId = 0;
                    if (comboBox.SelectedItem == null)
                    {
                        MessageBox.Show("le nom de l'article est obliagtoire");
                        isDataValid = false;
                        break;
                    }
                    else
                    {
                        if (comboBox.SelectedItem is Product selectedProduct)
                        {
                            selectedProductId = selectedProduct.Id;
                        }

                    }
                    // Vérifier si le produit existe déjà dans la liste
                    if (orderDataList.Any(data => (int)data["ProductId"] == selectedProductId))
                    {
                        MessageBox.Show("Vous ne pouvez ajouter le même produit qu'une seule fois.");
                        isDataValid = false;
                        break;
                    }
                    if (string.IsNullOrWhiteSpace(quantityTextBox.Text) || string.IsNullOrWhiteSpace(priceTextBox.Text))
                    {
                        MessageBox.Show("Veuillez entrer tous les champs obligatoires.");
                        isDataValid = false;
                    }
                    if (!int.TryParse(quantityTextBox.Text, out int quantityValue) || quantityValue <= 0)
                    {
                        MessageBox.Show("Veuillez entrer un nombre entier valide dans la quantité.");
                        isDataValid = false;
                        break;
                    }
                    decimal priceValue = 0;
                    // Essayez d'abord avec la virgule ou . comme séparateur décimal
                    if (!Decimal.TryParse(priceTextBox.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out priceValue) &&
                        !Decimal.TryParse(priceTextBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out  priceValue) || priceValue<=0)
                    {
                        MessageBox.Show("Veuillez entrer un prix valide.");
                        isDataValid = false;
                        break;
                    }
              
                    if (OrderDate.SelectedDate == null || ProvidersName.SelectedItem == null)
                    {
                        // La conversion a échoué, faire quelque chose en conséquence (peut-être afficher un message d'erreur)
                        MessageBox.Show("Veuillez entrer tous les champs obligatoires.");
                        isDataValid = false;
                        break;
                    }
                    // Créer un dictionnaire pour stocker les données de cette ligne
                    Dictionary<string, object> orderData = new Dictionary<string, object>
                    {
                        { "ProductId", selectedProductId },
                        { "Quantity", quantityValue },
                        { "Price", priceValue }
                    };
                    orderDataList.Add(orderData);
                }
            }
            if (isDataValid)
            {
                int selectedProviderId = 0;

                if (ProvidersName.SelectedItem is Provider selectedProvider)
                {
                    selectedProviderId = selectedProvider.Id;
                }
                if (selectedProviderId != 0)
                {
                    var orderProvider = new ProviderOrder
                    {
                        ProviderId = selectedProviderId,
                        Price = orderDataList.Sum(data => (int)data["Quantity"] * (decimal)data["Price"]),
                        Date = OrderDate.SelectedDate!.Value,
                        ProviderOrderStatus = ProviderOrderStatus.ENCOURSDEVALIDATION,
                        ProviderOrderLines = orderDataList.Select(data => new ProviderOrderLine
                        {
                            ProductId = (int)data["ProductId"],
                            Quantity = (int)data["Quantity"],
                            Price = (decimal)data["Price"]
                        }).ToList()

                    };
                    var (responseMessage, createdOrder) = await CommandProviderService.PostProviderOrder(orderProvider);
                    return (responseMessage, createdOrder);
                }
        }
         return (string.Empty, null);
        }
        private async void SaveOrderProvider(object sender, RoutedEventArgs e)
        {
            var (responseMessage, createdOrder)  = await SaveOrderProviderStatusInProgress();
            if (!string.IsNullOrEmpty(responseMessage) && createdOrder!=null)
            {
                var Message =MessageBox.Show(responseMessage, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                if (Message == MessageBoxResult.OK)
                {
                    _mainGrid.Children.Clear();
                    _mainGrid.Children.Add(new OrderProviderToValidate(_mainGrid));
                }
            }
            else
            {
                MessageBox.Show("Une erreur s'est produite", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private async void SaveAndValidateOrder(object sender, RoutedEventArgs e)
        {
            var (responseMessage, createdOrder) = await SaveOrderProviderStatusInProgress();
            if (!string.IsNullOrEmpty(responseMessage) && createdOrder!=null)
            {
                createdOrder.ProviderOrderStatus = ProviderOrderStatus.VALIDE;
                var updateOrder = await CommandProviderService.UpdateProviderOrder(createdOrder);
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

    }
}
