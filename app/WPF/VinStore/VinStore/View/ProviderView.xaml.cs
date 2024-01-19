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
            List<Provider> ProviderList = await ProviderService.GetProvidersAll();


            // Assigne la liste à la propriété ItemsSource du DataGrid
            ProviderDataGrid.ItemsSource = ProviderList;
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
   
                Provider selectedProvider = ProviderDataGrid.SelectedItem as Provider;

                

                if (selectedProvider != null)
                {
                    // Appelez la méthode pour effectuer la modification via l'API
                    bool success = await ProviderService.DeleteProvider(selectedProvider.Id);

                    if (success)
                    {

                        MessageBox.Show($"Le ProvIder '{selectedProvider.Name}' a été supprimé avec succès!");
                        // Rafraîchissez la liste après la modification

                        InitializeDataProvider();
                    }
                    else
                    {
                        MessageBox.Show($"La suppression du provider '{selectedProvider.Name}' a échoué.");
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

            //Provider_AddPopup.HorizontalOffset = 100;
            //Provider_AddPopup.VerticalOffset = 100;
            //Provider_AddPopup.HorizontalAlignment = HorizontalAlignment.Center;
            
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
        private void Button_Open_Provider_PutPopup_Click(object sender, RoutedEventArgs e)
        {

            ProviderDataGrid.CommitEdit();
 
            Provider selectProvider = ProviderDataGrid.SelectedItem as Provider;

            ProviderId.Text = selectProvider.Id.ToString();
            ProviderName.Text = selectProvider.Name;
            ProviderPhoneNumber.Text = selectProvider.PhoneNumber;
            ProviderEmail.Text = selectProvider.Email;
            ProviderAdressName.Text = selectProvider.Address.Name;
            ProviderAdressStreet.Text = selectProvider.Address.Street;
            ProviderAdressnumber.Text = selectProvider.Address.Number.ToString();
            ProviderAdresscity.Text = selectProvider.Address.City;
            ProviderAdresscountry.Text = selectProvider.Address.Country;
            ProviderAdressId.Text = selectProvider.Address.Id.ToString();

            Provider_PutPopup.IsOpen = true;

        }

        private void myPopup_Closed(object sender, EventArgs e)
        {

            InitializeDataProvider();
        }



        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Fermer le Popup

            Provider_AddPopup.IsOpen = false;
            Provider_PutPopup.IsOpen = false;
        }

       private async void ButtonPush_Provider_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                Provider newProvider = new Provider();

                // Récupérez la valeur de la zone de texte
                newProvider.Name = NewProviderName.Text;
                newProvider.PhoneNumber = NewProviderPhoneNumber.Text;
                newProvider.Email = NewProviderEmail.Text;

                newProvider.Address = new Address
                {
                    Name = NewProviderAdressName.Text,
                    Street = NewProviderAdressStreet.Text,
                    Number = int.Parse(NewProviderAdressnumber.Text),
                    City = NewProviderAdresscity.Text,
                    Country = NewProviderAdresscountry.Text
                };


               


                // Appel de la méthode PostFamily
                bool success = await ProviderService.PostProvider(newProvider);

                // Vérifiez si la famille a été ajoutée avec succès avant de fermer le Popup
                if (success)
                {
                    Provider_AddPopup.IsOpen = false;

                    MessageBox.Show("Provider ajoutée avec succès!");
                    // Fermez le Popup
                    

                }
                else
                {
                    Provider_AddPopup.IsOpen = false;
                    MessageBox.Show($"L'ajout du provider a échoué.");
                    Provider_AddPopup.IsOpen = true;
                }
            }
            catch (Exception ex)
            {
                // Gérez les erreurs ici
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


                // Appel de la méthode PutFamily
                bool success = await ProviderService.PutProvider(putProvider.Id, putProvider);

                // Vérifiez si la famille a été ajoutée avec succès avant de fermer le Popup
                if (success)
                {
                    Provider_PutPopup.IsOpen = false;

                    MessageBox.Show("Provider ajoutée avec succès!");
                    // Fermez le Popup


                }
                else
                {
                    Provider_AddPopup.IsOpen = false;
                    MessageBox.Show($"L'ajout du provider a échoué.");
                    Provider_AddPopup.IsOpen = true;
                }
            }
            catch (Exception ex)
            {
                // Gérez les erreurs ici
                MessageBox.Show($"Une erreur s'est produite: {ex.Message}");
            }





        }

    }
}
