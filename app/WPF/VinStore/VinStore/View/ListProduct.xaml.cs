﻿using ApiNegosud.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

        public ListProduct(bool loadAlertProducts = false)
        {
            InitializeComponent();
            products = new ObservableCollection<Product>();

            InitializeData(loadAlertProducts);
            InitScroll();
            //SizeChanged += WindowSizeChanged;


        }

   



        private bool enTrainDeMettreAJour = false;

        private async void WindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!enTrainDeMettreAJour)
            {
                enTrainDeMettreAJour = true;
                
                // Obtenez la nouvelle hauteur de la fenêtre
                double nouvelleHauteur = this.ActualHeight;

                // Ajustez la hauteur de la deuxième ligne en fonction de la nouvelle hauteur de la fenêtre
                double nouvelleHauteurDeuxiemeLigne = nouvelleHauteur; // Ajustez selon vos besoins

                textBlockVariable.Text = nouvelleHauteur.ToString();

                // Assurez-vous que la nouvelle hauteur de la deuxième ligne est positive
                if (nouvelleHauteurDeuxiemeLigne < 0)
                {
                    nouvelleHauteurDeuxiemeLigne = 0;
                }

                // Mettez à jour la hauteur de la deuxième ligne
                Dispatcher.Invoke(() =>
                {
                    // Mettez à jour la hauteur de la deuxième ligne
                    MyGrid.RowDefinitions[1].Height = new GridLength(nouvelleHauteurDeuxiemeLigne);
                });

                await Task.Delay(1000); 
            }
            else 
            { 
            enTrainDeMettreAJour = false;
            }
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
            foreach (var product in loadedProducts)
            {
                products.Add(product);
            }
            productDataGrid.ItemsSource = products;
            Dispatcher.Invoke(() => { }, DispatcherPriority.Render);
             // Créez une liste de produits
            List<Product> products = await ProductService.GetAllProducts();
            // Assigne la liste à la propriété ItemsSource du DataGrid
            productDataGrid.ItemsSource = products;

            Dispatcher.Invoke(() => { }, DispatcherPriority.Render);
        private void InitScroll() 
        {

            double nouvelleHauteur = SystemParameters.PrimaryScreenHeight * 0.75;
            double nouvelleHauteurDeuxiemeLigne = nouvelleHauteur; // Ajustez selon vos besoins

            textBlockVariable.Text = nouvelleHauteur.ToString();

            // Assurez-vous que la nouvelle hauteur de la deuxième ligne est positive
            if (nouvelleHauteurDeuxiemeLigne < 0)
            {
                nouvelleHauteurDeuxiemeLigne = 0;
            }

            // Mettez à jour la hauteur de la deuxième ligne
          
                // Mettez à jour la hauteur de la deuxième ligne
                MyGrid.RowDefinitions[1].Height = new GridLength(nouvelleHauteurDeuxiemeLigne);
           


        }


        private async void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            // Terminer l'édition pour s'assurer que les modifications sont prises en compte
            productDataGrid.CommitEdit();

            // Récupérer l'objet Product associé à la ligne sélectionnée
            Product selectedProduct = productDataGrid.SelectedItem as Product;

            if (selectedProduct.Image != null)
            {
                string selectedImagePath = selectedProduct.Image;
                string fileName = System.IO.Path.GetFileName(selectedImagePath);
                string uniqueFileName = System.IO.Path.Combine(DateTime.Now.ToString("yyyyMMddHHmmssfff")+fileName);
                //string targetFilePath = System.IO.Path.Combine(relativeFolderPath, uniqueFileName);

                selectedProduct.Image = postImgurImg(selectedImagePath, uniqueFileName);

                // Copier l'image sélectionnée vers le dossier cible
                //File.Copy(selectedImagePath, targetFilePath, true);
            }

            await UpdateProductOnServer(selectedProduct);
            // Actualiser la collection locale
            // Retirer l'ancien produit
            products.Remove(selectedProduct);
            // Ajouter le produit mis à jour
            products.Add(await ProductService.GetProductById(selectedProduct.Id));
            // Rafraîchir la vue du DataGrid 
            InitializeData(false);

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

        private void UpdateImageButton_Click(object sender, RoutedEventArgs e)
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


        private void productDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

