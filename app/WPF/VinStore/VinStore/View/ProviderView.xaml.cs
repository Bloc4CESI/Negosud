using System;
using ApiNegosud.Models;
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
using System.ComponentModel;
using Newtonsoft.Json;
using System.Collections;
using System.Windows.Controls.Primitives;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;

namespace VinStore.View
{
    /// <summary>
    /// Logique d'interaction pour ProviderView.xaml
    /// </summary>
    public partial class ProviderView : UserControl
    {
        private ObservableCollection<Provider> providers;
        public ProviderView()
        {
            InitializeComponent();
            providers = new ObservableCollection<Provider>();
            InitializeDataProvider();
        }
        private async void InitializeDataProvider()
        {
            // Créez une liste de stock
            List<Provider> providerList = await ProviderService.GetProvidersAll();
            // Assigne la liste à la propriété ItemsSource du DataGrid
            ProviderDataGrid.ItemsSource = providerList;
            Dispatcher.Invoke(() => { }, DispatcherPriority.Render);
        }
        private async void ButtonEdite_Click(object sender, RoutedEventArgs e)
        {
            InitializeDataProvider();
        }
        private async void ButtonSupp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProviderDataGrid.CommitEdit();
                var selectedProvider = ProviderDataGrid.SelectedItem as Provider;
                if (selectedProvider != null)
                {
                    var (success, message) = await ProviderService.DeleteProvider(selectedProvider.Id);
                    if (success)
                    {
                        MessageBox.Show("Le fournisseur a été supprimé avec succès!", "Suppression Réussie", MessageBoxButton.OK, MessageBoxImage.Information);
                        InitializeDataProvider();
                    }
                    else
                    {
                        MessageBox.Show(message, "Suppression Impossible", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite lors de la modification : {ex.Message}");
            }
        }
        private void Button_Open_Provider_AddPopup_Click(object sender, RoutedEventArgs e)
        {
           NewProviderName.Text = "";
           NewProviderPhoneNumber.Text = "";
           NewProviderEmail.Text = "";
           NewProviderAdressName.Text = "";
           NewProviderAdressStreet.Text = "";
           NewProviderAdressnumber.Text = "";
           NewProviderAdresscity.Text = "";
           NewProviderAdresscountry.Text = "";
           Provider_AddPopup.IsOpen = true;
        }
        private string ActivePopup = "";
        private void TextBox_IntegerInput(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _))
            {
                // Déterminer quel popup est actuellement ouvert
                if (Provider_AddPopup.IsOpen)
                {
                    ActivePopup = "Add";
                    Provider_AddPopup.IsOpen = false;
                }
                else if (Provider_PutPopup.IsOpen)
                {
                    ActivePopup = "Put";
                    Provider_PutPopup.IsOpen = false;
                }
                MessageBox.Show("Veuillez entrer un nombre entier valide.");
                e.Handled = true; // Ignorer si pas un entier

                // Réouvrir le popup basé sur ActivePopup
                if (ActivePopup == "Add")
                {
                    Provider_AddPopup.IsOpen = true;
                }
                else if (ActivePopup == "Put")
                {
                    Provider_PutPopup.IsOpen = true;
                }

                // Réinitialiser ActivePopup
                ActivePopup = "";
            }
        }
        private void Button_Open_Provider_PutPopup_Click(object sender, RoutedEventArgs e)
        {
            ProviderDataGrid.CommitEdit();
            var selectProvider = ProviderDataGrid.SelectedItem as Provider;
            if(selectProvider != null)
            {
                ProviderId.Text = selectProvider.Id.ToString();
                ProviderName.Text = selectProvider.Name;
                ProviderPhoneNumber.Text = selectProvider.PhoneNumber;
                ProviderEmail.Text = selectProvider.Email;
                ProviderAdressName.Text = selectProvider.Address?.Name;
                ProviderAdressStreet.Text = selectProvider.Address?.Street;
                ProviderAdressnumber.Text = selectProvider.Address?.Number.ToString();
                ProviderAdresscity.Text = selectProvider.Address?.City;
                ProviderAdresscountry.Text = selectProvider.Address?.Country;
                ProviderAdressId.Text = selectProvider.Address?.Id.ToString();
                Provider_PutPopup.IsOpen = true;
            }
        }
        private void myPopup_Closed(object sender, EventArgs e)
        {
            InitializeDataProvider();
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Provider_AddPopup.IsOpen = false;
            Provider_PutPopup.IsOpen = false;
        }
        private async void ButtonPush_Provider_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool isFormValid = true;
                string errorMessage = "Les champs suivants ne peuvent pas être vides :\n";
                // Vérifier chaque champ
                if (string.IsNullOrWhiteSpace(NewProviderName.Text))
                {
                    errorMessage += "- Nom\n";
                    isFormValid = false;
                }
                if (string.IsNullOrWhiteSpace(NewProviderPhoneNumber.Text))
                {
                    errorMessage += "- Numéro de téléphone\n";
                    isFormValid = false;
                }
                if (!Regex.IsMatch(NewProviderPhoneNumber.Text, @"^\d{10}$"))
                {
                    errorMessage += "- Numéro Téléphone doit contenir exactement 10 chiffres\n";
                    isFormValid = false;
                }

