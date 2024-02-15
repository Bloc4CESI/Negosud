using ApiNegosud.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using VinStore.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace VinStore.View
{
    /// <summary>
    /// Logique d'interaction pour ListProduct.xaml
    /// </summary>
    public partial class ListProduct : UserControl
    {
        private ObservableCollection<Product> products;

        public ListProduct(bool loadAlertProducts = false)
        {
            InitializeComponent();
            products = new ObservableCollection<Product>();
            InitializeData(loadAlertProducts);
            InitScroll();
        }
        private async void InitializeData(bool loadAlertProducts = false)
        {
            List<Product> loadedProducts;
            if (loadAlertProducts)
            {
                // Charger les produits qui ont une quantité en dessous du seuil
                loadedProducts = await ProductService.GetAlertProducts();
            }
            else
            {
                // Charger tous les produits
                loadedProducts = await ProductService.GetAllProducts();
            }
            productDataGrid.ItemsSource = loadedProducts;
            Dispatcher.Invoke(() => { }, DispatcherPriority.Render);
        }
        private void InitScroll()
        {
            double nouvelleHauteur = SystemParameters.PrimaryScreenHeight * 0.75;
            double nouvelleHauteurDeuxiemeLigne = nouvelleHauteur; // Ajustez selon vos besoins
                                                                   // Assurez-vous que la nouvelle hauteur de la deuxième ligne est positive
            if (nouvelleHauteurDeuxiemeLigne < 0)
            {
                nouvelleHauteurDeuxiemeLigne = 0;
            }
            // Mettez à jour la hauteur de la deuxième ligne
            MyGrid.RowDefinitions[1].Height = new GridLength(nouvelleHauteurDeuxiemeLigne);
        }


        private async void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            // Terminer l'édition pour s'assurer que les modifications sont prises en compte
            productDataGrid.CommitEdit();
            // Récupérer l'objet Product associé à la ligne sélectionnée
            var selectedProduct = productDataGrid.SelectedItem as Product;
            if (selectedProduct != null)
            {
                // Vérifier les champs requis du produit
                bool isFormValid = true;
                string errorMessage = "Veuillez remplir les champs: ";
                if (string.IsNullOrWhiteSpace(selectedProduct?.Name))
                {
                    errorMessage += " Nom ";
                    isFormValid = false;
                }
                if (string.IsNullOrWhiteSpace(selectedProduct?.Description))
                {
                    errorMessage += " Description";
                    isFormValid = false;
                }
                if (selectedProduct?.NbProductBox <= 0 || string.IsNullOrEmpty(selectedProduct?.NbProductBox.ToString()))
                {
                    MessageBox.Show("Veuillez entrer une quantité valide (supérieure à 0).", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Vérifier que le prix est valide
                if (selectedProduct?.Price <= 0 || string.IsNullOrEmpty(selectedProduct?.Price.ToString()))
                {
                    MessageBox.Show("Veuillez entrer un prix valide (supérieur à 0).", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (selectedProduct.Image != null)
                {
                    string selectedImagePath = selectedProduct.Image;
                    string fileName = System.IO.Path.GetFileName(selectedImagePath);
                    string uniqueFileName = System.IO.Path.Combine(DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileName);
                    //string targetFilePath = System.IO.Path.Combine(relativeFolderPath, uniqueFileName);

                    selectedProduct.Image = postImgurImg(selectedImagePath, uniqueFileName);

                    // Copier l'image sélectionnée vers le dossier cible
                    //File.Copy(selectedImagePath, targetFilePath, true);
                }
                if (!isFormValid)
                {
                    MessageBox.Show(errorMessage, "Champs manquants", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }           
                await UpdateProductOnServer(selectedProduct);
                // Retirer l'ancien produit
                products.Remove(selectedProduct);
                // Ajouter le produit mis à jour
                products.Add(await ProductService.GetProductById(selectedProduct.Id));
                // Rafraîchir la vue du DataGrid 
                InitializeData(false);
            }
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
            var selectedProduct = ((FrameworkElement)sender).DataContext as Product;
            if (selectedProduct != null)
            {
                bool isHasAnyTransaction = await ProductService.IsProductHasAnyTransaction(selectedProduct.Id);
                // Si des inventaires ou des commandes sont associés au produit on supprime pas
                if (isHasAnyTransaction)
                {
                    MessageBox.Show("Vous ne pouvez pas supprimer ce produit car il existe des commandes ou des inventaires associés à ce produit.", "Impossibilité de suppression", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    await DeleteProductOnServer(selectedProduct);
                    // Retirer l'ancien produit
                    products.Remove(selectedProduct);
                    // Rafraîchir la vue du DataGrid
                    InitializeData(false);
                }
            }
        }

        private async Task DeleteProductOnServer(Product updatedProduct)
        {
            // Construisez le JSON avec les données de l'objet Product
            int id = updatedProduct.Id;
            string productName = updatedProduct.Name;

            // Appelez une méthode de service qui enverra la requête HTTP PUT
            await ProductService.DeleteProduct(id, productName);
        }

        private void UpdateImageButton_Click(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Image clickedImage = sender as System.Windows.Controls.Image;

            // Obtenez la cellule à partir de l'image
            DataGridCell cell = FindVisualParent<DataGridCell>(clickedImage);

            // Vérifiez si la cellule est non nulle et obtenez la ligne associée
            if (cell != null)
            {
                DataGridRow row = FindVisualParent<DataGridRow>(cell);

                // Obtenez l'objet Product lié à la ligne
                if (row != null && row.Item is Product selectedProduct)
                {
                    // Utilisez un OpenFileDialog pour permettre à l'utilisateur de choisir une nouvelle image
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "Fichiers image (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp|Tous les fichiers (*.*)|*.*";

                    if (openFileDialog.ShowDialog() == true)
                    {

                        string selectedImagePath = openFileDialog.FileName;

                        // Mettez à jour la propriété Image de cet objet
                        selectedProduct.Image = selectedImagePath;

                        // Rafraîchissez le DataGrid pour refléter les changements
                        Dispatcher.Invoke(() =>
                        {
                            // Actualisez la vue pour refléter les changements
                            productDataGrid.Items.Refresh();
                        }, DispatcherPriority.Background);

                    }
                }
            }

        }
        private T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null)
                return null;

            if (parentObject is T parent)
                return parent;

            return FindVisualParent<T>(parentObject);
        }

        private string postImgurImg(string selectedpath, string imagename)
        {
            string apiKey = "f8e21651c328f2f";
            // Appel de la méthode pour charger l'image sur Imgur et obtenir le lien
            string imgurLink = UploadImageToImgur(selectedpath, apiKey, imagename);
            // Afficher le lien Imgur
            Console.WriteLine("Lien Imgur : " + imgurLink);
            return imgurLink;
        }

        static string UploadImageToImgur(string imagePath, string apiKey, string filename)
        {
            // Charger le fichier image en bytes
            byte[] imageData = File.ReadAllBytes(imagePath);
            // Créer une demande à l'API Imgur
            var client = new RestClient("https://api.imgur.com/3");
            var request = new RestRequest("image", Method.Post);
            // Ajouter l'image en tant que fichier à la demande
            request.AddFile("image", imageData, filename);
            // Ajouter l'en-tête d'autorisation avec votre clé API Imgur
            request.AddHeader("Authorization", $"Client-ID {apiKey}");
            // Exécuter la demande
            RestResponse response = client.Execute(request);
            // Analyser la réponse JSON pour obtenir le lien Imgur
            dynamic jsonResponse = JsonConvert.DeserializeObject(response.Content);
            string imgurLink = jsonResponse.data.link;
            return imgurLink;
        }

        public async Task LoadAlertProducts()
        {
            var alertProducts = await ProductService.GetAlertProducts();
            Dispatcher.Invoke(() =>
            {
                productDataGrid.ItemsSource = alertProducts;
            });
        }
        private void TextBox_IntegerInput(object sender, TextCompositionEventArgs e)
        {
            // Vérifier si le texte est un entier
            if (!int.TryParse(e.Text, out _))
            {
                MessageBox.Show("Veuillez entrer un nombre entier valide dans la quantité carton.");
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

    }
}

