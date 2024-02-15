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
        private bool isPopupOpen = false;
        public AddFamily()
        {
            InitializeComponent();
            LoadFamily();
        }

        private async void LoadFamily()
        {
            var family = await FamilyService.GetFamily();
            FamilyName.ItemsSource = family.Select(f => f.Name).ToList();
        }
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
                bool isDataValid = true;
                string userInput = textBoxInput.Text;
                if (string.IsNullOrWhiteSpace(userInput))
                {
                    MessageBox.Show("Veuillez entrer le nom de la famille.");
                    isDataValid = false;
                }
                if (isDataValid)
                {
                    var (success, message) = await FamilyService.PostFamily(userInput);
                    MessageBox.Show(message);
                    if (success)
                    {
                        // Fermez le Popup et rafraîchissez si nécessaire
                        myPopup.IsOpen = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite: {ex.Message}");
            }
        }
        private void myPopup_Closed(object sender, EventArgs e)
        {
            LoadFamily();
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            myPopup.IsOpen = false;
        }
        private void CancelButtonPut_Click(object sender, RoutedEventArgs e)
        {
            myPopupPut.IsOpen = false;
        }
        private void ModifyButton_Click(object sender, RoutedEventArgs e)
        {
            // Ouvrir le Popup Put
            var ModifyButton = sender as Button;
            if( ModifyButton != null)
            {
                string FamilyName = ModifyButton.DataContext as string;
                textBlockInputPutOld.Text = FamilyName;
                textBoxInputPutNew.Text = FamilyName;
                myPopupPut.IsOpen = true;
            }
        }
        private async void OKButtonPut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string FamilyNameOld = textBlockInputPutOld.Text;
                string FamilyNameNew = textBoxInputPutNew.Text;
                int FamilyId = await FamilyService.GetFamilyIdByNameAsync(FamilyNameOld);
                bool isDataValid = true;
                if (string.IsNullOrWhiteSpace(FamilyNameNew))
                {
                    MessageBox.Show("Veuillez entrer le nom de la famille.");
                    isDataValid = false;
                }
                if (isDataValid && FamilyNameNew != null)
                {
                    var (success, message) = await FamilyService.PutFamily(FamilyId, FamilyNameNew);
                    myPopupPut.IsOpen = false;
                    MessageBox.Show(message); // Affiche le message de succès ou d'erreur (incluant le conflit)
                    if (success)
                    {
                        // Rafraîchir la liste après la modification
                        LoadFamily();
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
                // Obtener le bouton cliqué
                var deleteButton = sender as Button;
                if (deleteButton != null)
                {
                    // Obtener l'objet associé à l'élément ListBox à partir du DataContext du bouton
                    var familyName = deleteButton.DataContext as string;
                    if (familyName != null)
                    {
                        // Obtener l'ID de la famille
                        int familyId = await FamilyService.GetFamilyIdByNameAsync(familyName);
                        MessageBoxResult result = MessageBox.Show($"Voulez-vous vraiment supprimer la famille '{familyName}' ?", "Confirmation de suppression", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (result == MessageBoxResult.Yes)
                        {
                            var (Success, Message) = await FamilyService.DeleteFamily(familyId);
                            if (Success)
                            {
                                MessageBox.Show("La famille a été supprimée avec succès!", "Suppression Réussie", MessageBoxButton.OK, MessageBoxImage.Information);
                                LoadFamily();
                            }
                            else
                            {
                                MessageBox.Show(Message, "Suppression Impossible", MessageBoxButton.OK, MessageBoxImage.Error);
                            }                           
                        }
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
