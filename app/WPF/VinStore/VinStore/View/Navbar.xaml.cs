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
using VinStore.Pages;
using VinStore.Services;
using static ApiNegosud.Models.Inventory;

namespace VinStore.View
{
    /// <summary>
    /// Logique d'interaction pour Navbar.xaml
    /// </summary>
    public partial class Navbar : UserControl
    {
        MainWindow _context;
        public Navbar(MainWindow context)
        {
            InitializeComponent();
            LoadOrderProvierToValidate();
            CommandToDeliver();
            LoadDeliveredCommand();
            LoadRefusedCommand();
            LoadRefusedInventories();
            LoadValidatedInventories();
            LoadInventoryToValidate();
            LoadRefusedCommandClient();
            LoadValidatedCommandClient();
            LoadDelivredCommandClient();
            GridMain.Children.Add(new Home(GridMain));
            _context = context;
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

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            GridMain.Children.Clear();
            GridMain.Children.Add(new AddProduct());
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
            _context.MainFrame.Navigate(new LogIn(_context));
        }
        private void HomePageClick(object sender, RoutedEventArgs e)
        {
            GridMain.Children.Clear();
            GridMain.Children.Add(new Home(GridMain));
        }
        private void CommandClientToDeliver(object sender, RoutedEventArgs e)
        {
            GridMain.Children.Clear();
            GridMain.Children.Add(new CommandClientToDelivrey(GridMain));
        }
        private void DeliveredClientCommand(object sender, RoutedEventArgs e)
        {
            GridMain.Children.Clear();
            GridMain.Children.Add(new LivredCommandClient(GridMain));
        }
        private void RefusedClientCommand(object sender, RoutedEventArgs e)
        {
            GridMain.Children.Clear();
            GridMain.Children.Add(new RefusedCommandClient(GridMain));
        }

        private async void LoadOrderProvierToValidate()
        {
            var providerOrdersToValidate = await CommandProviderService.GetProviderOrderByStatus(ProviderOrderStatus.ENCOURSDEVALIDATION);
            int count = providerOrdersToValidate.Count;
            TextBlock textBlock = new TextBlock();
            textBlock.Inlines.Add(new Run("Commande à valider ("));
            textBlock.Inlines.Add(new Run($"{count}") { Foreground = Brushes.Red });
            textBlock.Inlines.Add(new Run(")"));
            CommandToValidateHeader.Header = textBlock;
        }
        private async void CommandToDeliver()
        {
            var validatedProviderOrder = await CommandProviderService.GetProviderOrderByStatus(ProviderOrderStatus.VALIDE);
            int count = validatedProviderOrder.Count;
            TextBlock textBlock = new TextBlock();
            textBlock.Inlines.Add(new Run($"Commande à livrer ("));
            textBlock.Inlines.Add(new Run($"{count}") { Foreground = Brushes.Red });
            textBlock.Inlines.Add(new Run(")"));
            CommandToDeliverHeader.Header = textBlock;
        }
        private async void LoadDeliveredCommand()
        {
            var providerOrdersToPay = await CommandProviderService.GetProviderOrderByStatus(ProviderOrderStatus.LIVRE);
            int count = providerOrdersToPay.Count;
            TextBlock textBlock = new TextBlock();
            textBlock.Inlines.Add(new Run($"Commandes livrées ("));
            textBlock.Inlines.Add(new Run($"{count}") { Foreground = Brushes.Red });
            textBlock.Inlines.Add(new Run(")"));
            DeliveredCommandHeader.Header = textBlock;
        }
        private async void LoadRefusedCommand()
        {
            var refusedProviderOrders = await CommandProviderService.GetProviderOrderByStatus(ProviderOrderStatus.REFUSE);
            int count = refusedProviderOrders.Count;
            TextBlock textBlock = new TextBlock();
            textBlock.Inlines.Add(new Run($"Commandes refusées ("));
            textBlock.Inlines.Add(new Run($"{count}") { Foreground = Brushes.Red });
            textBlock.Inlines.Add(new Run(")"));
            RefusedCommandHeader.Header = textBlock;
        }
        private void CommandProviderMenuItem_MouseEnter(object sender, MouseEventArgs e)
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
        private void InventoryMenuItem_MouseEnter(object sender, MouseEventArgs e)
        {
            LoadRefusedInventories();
            LoadValidatedInventories();
            LoadInventoryToValidate();
        }
        private async void LoadRefusedCommandClient()
        {
            var RefusedCommandClients = await CommandClientService.GetClientOrderByStatus(OrderStatus.REFUSE);
            int count = RefusedCommandClients.Count;
            TextBlock textBlock = new TextBlock();
            textBlock.Inlines.Add(new Run($"Commandes Clients refusées ("));
            textBlock.Inlines.Add(new Run($"{count}") { Foreground = Brushes.Red });
            textBlock.Inlines.Add(new Run(")"));
            RefusedClientCommandHeader.Header = textBlock;
        }
        private async void LoadValidatedCommandClient()
        {
            var RefusedCommandClients = await CommandClientService.GetClientOrderByStatus(OrderStatus.VALIDE);
            int count = RefusedCommandClients.Count;
            TextBlock textBlock = new TextBlock();
            textBlock.Inlines.Add(new Run($"Commandes Clients à livrer ("));
            textBlock.Inlines.Add(new Run($"{count}") { Foreground = Brushes.Red });
            textBlock.Inlines.Add(new Run(")"));
            CommandClientToDeliverHeader.Header = textBlock;
        }
        private async void LoadDelivredCommandClient()
        {
            var RefusedCommandClients = await CommandClientService.GetClientOrderByStatus(OrderStatus.LIVRE);
            int count = RefusedCommandClients.Count;
            TextBlock textBlock = new TextBlock();
            textBlock.Inlines.Add(new Run($"Commandes Clients Livrées ("));
            textBlock.Inlines.Add(new Run($"{count}") { Foreground = Brushes.Red });
            textBlock.Inlines.Add(new Run(")"));
            DeliveredClientCommandHeader.Header = textBlock;
        }
        private void CommandClientMenuItem_MouseEnter(object sender, MouseEventArgs e)
        {
            LoadDelivredCommandClient();
            LoadValidatedCommandClient();
            LoadRefusedCommandClient();
        }
    }
}
