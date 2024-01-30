using DevExpress.Data.Helpers;
using DevExpress.Utils.CommonDialogs.Internal;
using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PMLMCustomerClub
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ThemedWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void ProductsButton_Click(object sender, EventArgs e)
        {
            ProductView productView = new ProductView();
            productView.Show();
            this.Close();
        }

        public void CustomersButton_Click(object sender, EventArgs e)
        {
            CustomersView customersView = new CustomersView();
            customersView.Show();
            this.Close();
        }

        public void OrdersButton_Click(object sender, EventArgs e)
        {
            OrdersView ordersView = new OrdersView();
            ordersView.Show();
            this.Close();
        }

        public void StoreButton_Click(object sender, EventArgs e)
        {
            StoreView storeView = new StoreView();
            storeView.Show();
            this.Close();
        }

        public void ExitButton_Click(object sender, EventArgs e)
        {
            string message = "Are you really want to exit the Application?";
            string caption = "Warning";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            
            
            if ( MessageBox.Show(message, caption, buttons, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

    }
}
