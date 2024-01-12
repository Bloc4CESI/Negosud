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
        private async void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            // Terminer l'édition pour s'assurer que les modifications sont prises en compte
            productDataGrid.CommitEdit();

            // Récupérer l'objet Product associé à la ligne sélectionnée
            Product selectedProduct = productDataGrid.SelectedItem as Product;

            await UpdateProductOnServer(selectedProduct);

            // Actualiser la collection locale
            // Retirer l'ancien produit
            products.Remove(selectedProduct);

            // Ajouter le produit mis à jour
            products.Add(await ProductService.GetProductById(selectedProduct.Id));

            // Rafraîchir la vue du DataGrid pour forcer la mise à jour
            CollectionViewSource.GetDefaultView(productDataGrid.ItemsSource).Refresh();

        }

        private async Task UpdateProductOnServer(Product updatedProduct)
        {
            // Construisez le JSON avec les données de l'objet Product
            int id = updatedProduct.Id;
            string productName = updatedProduct.Name;
            string jsonData = JsonConvert.SerializeObject(updatedProduct);

            // Appelez une méthode de service qui enverra la requête HTTP PUT
            await ProductService.EditProduct(jsonData, id, productName);
        }
        private async void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            // Terminer l'édition pour s'assurer que les modifications sont prises en compte
            productDataGrid.CommitEdit();

            // Récupérer l'objet Product associé à la ligne sélectionnée
            Product selectedProduct = productDataGrid.SelectedItem as Product;

            await DeleteProductOnServer(selectedProduct);

            // Actualiser la collection locale
            // Retirer l'ancien produit
            products.Remove(selectedProduct);

            // Rafraîchir la vue du DataGrid pour forcer la mise à jour
            CollectionViewSource.GetDefaultView(productDataGrid.ItemsSource).Refresh();

        }
        private async Task DeleteProductOnServer(Product updatedProduct)
        {
            // Construisez le JSON avec les données de l'objet Product
            int id = updatedProduct.Id;
            string productName = updatedProduct.Name;

            // Appelez une méthode de service qui enverra la requête HTTP PUT
            await ProductService.DeleteProduct(id, productName);
        }

    }
}