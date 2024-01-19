﻿using System;
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

namespace VinStore.View
{
    /// <summary>
    /// Logique d'interaction pour DetailCommand.xaml
    /// </summary>
    public partial class DetailCommand : UserControl
    {
        public DetailCommand()
        {
            InitializeComponent();
            DataContext = new ProviderOrder();
        }
        public void DetailProviderOrder(ProviderOrder ProviderOrder)
        {
            DataContext = ProviderOrder;

            TotalOrder.Text = $"Total commande: {ProviderOrder.Price}";
        }
    }
}
