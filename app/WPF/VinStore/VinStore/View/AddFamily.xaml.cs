using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Logique d'interaction pour AddFamily.xaml
    /// </summary>
    public partial class AddFamily : UserControl
    {
        public AddFamily()
        {
            InitializeComponent();
            LoadFamily();
        }

        private async void LoadFamily()
        {

            var family = await FamilyService.GetFamily();
            FamilyName.ItemsSource = family.Select(f => f.Name).ToList();
            //FamilyName.ItemsSource = family;


        }
        private bool isPopupOpen = false;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Ouvrir le Popup
            textBoxInput.Text = "";
            myPopup.IsOpen = true;
        }


        private async void OKButtonPost_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Récupérez la valeur de la zone de texte
                string userInput = textBoxInput.Text;

                // Appel de la méthode PostFamily
                bool success = await FamilyService.PostFamily(userInput);

                // Vérifiez si la famille a été ajoutée avec succès avant de fermer le Popup
                if (success)
                {
                    MessageBox.Show("Famille ajoutée avec succès!");
                    // Fermez le Popup
                    myPopup.IsOpen = false;

                }
            }
            catch (Exception ex)
            {
                // Gérez les erreurs ici
                MessageBox.Show($"Une erreur s'est produite: {ex.Message}");
            }
        }

        private void myPopup_Closed(object sender, EventArgs e)
        {

            LoadFamily();
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Fermer le Popup

            myPopup.IsOpen = false;
        }

        private void CancelButtonPut_Click(object sender, RoutedEventArgs e)
        {
            // Fermer le Popup

            myPopupPut.IsOpen = false;
        }

        private void ModifyButton_Click(object sender, RoutedEventArgs e)
        {
            // Ouvrir le Popup Put

            Button modifyButton = sender as Button;
            string familyName = modifyButton.DataContext as string;
            textBlockInputPutOld.Text = familyName;
            textBoxInputPutNew.Text = familyName;
            myPopupPut.IsOpen = true;


        }

        private async void OKButtonPut_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                // Obtenez l'objet associé à l'élément ListBox à partir du DataContext du bouton
                string familyNamold = textBlockInputPutOld.Text;
                string familyNameNew = textBoxInputPutNew.Text;
                // Obtenez l'ID de la famille en utilisant une méthode appropriée dans votre service
                int familyId = await FamilyService.GetFamilyIdByNameAsync(familyNamold);

                // Obtenez les nouvelles informations pour la famille (par exemple, à partir d'une boîte de dialogue)


                if (familyNameNew != null)
                {
                    // Appelez la méthode pour effectuer la modification via l'API
                    bool success = await FamilyService.PutFamily(familyId, familyNameNew);
                    myPopupPut.IsOpen = false;
                    if (success)
                    {

                        MessageBox.Show($"La famille '{familyNameNew}' a été modifiée avec succès!");
                        // Rafraîchissez la liste après la modification

                        LoadFamily();
                    }
                    else
                    {
                        MessageBox.Show($"La modification de la famille '{familyNameNew}' a échoué.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite lors de la modification : {ex.Message}");
            }


        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Obtenez le bouton cliqué
                Button deleteButton = sender as Button;

                // Obtenez l'objet associé à l'élément ListBox à partir du DataContext du bouton
                string familyName = deleteButton.DataContext as string;

                // Obtenez l'ID de la famille en utilisant une méthode appropriée dans votre service
                int familyId = await FamilyService.GetFamilyIdByNameAsync(familyName);

                // Confirmez avec l'utilisateur avant de supprimer
                MessageBoxResult result = MessageBox.Show($"Voulez-vous vraiment supprimer la famille '{familyName}' ?", "Confirmation de suppression", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    // Appelez la méthode pour effectuer la suppression via l'API
                    bool success = await FamilyService.DeleteFamily(familyId);

                    if (success)
                    {
                        MessageBox.Show($"La famille '{familyName}' a été supprimée avec succès!");
                        // Rafraîchissez la liste après la suppression
                        LoadFamily();
                    }
                    else
                    {
                        MessageBox.Show($"La suppression de la famille '{familyName}' a échoué.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite lors de la suppression : {ex.Message}");
            }
        }
        



    }
}
