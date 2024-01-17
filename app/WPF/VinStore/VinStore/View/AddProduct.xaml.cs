using ApiNegosud.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
                var providers = await ProviderService.GetProviders();

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

                // Vous pouvez également stocker le chemin de l'image si nécessaire
                // string selectedImagePath = openFileDialog.FileName;
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
                MessageBox.Show("Veuillez entrer un nombre entier valide dans la quantité.");
                e.Handled = true; // Ignorer si pas un entier
            }
        }

        private void TextBox_DecimalInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsDecimalAllowed(e.Text))
            {
                MessageBox.Show("Veuillez entrer un nombre décimal valide dans le prix.");
                e.Handled = true; // Bloquer la saisie si ce n'est pas un nombre décimal avec , ou .
            }
        }
        private bool IsDecimalAllowed(string input)
        {
            var regex = new System.Text.RegularExpressions.Regex(@"^[0-9]*(?:[\.,][0-9]*)?$");
            return regex.IsMatch(input);
        }

        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {

            string relativeFolderPath = @"..\..\..\..\..\..\..\website\public\img\products";

            string image = "";

            if (ImagePreview.Source != null)
            {
                string selectedImagePath = (ImagePreview.Source as BitmapImage)?.UriSource?.LocalPath;

                if (!string.IsNullOrEmpty(selectedImagePath))
                {
                    string uniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    string fileName = System.IO.Path.GetFileName(selectedImagePath);
                    string targetFilePath = System.IO.Path.Combine(relativeFolderPath, uniqueFileName+fileName);

                    image = targetFilePath;

                    // Copier l'image sélectionnée vers le dossier cible
                    File.Copy(selectedImagePath, targetFilePath, true);
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
                    decimal prix = Convert.ToDecimal(txtPrice.Text);
                    string description = txtDescription.Text;
                    DateOnly dateProduction = DateOnly.FromDateTime(OrderDate.SelectedDate.Value);
                    int nbProductBox = Convert.ToInt32(txtNbProductBox.Text);
                    string maison = txtHome.Text;
                    Family famille = FamilyName.SelectedItem as Family;
                    Provider fournisseur = ProvidersName.SelectedItem as Provider;

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

                    await CreateProductOnServer(createdProduct);

                    // Effectuez le traitement nécessaire avec ces valeurs
                    // ...

                    // Vous pouvez également afficher un message avec ces valeurs
                    // MessageBox.Show($"Nom: {nom}\nPrix: {prix}\nDescription: {description}\nDate de production: {dateProduction}\nNombre de produits dans le pack: {nbProductBox}\nMaison: {maison}\nFamille: {famille}\nFournisseur: {fournisseur}");

                    // Réinitialisez les champs du formulaire si nécessaire
                    ResetFormFields();
                }
                else
                {
                    // Affichez le message d'erreur indiquant les champs manquants
                    MessageBox.Show(errorMessage);
                }

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

    }
}

