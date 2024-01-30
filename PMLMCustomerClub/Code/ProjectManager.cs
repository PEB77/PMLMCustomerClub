using PMLMCustomerClub.View;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PMLMCustomerClub.Code
{
    public class ProjectManager
    {

        public enum SelectPart
        {
            CUSTOMER,
            ORDER,
            STORE,
            PRODUCT
        }
        public SelectPart Part = SelectPart.CUSTOMER;
        public Dictionary<SelectPart, DataTable> AllDataTables = new Dictionary<SelectPart, DataTable>();
        public MainPage Main;

        internal ProductManager ProductManager;
        internal StoreManager StoreManager;
        internal CustomerManager CustomerManager;
        internal OrderManager OrderManager;

        public delegate void ChangeProductItemSourceEventHandler(object itemSource);
        public delegate void ChangeStoreItemSourceEventHandler(object itemSource);
        public delegate void ChangeCustomerSourceEventHandler(object itemSource);
        public delegate void ChangeOrderSourceEventHandler(object itemSource);

        public ChangeProductItemSourceEventHandler ChangeProductItemSource;
        public ChangeStoreItemSourceEventHandler ChangeStoreItemSource;
        public ChangeCustomerSourceEventHandler ChangeCustomerSource;
        public ChangeOrderSourceEventHandler ChangeOrderSource;

        private void SetProductItemSource(object itemSource)
        {
            Main.ProductTableViewer.GridControlProp.BeginDataUpdate();
            Main.ProductTableViewer.GridControlProp.ItemsSource = itemSource;
            Main.ProductTableViewer.GridControlProp.RefreshData();
            Main.ProductTableViewer.GridControlProp.EndDataUpdate();
        }
        private void SetStoreItemSource(object itemSource)
        {
            Main.StoreTableViewer.GridControlProp.BeginDataUpdate();
            Main.StoreTableViewer.GridControlProp.ItemsSource = itemSource;
            Main.StoreTableViewer.GridControlProp.RefreshData();
            Main.StoreTableViewer.GridControlProp.EndDataUpdate();
        }
        private void SetCustomerSource(object itemSource)
        {
            Main.CustomerTableViewer.GridControlProp.BeginDataUpdate();
            Main.CustomerTableViewer.GridControlProp.ItemsSource = itemSource;
            Main.CustomerTableViewer.GridControlProp.RefreshData();
            Main.CustomerTableViewer.GridControlProp.EndDataUpdate();
        }
        private void SetOrderSource(object itemSource)
        {
            Main.OrderTableViewer.GridControlProp.BeginDataUpdate();
            Main.OrderTableViewer.GridControlProp.ItemsSource = itemSource;
            Main.OrderTableViewer.GridControlProp.RefreshData();
            Main.OrderTableViewer.GridControlProp.EndDataUpdate();

        }

        public Task LoadDatabases()
        {
            Task task = Task.Run(() =>
            {
                AllDataTables.Clear();

                DataTable productTable = ProductDatabase.GetDataTable();
                AllDataTables.Add(SelectPart.PRODUCT, productTable);

                DataTable storeTable = StoreDatabase.GetDataTable();
                AllDataTables.Add(SelectPart.STORE, storeTable);

                DataTable customerTable = CustomerDatabase.GetDataTable();
                AllDataTables.Add(SelectPart.CUSTOMER, customerTable);

                DataTable orderTable = OrderDatabase.GetDataTable();
                AllDataTables.Add(SelectPart.ORDER, orderTable);
            });

            return task;
        }
        public Task LoadDatabase()
        {
            Task task = Task.Run(() =>
            {
                switch (Part)
                {
                    case SelectPart.CUSTOMER:
                        AllDataTables[Part] = CustomerDatabase.GetDataTable();
                        Main.UIDispatcher.BeginInvoke(ChangeCustomerSource, AllDataTables[Part]);
                        break;
                    case SelectPart.ORDER:
                        AllDataTables[Part] = OrderDatabase.GetDataTable();
                        AllDataTables[SelectPart.CUSTOMER] = CustomerDatabase.GetDataTable();
                        AllDataTables[SelectPart.STORE] = StoreDatabase.GetDataTable();
                        Main.UIDispatcher.BeginInvoke(ChangeOrderSource, AllDataTables[Part]);
                        Main.UIDispatcher.BeginInvoke(ChangeCustomerSource, AllDataTables[SelectPart.CUSTOMER]);
                        Main.UIDispatcher.BeginInvoke(ChangeStoreItemSource, AllDataTables[SelectPart.STORE]);
                        break;
                    case SelectPart.STORE:
                        AllDataTables[Part] = StoreDatabase.GetDataTable();
                        Main.UIDispatcher.BeginInvoke(ChangeStoreItemSource, AllDataTables[Part]);
                        break;
                    default:
                        AllDataTables[Part] = ProductDatabase.GetDataTable();
                        Main.UIDispatcher.BeginInvoke(ChangeProductItemSource, AllDataTables[Part]);
                        break;
                }
            });
            return task;
        }

        public void FirstInit()
        {
            Task loading = LoadDatabases();
            loading.Wait();

            Main.CustomerTableViewer.GridControlProp.ItemsSource = AllDataTables[SelectPart.CUSTOMER];
            Main.OrderTableViewer.GridControlProp.ItemsSource = AllDataTables[SelectPart.ORDER];
            Main.StoreTableViewer.GridControlProp.ItemsSource = AllDataTables[SelectPart.STORE];
            Main.ProductTableViewer.GridControlProp.ItemsSource = AllDataTables[SelectPart.PRODUCT];

            switch (Part)
            {
                case SelectPart.PRODUCT: Main.MainTabControl.SelectedIndex = 3; break;
                case SelectPart.STORE: Main.MainTabControl.SelectedIndex = 2; break;
                case SelectPart.CUSTOMER: Main.MainTabControl.SelectedIndex = 0; break;
                case SelectPart.ORDER: Main.MainTabControl.SelectedIndex = 1; break;
            }

            Main.SelectionTab += Main_SelectionTab;
            Main.ExportStore += Main_ExportStore;
            ChangeProductItemSource = new ChangeProductItemSourceEventHandler(SetProductItemSource);
            ChangeStoreItemSource = new ChangeStoreItemSourceEventHandler(SetStoreItemSource);
            ChangeCustomerSource = new ChangeCustomerSourceEventHandler(SetCustomerSource);
            ChangeOrderSource = new ChangeOrderSourceEventHandler(SetOrderSource);

            ProductManager = new ProductManager(this, Main.ProductTableViewer);
            StoreManager = new StoreManager(this, Main.StoreTableViewer);
            CustomerManager = new CustomerManager(this, Main.CustomerTableViewer);
            OrderManager = new OrderManager(this, Main.OrderTableViewer);
        }

        private void Main_ExportStore(object sender, System.Windows.RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                StoreDatabase.SaveCsv(folderBrowser.SelectedPath);
            }
            
        }

        private void Main_SelectionTab(object sender, DevExpress.Xpf.Core.TabControlSelectionChangedEventArgs e)
        {
            Part = GetCurrentPart(Main.MainTabControl.SelectedIndex);
        }

        private SelectPart GetCurrentPart(int index)
        {
            switch (index)
            {
                case 0: return SelectPart.CUSTOMER;
                case 1: return SelectPart.ORDER;
                case 2: return SelectPart.STORE;
                default: return SelectPart.PRODUCT;
            }
        }

    }
}
