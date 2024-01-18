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
using static ApiNegosud.Models.Inventory;

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
            CommandToDeliver();
            LoadDeliveredCommand();
            LoadRefusedCommand();
            LoadRefusedInventories();
            LoadValidatedInventories();
            LoadInventoryToValidate();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        { 
            GridMain.Children.Clear();
            GridMain.Children.Add(new AddEmployee());
        }
        private void AddProviderOrder(object sender, RoutedEventArgs e)
        {
            GridMain.Children.Clear();
            GridMain.Children.Add(new AddCommandProvider(GridMain));
        }
        private void ListProduct_Click(object sender, RoutedEventArgs e)
        {
            GridMain.Children.Clear();
            GridMain.Children.Add(new ListProduct());
        }
        private void CreateInventory(object sender, RoutedEventArgs e)
        {
            GridMain.Children.Clear();
            GridMain.Children.Add(new CreateInventory(GridMain));
        }
        
        private void AddFamille_Click(object sender, RoutedEventArgs e)
        {
            GridMain.Children.Clear();
            GridMain.Children.Add(new AddFamily());
        }
        private void CommandToValidate(object sender, RoutedEventArgs e)
        {
            GridMain.Children.Clear();
            GridMain.Children.Add(new OrderProviderToValidate(GridMain));
        }
        private void CommandToDeliver(object sender, RoutedEventArgs e)
        {
            GridMain.Children.Clear();
            GridMain.Children.Add(new ProviderOrderToDelivery(GridMain));
        }
        private void DeliveredCommand(object sender, RoutedEventArgs e)
        {
            GridMain.Children.Clear();
            GridMain.Children.Add(new DelivredCommand(GridMain));
        }
        private void RefusedCommand(object sender, RoutedEventArgs e)
        {
            GridMain.Children.Clear();
            GridMain.Children.Add(new RefusedOrderProvider(GridMain));
        }
        private void InventoryToValidate(object sender, RoutedEventArgs e)
        {
            GridMain.Children.Clear();
            GridMain.Children.Add(new InventoryToValidate(GridMain));
        }
        private void ValidatedInventories(object sender, RoutedEventArgs e)
        {
            GridMain.Children.Clear();
            GridMain.Children.Add(new ValidateInventory(GridMain));
        }
        private void RefusedInventories(object sender, RoutedEventArgs e)
        {
            GridMain.Children.Clear();
            GridMain.Children.Add(new RefusedInventory(GridMain));
        }
        private void DeconnxionClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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
        private async void CommandToDeliver()
        {
            var ValidatedProviderOrder = await CommandProviderService.GetProviderOrderByStatus(ProviderOrderStatus.VALIDE);
            int count = ValidatedProviderOrder.Count;
            TextBlock textBlock = new TextBlock();
            textBlock.Inlines.Add(new Run($"Commande à livrer ("));
            textBlock.Inlines.Add(new Run($"{count}") { Foreground = Brushes.Red });
            textBlock.Inlines.Add(new Run(")"));
            CommandToDeliverHeader.Header = textBlock;
        }
        private async void LoadDeliveredCommand()
        {
            var ProviderOrdersToPay = await CommandProviderService.GetProviderOrderByStatus(ProviderOrderStatus.LIVRE);
            int count = ProviderOrdersToPay.Count;
            TextBlock textBlock = new TextBlock();
            textBlock.Inlines.Add(new Run($"Commandes livrées ("));
            textBlock.Inlines.Add(new Run($"{count}") { Foreground = Brushes.Red });
            textBlock.Inlines.Add(new Run(")"));
            DeliveredCommandHeader.Header = textBlock;
        }
        private async void LoadRefusedCommand()
        {
            var RefusedProviderOrders = await CommandProviderService.GetProviderOrderByStatus(ProviderOrderStatus.REFUSE);
            int count = RefusedProviderOrders.Count;
            TextBlock textBlock = new TextBlock();
            textBlock.Inlines.Add(new Run($"Commandes refusées ("));
            textBlock.Inlines.Add(new Run($"{count}") { Foreground = Brushes.Red });
            textBlock.Inlines.Add(new Run(")"));
            RefusedCommandHeader.Header = textBlock;
        }
        private async void CommandeProviderMenuItem_Click(object sender, RoutedEventArgs e)
        {
            LoadOrderProvierToValidate();
            CommandToDeliver();
            LoadDeliveredCommand();
            LoadRefusedCommand();
        }
        private async void LoadInventoryToValidate()
        {
            var InventoriesToValidate = await InventoryService.GetInventoriesByStatus(InventoryEnum.ENCOURSDEVALIDATION);
            int count = InventoriesToValidate.Count;
            TextBlock textBlock = new TextBlock();
            textBlock.Inlines.Add(new Run($"Inventaire à valider ("));
            textBlock.Inlines.Add(new Run($"{count}") { Foreground = Brushes.Red });
            textBlock.Inlines.Add(new Run(")"));
            InventoryToValidateHeader.Header = textBlock;
        }
        private async void LoadValidatedInventories()
        {
            var ValidatedInventories = await InventoryService.GetInventoriesByStatus(InventoryEnum.VALIDE);
            int count = ValidatedInventories.Count;
            TextBlock textBlock = new TextBlock();
            textBlock.Inlines.Add(new Run($"Inventaire validé ("));
            textBlock.Inlines.Add(new Run($"{count}") { Foreground = Brushes.Red });
            textBlock.Inlines.Add(new Run(")"));
            ValidatedInventoriesHeader.Header = textBlock;
        }
        private async void LoadRefusedInventories()
        {
            var RefusedInventories = await InventoryService.GetInventoriesByStatus(InventoryEnum.REFUSE);
            int count = RefusedInventories.Count;
            TextBlock textBlock = new TextBlock();
            textBlock.Inlines.Add(new Run($"Inventaire refusé ("));
            textBlock.Inlines.Add(new Run($"{count}") { Foreground = Brushes.Red });
            textBlock.Inlines.Add(new Run(")"));
            RefusedInventoriesHeader.Header = textBlock;
        }
        private async void InventoryMenuItem_Click(object sender, RoutedEventArgs e)
        {
            LoadRefusedInventories();
            LoadValidatedInventories();
            LoadInventoryToValidate();
        }
    }
}
