using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using MaterialDesignColors;
using VinStore.Services;
using VinStore.View;

namespace VinStore.Pages
{
    /// <summary>
    /// Logique d'interaction pour LogIn.xaml
    /// </summary>
    public partial class LogIn : Page
    {
        MainWindow _context;
        public LogIn(MainWindow context)
        {
            InitializeComponent();
            _context = context;
        }
        private void exitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private bool ValidateFields()
        {
            bool isValid = true;
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                // Afficher un message d'erreur ou prendre d'autres mesures appropriées
                MessageBox.Show("Veuillez saisir le mail.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Veuillez saisir Votre Mot de passe.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                isValid = false;
            }
            if (!Regex.IsMatch(txtUsername.Text, @"^[a-zA-Z0-9]+(?:\.[a-zA-Z0-9]+)*@[a-zA-Z0-9]+(?:\.[a-zA-Z0-9]+)*$"))
            {
                MessageBox.Show("Veuillez saisir une adresse email valide.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                isValid = false;
            }
            return isValid;
        }

        private async void ConnexionClick(object sender, RoutedEventArgs e)
        {
            if (ValidateFields())
            {
                var mail = txtUsername.Text.Trim();
                var mdp = txtPassword.Password.Trim();
                var employee = await EmployeeService.GetEmployeeByMail(mail);
                if (employee != null && BCrypt.Net.BCrypt.Verify(mdp, employee.Password))
                {
                    _context.InitializeComponent();
                    _context.MainFrame.Navigate(new Navbar(_context));
                }
                else
                {
                    MessageBox.Show($"Nom d'utilisateur ou mot de passe incorrect.");
                }
            }
        }
    }

}
