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
    /// Interaction logic for StoreView.xaml
    /// </summary>
    public partial class StoreView : Window
    {
        public StoreView()
        {
            InitializeComponent();
            RecieveDataFromDatabase();
        }

        DataTable DataTable1 = new DataTable();
        DataRow DataRowFocused;

        public void RecieveDataFromDatabase()
        {
            DataTable1 = DataBase.GetStoreDataTable();
            StoreItemGridControl.ItemsSource = DataTable1.DefaultView;
            for (int i = 0; i < StoreItemGridControl.Columns.Count; i++)
            {
                StoreItemGridControl.Columns[i].ReadOnly = true;
            }
        }

        public void NewStoreItemButton_Click(object sender, EventArgs e)
        {
            int nextRowID = 0;
            if (DataTable1.Rows.Count == 0) nextRowID = 0;
            else
            {
                DataRow dataRow = DataTable1.Rows[DataTable1.Rows.Count - 1];
                nextRowID = int.Parse(dataRow[0].ToString());
            }
            this.ShowActivated = false;
            NewStoreItemWindow newStoreItemWindow = new NewStoreItemWindow(nextRowID);
            newStoreItemWindow.Show();
            
        }

        private void StoreView_Activated(object sender, EventArgs e)
        {
            RecieveDataFromDatabase();
            DataRowFocused = null;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void StoreTable_CanSelectRow(object sender, DevExpress.Xpf.Grid.CanSelectRowEventArgs e)
        {
            DataRowView row = (DataRowView)e.Row;
            if (row == null) return;
            DataRowFocused = row.Row;
        }

        private void EditStoreItemButtom_Click(object sender, RoutedEventArgs e)
        {
            if(DataTable1.Rows.Count == 0)
            {
                XtraMessageBox.Show("There is no Data in The Table");
                return;
            }
            if (DataRowFocused == null)
                DataRowFocused = DataTable1.Rows[0];

            this.ShowActivated = false;
            NewStoreItemWindow newStoreItemWindow = new NewStoreItemWindow(DataRowFocused);
            newStoreItemWindow.Show();
            
        }

        private void DeleteStoreItemButtom_Click(object sender, RoutedEventArgs e)
        {
            if (DataTable1.Rows.Count == 0)
            {
                XtraMessageBox.Show("There is no Data in The Data Table");
                return;
            }
            if (DataRowFocused != null)
                DataBase.DeleteStoreRow(int.Parse(DataRowFocused[0].ToString()));
            else
                XtraMessageBox.Show("First select a row");

            RecieveDataFromDatabase();
        }
    }
}
