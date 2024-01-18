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
                var Products = await ProductService.GetAllProducts();

                if (Products != null)
                {
                    foreach (var Product in Products)
                    {
                        var NewRow = new RowDefinition();
                        NewRow.Height = GridLength.Auto;
                        ProductLigneGrid.RowDefinitions.Add(NewRow);
/*                        int CurrentRow = ProductLigneGrid.RowDefinitions.Count - 1;*/
                        var ArticleTextBox = new TextBox
                        {
                            Margin = new Thickness(20),
                            Background = Brushes.White,
                            Padding = new Thickness(10),
                            Name = "Product" + RowCounter,
                            Text = Product.Name,
                            Tag = Product.Id
                        };
                        RegisterName(ArticleTextBox.Name, ArticleTextBox);
                        Grid.SetColumn(ArticleTextBox, 0);
                        Grid.SetColumnSpan(ArticleTextBox, 2);
                        Grid.SetRow(ArticleTextBox, RowCounter);                     
                        HintAssist.SetHint(ArticleTextBox, "Nom de l'article");
                        TextFieldAssist.SetUnderlineBrush(ArticleTextBox, Brushes.Black);
                        HintAssist.SetHintOpacity(ArticleTextBox, 0.26);
                        ArticleTextBox.MaxHeight = 50;
                        ArticleTextBox.Foreground = Brushes.Black;
                        ArticleTextBox.IsReadOnly = true;
                        TextBox TextBoxQuantityStock = new TextBox
                        {
                            Margin = new Thickness(20),
                            Background = Brushes.White,
                            Padding = new Thickness(10),
                            IsReadOnly = true,
                            Name = "StockQuantity" + RowCounter,
                            Text = Product.Stock?.Quantity.ToString() ?? "N/A"
                        };
                        Grid.SetColumn(TextBoxQuantityStock, 2);
                        Grid.SetRow(TextBoxQuantityStock, RowCounter);
                        RegisterName(TextBoxQuantityStock.Name, TextBoxQuantityStock);
                        HintAssist.SetHint(TextBoxQuantityStock, "Quantité stock");
                        TextFieldAssist.SetUnderlineBrush(TextBoxQuantityStock, Brushes.Black);
                        HintAssist.SetHintOpacity(TextBoxQuantityStock, 0.26);
                        TextBoxQuantityStock.MaxHeight = 50;
                        TextBoxQuantityStock.Foreground = Brushes.Black;
                        TextBox TextBoxQuantityInventory = new TextBox
                        {
                            Margin = new Thickness(20),
                            Background = Brushes.White,
                            Padding = new Thickness(10),
                            Name = "QuantityInventory" + RowCounter
                        };
                        TextBoxQuantityInventory.PreviewTextInput += TextBox_IntegerInput;                      
                        Grid.SetColumn(TextBoxQuantityInventory, 3);
                        Grid.SetRow(TextBoxQuantityInventory, RowCounter);
                        RegisterName(TextBoxQuantityInventory.Name, TextBoxQuantityInventory);
                        HintAssist.SetHint(TextBoxQuantityInventory, "Quantité Inventaire");
                        TextFieldAssist.SetUnderlineBrush(TextBoxQuantityInventory, Brushes.Black);
                        HintAssist.SetHintOpacity(TextBoxQuantityInventory, 0.26);
                        TextBoxQuantityInventory.MaxHeight = 50;
                        TextBoxQuantityInventory.Foreground = Brushes.Black;
                        ProductLigneGrid.Children.Add(ArticleTextBox);
                        ProductLigneGrid.Children.Add(TextBoxQuantityStock);
                        ProductLigneGrid.Children.Add(TextBoxQuantityInventory);
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
                MessageBox.Show("Veuillez entrer un nombre entier valide dans la quantité.");
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
                var ArticleTextBox = ProductLigneGrid.FindName("Product" + i ) as TextBox;
                var StockQuantityTextBox = ProductLigneGrid.FindName("StockQuantity" + i) as TextBox;
                var QuantityLigneOrderTextBox = ProductLigneGrid.FindName("QuantityInventory" + i) as TextBox;
                if (ArticleTextBox != null && StockQuantityTextBox != null && QuantityLigneOrderTextBox != null)
                {
                    // Ajouter les valeurs
                    // Vérifier si l'utilisateur entre toutes les quantités
                    if (string.IsNullOrWhiteSpace(QuantityLigneOrderTextBox.Text))
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

                    if (!int.TryParse(QuantityLigneOrderTextBox.Text, out int quantityValue) || quantityValue <= 0)
                    {
                        MessageBox.Show("Veuillez entrer un nombre entier valide dans la quantité.");
                        isDataValid = false;
                        break;
                    }

                    if (ArticleTextBox.Tag is int productId)
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
                    { "StockQuantity", quantityValue },
                    { "StockId", stockId }
                };
                        InventoryDataList.Add(InventoryData);
                    }
                }
            }

            if (isDataValid)
            {
                var Inventory = new Inventory
                {
                    Date = OrderDate.SelectedDate!.Value,
                    StatusInventory = ApiNegosud.Models.Inventory.InventoryEnum.ENCOURSDEVALIDATION,
                    InventoryLignes = InventoryDataList.Select(data => new InventoryLigne
                    {
                        StockId = (int)data["StockId"],
                        QuantityInventory = (int)data["StockQuantity"],
                    }).ToList()
                };

                var (responseMessage, createdOrder) = await InventoryService.PostInventory(Inventory);
                return (responseMessage, createdOrder);
            }

            return (string.Empty, null);
        }

        private async void SaveInventory(object sender, RoutedEventArgs e)
        {
            var (ResponseMessage, CreatedInventory) = await SaveInventoryStatusInProgress();
            if (!string.IsNullOrEmpty(ResponseMessage) && CreatedInventory != null)
            {
                var Message = MessageBox.Show(ResponseMessage, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                if (Message == MessageBoxResult.OK)
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
            var (ResponseMessage, CreatedInventory) = await SaveInventoryStatusInProgress();
            if (!string.IsNullOrEmpty(ResponseMessage) && CreatedInventory != null)
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
