using ApiNegosud.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using VinStore.Services;
using static System.Net.Mime.MediaTypeNames;
using RestSharp;
using System.Globalization;
using System.Diagnostics;

namespace VinStore.View
{
    /// <summary>
    /// Logique d'interaction pour AddProduct.xaml
    /// </summary>
    public partial class AddProduct : UserControl
    {
        public AddProduct()
        {
            InitializeComponent();
            LoadProviders();
            LoadFamily();
        }
        private async void LoadProviders()
        {
            try
            {
                var providers = await ProviderService.GetProvidersAll();

                // Utiliser Dispatcher.Invoke pour mettre à jour l'interface utilisateur
                Dispatcher.Invoke(() =>
                {
                    ProvidersName.ItemsSource = providers;
                    ProvidersName.DisplayMemberPath = "Name";
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la requête : {ex.Message}");
                MessageBox.Show($"{ex.Message}");
            }
     
        }
        private async void LoadFamily()
        {
            try
            {
            var family = await FamilyService.GetFamily();
            FamilyName.ItemsSource = family;
            FamilyName.DisplayMemberPath = "Name";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la requête : {ex.Message}");
                MessageBox.Show($"{ex.Message}");
            }
        }
        private void ChooseImageButton_Click(object sender, RoutedEventArgs e)
        {
            // Utilisez un OpenFileDialog pour permettre à l'utilisateur de choisir plusieurs images
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Fichiers image (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp|Tous les fichiers (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string selectedImagePath = openFileDialog.FileName;
                // Affichez l'aperçu de l'image sélectionnée
                ImagePreview.Source = new BitmapImage(new Uri(selectedImagePath));
            }
        }

        private async Task CreateProductOnServer(Product updatedProduct)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            };
            // Construisez le JSON avec les données de l'objet Product
            string productName = updatedProduct.Name;
            string jsonData = JsonConvert.SerializeObject(updatedProduct, settings);

            // Appelez une méthode de service qui enverra la requête HTTP PUT
            await ProductService.PostProduct(jsonData, productName);
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
        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string image = "";
            if (ImagePreview.Source != null)
            {
                var selectedImagePath = (ImagePreview.Source as BitmapImage)?.UriSource?.LocalPath;
                if (!string.IsNullOrEmpty(selectedImagePath))
                {
                    string fileName = System.IO.Path.GetFileName(selectedImagePath);
                    string uniqueFileName = System.IO.Path.Combine(DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileName);

                    image = postImgurImg(selectedImagePath, uniqueFileName);
                }
                bool isFormValid = true;
                string errorMessage = "Veuillez remplir les champs:\n";
                if (string.IsNullOrEmpty(txtName.Text))
                {
                    errorMessage += "- Nom\n";
                    isFormValid = false;
                }
                if (string.IsNullOrEmpty(txtPrice.Text))
                {
                    errorMessage += "- Prix\n";
                    isFormValid = false;
                }
                if (OrderDate.SelectedDate == null)
                {
                    errorMessage += "- Date de production\n";
                    isFormValid = false;
                }
                if (string.IsNullOrEmpty(txtNbProductBox.Text))
                {
                    errorMessage += "- Nombre de produits dans le pack\n";
                    isFormValid = false;
                }
                if (string.IsNullOrEmpty(txtHome.Text))
                {
                    errorMessage += "- Maison\n";
                    isFormValid = false;
                }
                if (FamilyName.SelectedItem == null)
                {
                    errorMessage += "- Famille\n";
                    isFormValid = false;
                }
                if (ProvidersName.SelectedItem == null)
                {
                    errorMessage += "- Fournisseur\n";
                    isFormValid = false;
                }
                if (string.IsNullOrEmpty(image))
                {
                    errorMessage += "- Image\n";
                    isFormValid = false;
                }
                if (isFormValid)
                {
                    // Le formulaire est valide, procédez avec le reste du code
                    string nom = txtName.Text;
                    decimal.TryParse(txtPrice.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal prix);
                    string description = txtDescription.Text;
                    DateOnly dateProduction = DateOnly.FromDateTime(OrderDate.SelectedDate!.Value);
                    int nbProductBox = Convert.ToInt32(txtNbProductBox.Text);
                    string maison = txtHome.Text;
                    var famille = FamilyName.SelectedItem as Family;
                    var fournisseur = ProvidersName.SelectedItem as Provider;
                    Product createdProduct = new Product
                    {
                        Name = nom,
                        Price = Convert.ToDecimal(prix),
                        Description = description,
                        DateProduction = dateProduction,
                        NbProductBox = nbProductBox,
                        Home = maison,
                        FamilyId = famille?.Id ?? 0, // Utilisez l'Id de la famille si elle n'est pas nulle, sinon 0
                        ProviderId = fournisseur?.Id ?? 0, // Utilisez l'Id du fournisseur s'il n'est pas nul, sinon 0
                        Image = image
                    };
                    createdProduct.Stock = new Stock 
                    { 
                        Quantity = 0,
                        Minimum = 0,
                        Maximum = 0,
                        AutoOrder = false                    
                    };
                    await CreateProductOnServer(createdProduct);
                    // Réinitialisez les champs du formulaire si nécessaire
                    ResetFormFields();
                }
                else
                {
                    // Affichez le message d'erreur indiquant les champs manquants
                    MessageBox.Show(errorMessage);
                }
            }
            else
            {
                MessageBox.Show("Veuillez remplir l'image du produit ");
            }
        }

        private void ResetFormFields()
        {
            // Réinitialisez les valeurs des champs du formulaire à vide ou à leur état initial
            txtName.Text = "";
            ImagePreview.Source = null;
            txtPrice.Text = "";
            txtDescription.Text = "";
            OrderDate.Text = "";
            txtNbProductBox.Text = "";
            txtHome.Text = "";
            FamilyName.SelectedIndex = -1; // Réinitialisez la ComboBox "Famille"
            ProvidersName.SelectedIndex = -1; // Réinitialisez la ComboBox "Fournisseur"
        }
        private string postImgurImg(string selectedpath, string imagename)
        {
            // Remplacez "VOTRE_CLE_API" par votre clé API Imgur
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

    }
}

