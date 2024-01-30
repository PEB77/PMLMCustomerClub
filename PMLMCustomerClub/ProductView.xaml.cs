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
using DevExpress.Xpf.Editors.Helpers;
using DevExpress.Xpf.Grid;
using MySql.Data.MySqlClient;

namespace PMLMCustomerClub
{
    /// <summary>
    /// Interaction logic for ProductView.xaml
    /// </summary>
    public partial class ProductView : Window
    {
        public ProductView()
        {
            InitializeComponent();
            RecieveDataFromDatabase();

        }

        DataTable DataTable1;
        DataRow DataRowFocused;

        private void RecieveDataFromDatabase()
        {
            DataTable1 = DataBase.GetProductsDataTable();
            ProductGridControl.ItemsSource = DataTable1.DefaultView;

            for (int i = 0; i < ProductGridControl.Columns.Count; i++)
            {
                ProductGridControl.Columns[i].ReadOnly = true;
            }
            
        }
        
        private void ProductTable_CanSelectRow(object sender, CanSelectRowEventArgs e)
        {
            DataRowView row = (DataRowView)e.Row;
            if (row == null) return;
            DataRowFocused = row.Row;
            
        }

        private void NewProductButton_Click(object sender, EventArgs e)
        {
            this.ShowActivated = false;
            NewProductWindow newProductWindow = new NewProductWindow(DataTable1.Rows.Count, this);
            newProductWindow.Show();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void EditProductButton_Click(object sender, EventArgs e)
        {
            if (DataRowFocused == null) DataRowFocused = DataTable1.Rows[0];
            this.ShowActivated = false;
            NewProductWindow newProductWindow = new NewProductWindow(DataRowFocused, this);
            newProductWindow.Show();
        }

        private void DeleteProductButton_Click(object sender, EventArgs e)
        {
            if (DataTable1.Rows.Count == 0)
            {
                MessageBox.Show("There is no data.");
                return;
            }
            if (DataRowFocused == null) DataRowFocused = DataTable1.Rows[0];
            DataBase.DeleteProductRow(int.Parse(DataRowFocused[0].ToString()));
            RecieveDataFromDatabase();
        }

        private void ProductView_Activated(object sender, EventArgs e)
        {
            RecieveDataFromDatabase();
        }
    }
}
