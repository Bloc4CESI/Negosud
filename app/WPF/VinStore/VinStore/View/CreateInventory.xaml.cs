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
using MaterialDesignThemes.Wpf;
using VinStore.Models;
using VinStore.Services;

namespace VinStore.View
{
    /// <summary>
    /// Logique d'interaction pour CreateInventory.xaml
    /// </summary>
    public partial class CreateInventory : UserControl
    {
        private Grid _mainGrid;
        int RowCounter = 0;
        public CreateInventory(Grid mainGrid)
        {
            InitializeComponent();
            OrderDate.SelectedDate = DateTime.Now;
            LoadProducts();
            _mainGrid = mainGrid;
        }
        private async void LoadProducts()
        {
            try
            {
                var products = await ProductService.GetAllProducts();
                if (products != null)
                {
                    foreach (var Product in products)
                    {
                        var newRow = new RowDefinition();
                        newRow.Height = GridLength.Auto;
                        ProductLigneGrid.RowDefinitions.Add(newRow);
/*                        int CurrentRow = ProductLigneGrid.RowDefinitions.Count - 1;*/
                        var articleTextBox = new TextBox
                        {
                            Margin = new Thickness(20),
                            Background = Brushes.White,
                            Padding = new Thickness(10),
                            Name = "Product" + RowCounter,
                            Text = Product.Name,
                            Tag = Product.Id
                        };
                        RegisterName(articleTextBox.Name, articleTextBox);
                        Grid.SetColumn(articleTextBox, 0);
                        Grid.SetColumnSpan(articleTextBox, 2);
                        Grid.SetRow(articleTextBox, RowCounter);                     
                        HintAssist.SetHint(articleTextBox, "Nom de l'article");
                        TextFieldAssist.SetUnderlineBrush(articleTextBox, Brushes.Black);
                        HintAssist.SetHintOpacity(articleTextBox, 0.26);
                        articleTextBox.MaxHeight = 50;
                        articleTextBox.Foreground = Brushes.Black;
                        articleTextBox.IsReadOnly = true;
                        TextBox textBoxQuantityStock = new TextBox
                        {
                            Margin = new Thickness(20),
                            Background = Brushes.White,
                            Padding = new Thickness(10),
                            IsReadOnly = true,
                            Name = "StockQuantity" + RowCounter,
                            Text = Product.Stock?.Quantity.ToString() ?? "N/A"
                        };
                        Grid.SetColumn(textBoxQuantityStock, 2);
                        Grid.SetRow(textBoxQuantityStock, RowCounter);
                        RegisterName(textBoxQuantityStock.Name, textBoxQuantityStock);
                        HintAssist.SetHint(textBoxQuantityStock, "Quantité stock");
                        TextFieldAssist.SetUnderlineBrush(textBoxQuantityStock, Brushes.Black);
                        HintAssist.SetHintOpacity(textBoxQuantityStock, 0.26);
                        textBoxQuantityStock.MaxHeight = 50;
                        textBoxQuantityStock.Foreground = Brushes.Black;
                        TextBox textBoxQuantityInventory = new TextBox
                        {
                            Margin = new Thickness(20),
                            Background = Brushes.White,
                            Padding = new Thickness(10),
                            Name = "QuantityInventory" + RowCounter
                        };
                        textBoxQuantityInventory.PreviewTextInput += TextBox_IntegerInput;                      
                        Grid.SetColumn(textBoxQuantityInventory, 3);
                        Grid.SetRow(textBoxQuantityInventory, RowCounter);
                        RegisterName(textBoxQuantityInventory.Name, textBoxQuantityInventory);
                        HintAssist.SetHint(textBoxQuantityInventory, "Quantité Inventaire");
                        TextFieldAssist.SetUnderlineBrush(textBoxQuantityInventory, Brushes.Black);
                        HintAssist.SetHintOpacity(textBoxQuantityInventory, 0.26);
                        textBoxQuantityInventory.MaxHeight = 50;
                        textBoxQuantityInventory.Foreground = Brushes.Black;
                        ProductLigneGrid.Children.Add(articleTextBox);
                        ProductLigneGrid.Children.Add(textBoxQuantityStock);
                        ProductLigneGrid.Children.Add(textBoxQuantityInventory);
                        RowCounter++;
                    }
                    ProductLigneGrid.UpdateLayout();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la requête : {ex.Message}");
            }
        }


        private void TextBox_IntegerInput(object sender, TextCompositionEventArgs e)
        {
            // Vérifier si le texte est un entier
            if (!int.TryParse(e.Text, out _))
            {
                MessageBox.Show("Veuillez entrer un nombre entier valide.");
                e.Handled = true; // Ignorer si pas un entier
            }
        }
        private async Task<(string ResponseMessage, Inventory? Inventory)> SaveInventoryStatusInProgress()
        {
            // Enregistrer clé valeur 
            List<Dictionary<string, object>> InventoryDataList = new List<Dictionary<string, object>>();
            bool isDataValid = true;

            for (int i = 0; i <= RowCounter ; i++)
            {
                var articleTextBox = ProductLigneGrid.FindName("Product" + i ) as TextBox;
                var stockQuantityTextBox = ProductLigneGrid.FindName("StockQuantity" + i) as TextBox;
                var quantityInventoryTextBox = ProductLigneGrid.FindName("QuantityInventory" + i) as TextBox;
                if (articleTextBox != null && stockQuantityTextBox != null && quantityInventoryTextBox != null)
                {
                    // Ajouter les valeurs
                    // Vérifier si l'utilisateur entre toutes les quantités
                    if (string.IsNullOrWhiteSpace(quantityInventoryTextBox.Text))
                    {
                        MessageBox.Show("Veuillez entrer toutes les quantités des articles");
                        isDataValid = false;
                    }

                    if (OrderDate.SelectedDate == null)
                    {
                        MessageBox.Show("Veuillez entrer la date de l'inventaire.");
                        isDataValid = false;
                        break;
                    }

                    if (!int.TryParse(quantityInventoryTextBox.Text, out int quantityValueInventory) )
                    {
                        MessageBox.Show("Veuillez entrer un nombre entier valide dans la quantité.");
                        isDataValid = false;
                        break;
                    }

                    if (!int.TryParse(stockQuantityTextBox.Text, out int quantityValueStock))
                    {
                        MessageBox.Show("Veuillez entrer un nombre entier valide dans la quantité.");
                        isDataValid = false;
                        break;
                    }

                    if (articleTextBox.Tag is int productId)
                    {
                        // Retrieve StockId using navigation property
                        int stockId = 0;
                        try
                        {
                            var product = await ProductService.GetProductById(productId);
                            if (product != null && product.Stock != null)
                            {
                                stockId = product.Stock.Id;
                            }
                        }
                        catch (Exception ex)
                        {
                            // Handle the exception or propagate it if needed
                            MessageBox.Show($"Error retrieving stock information: {ex.Message}");
                            isDataValid = false;
                            break;
                        }

                        // Créer un dictionnaire = map pour stocker les données de cette ligne
                        Dictionary<string, object> InventoryData = new Dictionary<string, object>
                {
                    { "ProductId", productId },
                    { "StockQuantity", quantityValueInventory },
                    { "StockId", stockId },
                    {"quantityValueStock",quantityValueStock }
                };
                        InventoryDataList.Add(InventoryData);
                    }
                }
            }

            if (isDataValid)
            {
                var inventory = new Inventory
                {
                    Date = OrderDate.SelectedDate!.Value,
                    StatusInventory = ApiNegosud.Models.Inventory.InventoryEnum.ENCOURSDEVALIDATION,
                    InventoryLignes = InventoryDataList.Select(data => new InventoryLigne
                    {
                        StockId = (int)data["StockId"],                       
                        QuantityInventory = (int)data["StockQuantity"],
                        QuantityStock = (int)data["quantityValueStock"]
                    }).ToList()
                };

                var (responseMessage, createdOrder) = await InventoryService.PostInventory(inventory);
                return (responseMessage, createdOrder);
            }

            return (string.Empty, null);
        }

        private async void SaveInventory(object sender, RoutedEventArgs e)
        {
            var (responseMessage, createdInventory) = await SaveInventoryStatusInProgress();
            if (!string.IsNullOrEmpty(responseMessage) && createdInventory != null)
            {
                var message = MessageBox.Show(responseMessage, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                if (message == MessageBoxResult.OK)
                {
                    _mainGrid.Children.Clear();
                    _mainGrid.Children.Add(new InventoryToValidate(_mainGrid));
                }
            }
            else
            {
                MessageBox.Show("Une erreur s'est produite", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private async void SaveAndValidateInventory(object sender, RoutedEventArgs e)
        {
            var (responseMessage, CreatedInventory) = await SaveInventoryStatusInProgress();
            if (!string.IsNullOrEmpty(responseMessage) && CreatedInventory != null)
            {
                CreatedInventory.StatusInventory = Inventory.InventoryEnum.VALIDE;
                var updateOrder = await InventoryService.UpdateInventory(CreatedInventory);
                if (!string.IsNullOrEmpty(updateOrder))
                {
                    var Message = MessageBox.Show(updateOrder, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    if (Message == MessageBoxResult.OK)
                    {
                        _mainGrid.Children.Clear();
                        _mainGrid.Children.Add(new ValidateInventory(_mainGrid));
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
