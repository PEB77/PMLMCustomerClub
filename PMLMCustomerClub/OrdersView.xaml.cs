using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;


namespace PMLMCustomerClub
{
    /// <summary>
    /// Interaction logic for OrdersView.xaml
    /// </summary>
    public partial class OrdersView : Window
    {
        public OrdersView()
        {
            InitializeComponent();
            RecieveOrdersDataTable();
        }

        DataTable DataTable1 = new DataTable();
        DataRow DataRowFocused;

        private void RecieveOrdersDataTable()
        {
            DataTable1 = DataBase.GetOrdersDataTable();
            OrdersGridControl.ItemsSource = DataTable1.DefaultView;
            for (int i = 0; i < OrdersGridControl.Columns.Count; i++)
                OrdersGridControl.Columns[i].ReadOnly = true;
        }

        private void OrdersTable_CanSelectRow(object sender, DevExpress.Xpf.Grid.CanSelectRowEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)e.Row;
            if (dataRowView == null) return;
            DataRowFocused = dataRowView.Row;
            DeleteOrdersButton.IsEnabled = true;
            EditOrdersButton.IsEnabled = true;
        }

        private void NewOrderButton_Click(object sender, RoutedEventArgs e)
        {
            int nextID = 1;
            if (DataTable1.Rows.Count != 0)
                nextID = DataTable1.Rows.Count + 1;
            NewOrderWindow newOrderWindow = new NewOrderWindow(nextID);
            newOrderWindow.Show();
            this.ShowActivated = false;
        }

        private void EditOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            int customerID = int.Parse(DataRowFocused[2].ToString());
            string orderFileName = DataRowFocused[4].ToString();
            DataBase.SearchCustomerProfile(customerID, out CustomerProfile customer);
            OrdersInfo order = FileManagment.ReadOrdersInfo(customer, orderFileName);
            DataBase.DeleteOrder(order, false);
            FileManagment.DeleteOrderFile(customer, orderFileName);

            NewOrderWindow newOrderWindow = new NewOrderWindow(order);
            newOrderWindow.Show();
            this.ShowActivated = false;
            DeleteOrdersButton.IsEnabled = false;
            EditOrdersButton.IsEnabled = false;
        }

        private void DeleteOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            int customerID = int.Parse(DataRowFocused[2].ToString());
            string orderFileName = DataRowFocused[4].ToString();
            DataBase.SearchCustomerProfile(customerID, out CustomerProfile customer);
            OrdersInfo order = FileManagment.ReadOrdersInfo(customer, orderFileName);
            DataBase.DeleteOrder(order);
            FileManagment.DeleteOrderFile(customer, orderFileName);
            DeleteOrdersButton.IsEnabled = false;
            EditOrdersButton.IsEnabled = false;
            RecieveOrdersDataTable();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void OrdersWindows_Activated(object sender, EventArgs e)
        {
            RecieveOrdersDataTable();
        }

    }
}
