﻿#pragma checksum "..\..\..\..\View\Navbar.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "02CD275327E83798EFB8C0565844C33E85059270"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using VinStore.View;


namespace VinStore.View {
    
    
    /// <summary>
    /// Navbar
    /// </summary>
    public partial class Navbar : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 71 "..\..\..\..\View\Navbar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem CommandToValidateHeader;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\..\..\View\Navbar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem CommandToDeliverHeader;
        
        #line default
        #line hidden
        
        
        #line 73 "..\..\..\..\View\Navbar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem DeliveredCommandHeader;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\..\..\View\Navbar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem RefusedCommandHeader;
        
        #line default
        #line hidden
        
        
        #line 84 "..\..\..\..\View\Navbar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem InventoryToValidateHeader;
        
        #line default
        #line hidden
        
        
        #line 85 "..\..\..\..\View\Navbar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem ValidatedInventoriesHeader;
        
        #line default
        #line hidden
        
        
        #line 86 "..\..\..\..\View\Navbar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem RefusedInventoriesHeader;
        
        #line default
        #line hidden
        
        
        #line 95 "..\..\..\..\View\Navbar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem CommandClientToDeliverHeader;
        
        #line default
        #line hidden
        
        
        #line 96 "..\..\..\..\View\Navbar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem DeliveredClientCommandHeader;
        
        #line default
        #line hidden
        
        
        #line 97 "..\..\..\..\View\Navbar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem RefusedClientCommandHeader;
        
        #line default
        #line hidden
        
        
        #line 109 "..\..\..\..\View\Navbar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid GridMain;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.1.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/VinStore;V1.0.0.0;component/view/navbar.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\View\Navbar.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.1.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 21 "..\..\..\..\View\Navbar.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.HomePageClick);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 33 "..\..\..\..\View\Navbar.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.AddFamille_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 42 "..\..\..\..\View\Navbar.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.AddProduct_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 43 "..\..\..\..\View\Navbar.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.ListProduct_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 52 "..\..\..\..\View\Navbar.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Stock_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 61 "..\..\..\..\View\Navbar.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Provider_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 63 "..\..\..\..\View\Navbar.xaml"
            ((System.Windows.Controls.MenuItem)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.CommandProviderMenuItem_MouseEnter);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 70 "..\..\..\..\View\Navbar.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.AddProviderOrder);
            
            #line default
            #line hidden
            return;
            case 9:
            this.CommandToValidateHeader = ((System.Windows.Controls.MenuItem)(target));
            
            #line 71 "..\..\..\..\View\Navbar.xaml"
            this.CommandToValidateHeader.Click += new System.Windows.RoutedEventHandler(this.CommandToValidate);
            
            #line default
            #line hidden
            return;
            case 10:
            this.CommandToDeliverHeader = ((System.Windows.Controls.MenuItem)(target));
            
            #line 72 "..\..\..\..\View\Navbar.xaml"
            this.CommandToDeliverHeader.Click += new System.Windows.RoutedEventHandler(this.CommandToDeliver);
            
            #line default
            #line hidden
            return;
            case 11:
            this.DeliveredCommandHeader = ((System.Windows.Controls.MenuItem)(target));
            
            #line 73 "..\..\..\..\View\Navbar.xaml"
            this.DeliveredCommandHeader.Click += new System.Windows.RoutedEventHandler(this.DeliveredCommand);
            
            #line default
            #line hidden
            return;
            case 12:
            this.RefusedCommandHeader = ((System.Windows.Controls.MenuItem)(target));
            
            #line 74 "..\..\..\..\View\Navbar.xaml"
            this.RefusedCommandHeader.Click += new System.Windows.RoutedEventHandler(this.RefusedCommand);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 76 "..\..\..\..\View\Navbar.xaml"
            ((System.Windows.Controls.MenuItem)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.InventoryMenuItem_MouseEnter);
            
            #line default
            #line hidden
            return;
            case 14:
            
            #line 83 "..\..\..\..\View\Navbar.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.CreateInventory);
            
            #line default
            #line hidden
            return;
            case 15:
            this.InventoryToValidateHeader = ((System.Windows.Controls.MenuItem)(target));
            
            #line 84 "..\..\..\..\View\Navbar.xaml"
            this.InventoryToValidateHeader.Click += new System.Windows.RoutedEventHandler(this.InventoryToValidate);
            
            #line default
            #line hidden
            return;
            case 16:
            this.ValidatedInventoriesHeader = ((System.Windows.Controls.MenuItem)(target));
            
            #line 85 "..\..\..\..\View\Navbar.xaml"
            this.ValidatedInventoriesHeader.Click += new System.Windows.RoutedEventHandler(this.ValidatedInventories);
            
            #line default
            #line hidden
            return;
            case 17:
            this.RefusedInventoriesHeader = ((System.Windows.Controls.MenuItem)(target));
            
            #line 86 "..\..\..\..\View\Navbar.xaml"
            this.RefusedInventoriesHeader.Click += new System.Windows.RoutedEventHandler(this.RefusedInventories);
            
            #line default
            #line hidden
            return;
            case 18:
            
            #line 88 "..\..\..\..\View\Navbar.xaml"
            ((System.Windows.Controls.MenuItem)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.CommandClientMenuItem_MouseEnter);
            
            #line default
            #line hidden
            return;
            case 19:
            this.CommandClientToDeliverHeader = ((System.Windows.Controls.MenuItem)(target));
            
            #line 95 "..\..\..\..\View\Navbar.xaml"
            this.CommandClientToDeliverHeader.Click += new System.Windows.RoutedEventHandler(this.CommandClientToDeliver);
            
            #line default
            #line hidden
            return;
            case 20:
            this.DeliveredClientCommandHeader = ((System.Windows.Controls.MenuItem)(target));
            
            #line 96 "..\..\..\..\View\Navbar.xaml"
            this.DeliveredClientCommandHeader.Click += new System.Windows.RoutedEventHandler(this.DeliveredClientCommand);
            
            #line default
            #line hidden
            return;
            case 21:
            this.RefusedClientCommandHeader = ((System.Windows.Controls.MenuItem)(target));
            
            #line 97 "..\..\..\..\View\Navbar.xaml"
            this.RefusedClientCommandHeader.Click += new System.Windows.RoutedEventHandler(this.RefusedClientCommand);
            
            #line default
            #line hidden
            return;
            case 22:
            
            #line 99 "..\..\..\..\View\Navbar.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.DeconnxionClick);
            
            #line default
            #line hidden
            return;
            case 23:
            this.GridMain = ((System.Windows.Controls.Grid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