                if (string.IsNullOrWhiteSpace(NewProviderEmail.Text))
                {
                    errorMessage += "- Email\n";
                    isFormValid = false;
                }
                if (string.IsNullOrWhiteSpace(NewProviderAdressName.Text))
                {
                    errorMessage += "- Nom de l'adresse\n";
                    isFormValid = false;
                }
                if (string.IsNullOrWhiteSpace(NewProviderAdressStreet.Text))
                {
                    errorMessage += "- Rue\n";
                    isFormValid = false;
                }
                if (string.IsNullOrWhiteSpace(NewProviderAdressnumber.Text))
                {
                    errorMessage += "- Numéro\n";
                    isFormValid = false;
                }
                if (string.IsNullOrWhiteSpace(NewProviderAdresscity.Text))
                {
                    errorMessage += "- Ville\n";
                    isFormValid = false;
                }
                if (string.IsNullOrWhiteSpace(NewProviderAdresscountry.Text))
                {
                    errorMessage += "- Pays\n";
                    isFormValid = false;
                }
                if (!Regex.IsMatch(NewProviderEmail.Text, @"^[a-zA-Z0-9]+(?:\.[a-zA-Z0-9]+)*@[a-zA-Z0-9]+(?:\.[a-zA-Z0-9]+)*$"))
                {
                    errorMessage += "- Adresse email invalide\n";
                    isFormValid = false;
                }
                // Afficher le message d'erreur si le formulaire n'est pas valide
                if (!isFormValid)
                {
                    Provider_AddPopup.IsOpen = false;
                    MessageBox.Show(errorMessage, "Champs Manquants ou Invalides", MessageBoxButton.OK, MessageBoxImage.Error);
                    Provider_AddPopup.IsOpen = true;
                    return;
                }
                // Création de l'objet Provider
                Provider newProvider = new Provider
                {
                    Name = NewProviderName.Text,
                    PhoneNumber = NewProviderPhoneNumber.Text,
                    Email = NewProviderEmail.Text,
                    Address = new Address
                    {
                        Name = NewProviderAdressName.Text,
                        Street = NewProviderAdressStreet.Text,
                        Number = int.Parse(NewProviderAdressnumber.Text),
                        City = NewProviderAdresscity.Text,
                        Country = NewProviderAdresscountry.Text
                    }
                };
                var (success, message) = await ProviderService.PostProvider(newProvider);
                if (success)
                {
                    Provider_AddPopup.IsOpen = false;
                    MessageBox.Show("Le fournisseur est ajouté avec succès");
                }
                else
                {
                    MessageBox.Show(message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite: {ex.Message}");
            }
        }

