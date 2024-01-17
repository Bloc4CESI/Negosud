using System;
using System.Collections.Generic;
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
using ApiNegosud.Models;
using VinStore.Services;

namespace VinStore.View
{
    /// <summary>
    /// Logique d'interaction pour Navbar.xaml
    /// </summary>
    public partial class Navbar : UserControl
    {
        public Navbar()
        {
            InitializeComponent();
            LoadOrderProvierToValidate();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        { 
            GridMain.Children.Clear();
            GridMain.Children.Add(new AddEmployee());
        }
        private void AddProviderOrder(object sender, RoutedEventArgs e)
        {
            GridMain.Children.Clear();
            GridMain.Children.Add(new AddCommandProvider());
        }
        private void ListProduct_Click(object sender, RoutedEventArgs e)
        {
            GridMain.Children.Clear();
            GridMain.Children.Add(new ListProduct());
        }
        private void AddFamille_Click(object sender, RoutedEventArgs e)
        {
            GridMain.Children.Clear();
            GridMain.Children.Add(new AddFamily());
        }

        private void Stock_Click(object sender, RoutedEventArgs e)
        {
            GridMain.Children.Clear();
            GridMain.Children.Add(new StockView());
        }

        private void Provider_Click(object sender, RoutedEventArgs e)
        {
            GridMain.Children.Clear();
            GridMain.Children.Add(new ProviderView());
        }

        private void CommandToValidate(object sender, RoutedEventArgs e)
        {
            GridMain.Children.Clear();
            GridMain.Children.Add(new OrderProviderToValidate(GridMain));
        }

        private async void LoadOrderProvierToValidate()
        {
            var ProviderOrdersToValidate = await CommandProviderService.GetProviderOrderByStatus(ProviderOrderStatus.ENCOURSDEVALIDATION);
            int count = ProviderOrdersToValidate.Count;
            TextBlock textBlock = new TextBlock();
            textBlock.Inlines.Add(new Run($"Commande à valider ("));
            textBlock.Inlines.Add(new Run($"{count}") { Foreground = Brushes.Red });
            textBlock.Inlines.Add(new Run(")"));
            CommandToValidateHeader.Header = textBlock;

        }
    }
}
