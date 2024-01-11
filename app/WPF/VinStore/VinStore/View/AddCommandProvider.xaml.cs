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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
        public AddCommandProvider()
        {
            InitializeComponent();
            OrderDate.SelectedDate= DateTime.Now;
            LoadProviders();

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
        private int productComboBoxIndex = 0;
        private void AddProduct(object sender, RoutedEventArgs e)
        {
            // Créer une nouvelle ligne à ajouter à la grille
            RowDefinition newRow = new RowDefinition();
            newRow.Height = GridLength.Auto;
            ProductLigneGrid.RowDefinitions.Add(newRow);
            int currentRow = ProductLigneGrid.RowDefinitions.Count - 1;

            // Ajouter les contrôles à la nouvelle ligne
            ComboBox newComboBox = new ComboBox
            {
                Margin = new Thickness(20),
                Background = Brushes.White,
                Padding = new Thickness(10),
                Name = "ProductsComboBox_" + productComboBoxIndex
            };           
            Grid.SetColumn(newComboBox, 0);
            Grid.SetColumnSpan(newComboBox, 2);
            Grid.SetRow(newComboBox, currentRow);
            // Configurer les propriétés Material Design
            HintAssist.SetHint(newComboBox, "Nom de l'article");
            TextFieldAssist.SetUnderlineBrush(newComboBox, Brushes.Black);
            HintAssist.SetHintOpacity(newComboBox, 0.26);
            newComboBox.MaxHeight = 50;
            newComboBox.Foreground = Brushes.Black;
            newComboBox.IsEditable = true;
            // ajouter un evennt listener à chaque combox 
            newComboBox.SelectionChanged += ProductComboBox_SelectionChanged;
            productComboBoxIndex++;
            TextBox newTextBox1 = new TextBox
            {
                Margin = new Thickness(20),
                Background = Brushes.White,
                Padding = new Thickness(10),
                IsReadOnly = true
            };
            Grid.SetColumn(newTextBox1, 2);
            Grid.SetRow(newTextBox1, currentRow);

            // Configurer les propriétés Material Design
            HintAssist.SetHint(newTextBox1, "Quantité stock");
            TextFieldAssist.SetUnderlineBrush(newTextBox1, Brushes.Black);
            HintAssist.SetHintOpacity(newTextBox1, 0.26);
            newTextBox1.MaxHeight = 50;
            newTextBox1.Foreground = Brushes.Black;

            TextBox newTextBox2 = new TextBox
            {
                Margin = new Thickness(20),
                Background = Brushes.White,
                Padding = new Thickness(10)
            };
            Grid.SetColumn(newTextBox2, 3);
            Grid.SetRow(newTextBox2, currentRow);

            // Configurer les propriétés Material Design
            HintAssist.SetHint(newTextBox2, "Quantité");
            TextFieldAssist.SetUnderlineBrush(newTextBox2, Brushes.Black);
            HintAssist.SetHintOpacity(newTextBox2, 0.26);
            newTextBox2.MaxHeight = 50;
            newTextBox2.Foreground = Brushes.Black;

            TextBox newTextBox3 = new TextBox
            {
                Margin = new Thickness(20),
                Background = Brushes.White,
                Padding = new Thickness(10)
            };
            Grid.SetColumn(newTextBox3, 4);
            Grid.SetRow(newTextBox3, currentRow);

            // Configurer les propriétés Material Design
            HintAssist.SetHint(newTextBox3, "Prix article");
            TextFieldAssist.SetUnderlineBrush(newTextBox3, Brushes.Black);
            HintAssist.SetHintOpacity(newTextBox3, 0.26);
            newTextBox3.MaxHeight = 50;
            newTextBox3.Foreground = Brushes.Black;

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
            Grid.SetColumn(addButton, 5);
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
            Grid.SetColumn(deleteButton, 6);
            Grid.SetRow(deleteButton, currentRow);
            ProductLigneGrid.Children.Add(newComboBox);
            ProductLigneGrid.Children.Add(newTextBox1);
            ProductLigneGrid.Children.Add(newTextBox2);
            ProductLigneGrid.Children.Add(newTextBox3);
            ProductLigneGrid.Children.Add(addButton);
            ProductLigneGrid.Children.Add(deleteButton);
            ProductLigneGrid.UpdateLayout();
        }
        private async void ProviderSelected(object sender, RoutedEventArgs e)
        {
            if (ProvidersName.SelectedItem is Provider selectedProvider)
            {
                AddressProvider.Text = selectedProvider.Address?.ToString() ?? string.Empty;
                var Products = await ProductService.GetProductByProvider(selectedProvider.Id);
                ProductsComboBox.ItemsSource = Products;
                ProductsComboBox.DisplayMemberPath = "Name";
                if (productComboBoxIndex > 0)
                {
                    var comboBoxName = "ProductsComboBox_" + productComboBoxIndex;
                    ComboBox? selectedComboBox = FindName(comboBoxName) as ComboBox;
                    if (selectedComboBox!=null)
                    {
                        selectedComboBox.ItemsSource = Products;
                        selectedComboBox.DisplayMemberPath = "Name";
                    }
                   
                }
            }
        }
        private async void ProductComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                if (comboBox.SelectedItem is Product selectedProduct)
                {
                    var productDetails = await ProductService.GetProductById(selectedProduct.Id);
                    StockQuantity.Text = productDetails.Stock?.Quantity.ToString() ?? "N/A";
                }


            }
        }
    }
}
