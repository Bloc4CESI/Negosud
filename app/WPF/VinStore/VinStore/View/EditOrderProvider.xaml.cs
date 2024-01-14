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
    /// Logique d'interaction pour EditOrderProvider.xaml
    /// </summary>
    public partial class EditOrderProvider : UserControl
    {
        public EditOrderProvider()
        {
            InitializeComponent();
            DataContext = new ProviderOrder();
        }
        public void UpdateProviderOrder(ProviderOrder ProviderOrder)
        {
            DataContext = ProviderOrder;
      /*      LoadItems(ProviderOrder.Provider!.Id);*/
            TotalOrder.Text = $"Total commande: {ProviderOrder.Price}";
        }
      /*  private async void LoadItems(int ProviderId )
        {
            ProductNameCombox.ItemsSource = await ProductService.GetProductByProvider(ProviderId);
        }*/
    }
}
