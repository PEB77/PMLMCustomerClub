using DevExpress.XtraEditors;
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
    /// Interaction logic for CustomersView.xaml
    /// </summary>
    public partial class CustomersView : Window
    {
        public CustomersView()
        {
            InitializeComponent();
            RecieveDataFromDatabase();
        }
        
        DataTable DataTable1 = new DataTable();
        DataRow DataRowFocused;

        private void RecieveDataFromDatabase()
        {
            DataTable1 = DataBase.GetCustomersDataTable();
            CustomersGridControl.ItemsSource = DataTable1.DefaultView;
            for (int i = 0; i < CustomersGridControl.Columns.Count; i++)
                CustomersGridControl.Columns[i].ReadOnly = true;
        }
        
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void NewCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            int nextCustomerRow = 1;
            if (DataTable1.Rows.Count == 0)
                nextCustomerRow = 1;
            else
            {
                DataRow dataRow = DataTable1.Rows[DataTable1.Rows.Count - 1];
                nextCustomerRow = 1 + int.Parse(dataRow[0].ToString());
            }
            NewCustomerWindow newCustomerWindow = new NewCustomerWindow(nextCustomerRow);
            this.ShowActivated = false;
            newCustomerWindow.Show();
        }

        private void EditCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataTable1.Rows.Count == 0)
            {
                XtraMessageBox.Show("There is no Data in The Table");
                return;
            }
            if (DataRowFocused == null)
                DataRowFocused = DataTable1.Rows[0];

            this.ShowActivated = false;
            NewCustomerWindow newCustomerWindow = new NewCustomerWindow(DataRowFocused);
            newCustomerWindow.Show();
        }

        private void DeleteCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataRowFocused == null) return;
            string folderName = DataRowFocused[7].ToString();
            int customerID = int.Parse(DataRowFocused[0].ToString());
            FileManagment.DeleteCustomerFolder(folderName);
            DataBase.DeleteCustomerProfile(customerID);
            RecieveDataFromDatabase();
        }

        private void CustomersView_Activated(object sender, EventArgs e)
        {
            RecieveDataFromDatabase();
            DataRowFocused = null;
        }

        private void CustomerTable_CanSelectRow(object sender, DevExpress.Xpf.Grid.CanSelectRowEventArgs e)
        {
            DataRowView row = (DataRowView)e.Row;
            if (row == null) return;
            DataRowFocused = row.Row;
        }
    }
}