        private async void ButtonPut_Provider_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Provider putProvider = new Provider();
                // Récupérez la valeur de la zone de texte
                putProvider.Id = int.Parse(ProviderId.Text);
                putProvider.Name = ProviderName.Text;
                putProvider.PhoneNumber = ProviderPhoneNumber.Text;
                putProvider.Email = ProviderEmail.Text;
                putProvider.Address = new Address
                {
                    Id = int.Parse(ProviderAdressId.Text),
                    Name = ProviderAdressName.Text,
                    Street = ProviderAdressStreet.Text,
                    Number = int.Parse(ProviderAdressnumber.Text),
                    City = ProviderAdresscity.Text,
                    Country = ProviderAdresscountry.Text
                };
                bool isFormValid = true;
                string errorMessage = "Les champs suivants ne peuvent pas être vides :\n";
                // Vérifier chaque champ
                if (string.IsNullOrWhiteSpace(ProviderName.Text))
                {
                    errorMessage += "- Nom\n";
                    isFormValid = false;
                }
                if (string.IsNullOrWhiteSpace(ProviderPhoneNumber.Text))
                {
                    errorMessage += "- Numéro de téléphone\n";
                    isFormValid = false;
                }
                if (!Regex.IsMatch(ProviderPhoneNumber.Text, @"^\d{10}$"))
                {
                    errorMessage += "- Numéro Téléphone doit contenir exactement 10 chiffres\n";
                    isFormValid = false;
                }
                if (string.IsNullOrWhiteSpace(ProviderEmail.Text))
                {
                    errorMessage += "- Email\n";
                    isFormValid = false;
                }
                if (string.IsNullOrWhiteSpace(ProviderAdressName.Text))
                {
                    errorMessage += "- Nom de l'adresse\n";
                    isFormValid = false;
                }
                if (string.IsNullOrWhiteSpace(ProviderAdressStreet.Text))
                {
                    errorMessage += "- Rue\n";
                    isFormValid = false;
                }
                if (string.IsNullOrWhiteSpace(ProviderAdressnumber.Text))
                {
                    errorMessage += "- Numéro\n";
                    isFormValid = false;
                }
                if (string.IsNullOrWhiteSpace(ProviderAdresscity.Text))
                {
                    errorMessage += "- Ville\n";
                    isFormValid = false;
                }
                if (string.IsNullOrWhiteSpace(ProviderAdresscountry.Text))
                {
                    errorMessage += "- Pays\n";
                    isFormValid = false;
                }
                if (!Regex.IsMatch(ProviderEmail.Text, @"^[a-zA-Z0-9]+(?:\.[a-zA-Z0-9]+)*@[a-zA-Z0-9]+(?:\.[a-zA-Z0-9]+)*$"))
                {
                    errorMessage += "- Adresse email invalide\n";
                    isFormValid = false;
                }
                // Afficher le message d'erreur si le formulaire n'est pas valide
                if (!isFormValid)
                {
                    Provider_PutPopup.IsOpen = false;
                    MessageBox.Show(errorMessage, "Champs Manquants ou Invalides", MessageBoxButton.OK, MessageBoxImage.Error);
                    Provider_PutPopup.IsOpen = true;
                    return;
                }
                // Appel de la méthode PutFamily
                var (success, message) = await ProviderService.PutProvider(putProvider.Id, putProvider);
                if (success)
                {
                    Provider_PutPopup.IsOpen = false;
                    MessageBox.Show("Le fournisseur a été mis à jour avec succès!");
                }
                else
                {
                    Provider_PutPopup.IsOpen = false;
                    MessageBox.Show(message);
                    Provider_PutPopup.IsOpen = true;
                }
            }
            catch (Exception ex)
            {
                // Gérez les erreurs ici
                MessageBox.Show($"Une erreur s'est produite: {ex.Message}");
            }
        }
        private async void SearchWithProviderName(object sender, RoutedEventArgs e)
        {
            var providerName = ProviderNameTextBox.Text;

            if (!string.IsNullOrEmpty(providerName))
            {
                List<Provider> ProviderList = await ProviderService.GetProvidersAll(providerName);
                ProviderDataGrid.ItemsSource = ProviderList;
            }
            else
            {
                InitializeDataProvider();
            }

        }

    }
}
