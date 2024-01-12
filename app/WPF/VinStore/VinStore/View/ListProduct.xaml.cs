using ApiNegosud.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;
using VinStore.Services;

namespace VinStore.View
{
    /// <summary>
    /// Logique d'interaction pour ListProduct.xaml
    /// </summary>
    public partial class ListProduct : UserControl
    {
        private ObservableCollection<Product> products;

        public ListProduct()
        {
            InitializeComponent();
            products = new ObservableCollection<Product>();
            InitializeData();
        }

        private async void InitializeData()
        {
            // Créez une liste de produits
            List<Product> products = await ProductService.GetAllProducts();

            // Ajoutez d'autres produits selon vos besoins

            // Assigne la liste à la propriété ItemsSource du DataGrid
            productDataGrid.ItemsSource = products;

            Dispatcher.Invoke(() => { }, DispatcherPriority.Render);
        }
        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            // Terminer l'édition pour s'assurer que les modifications sont prises en compte
            productDataGrid.CommitEdit();

            // Récupérer l'objet Product associé à la ligne sélectionnée
            Product selectedProduct = productDataGrid.SelectedItem as Product;

            UpdateProductOnServer(selectedProduct);
            InitializeData();
        }

        private async void UpdateProductOnServer(Product updatedProduct)
        {
            // Construisez le JSON avec les données de l'objet Product
            int id = updatedProduct.Id;
            string productName = updatedProduct.Name;
            string jsonData = JsonConvert.SerializeObject(updatedProduct);

            // Appelez une méthode de service qui enverra la requête HTTP PUT
            await ProductService.EditProduct(jsonData, id, productName);
        }

    }
}