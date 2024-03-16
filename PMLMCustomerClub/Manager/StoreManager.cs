using DevExpress.Accessibility;
using DevExpress.Xpf.Grid;
using PMLMCustomerClub.CustomControls;
using PMLMCustomerClub.View;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PMLMCustomerClub.Model;

namespace PMLMCustomerClub.Manager
{
    public class StoreManager : IManager
    {
        public StoreManager(ProjectManager manager, TableViewer viewer)
        {
            Manager = manager;
            Viewer = viewer;

            Viewer.NewButtonClick += Viewer_NewButtonClick;
            Viewer.EditButtonClick += Viewer_EditButtonClick;
            Viewer.DeleteButtonClick += Viewer_DeleteButtonClick;
            Viewer.TableSelectingRow += Viewer_TableSelectingRow;
        }

        public ProjectManager Manager { get; set; }
        public TableViewer Viewer { get; set; }
        public DataRow RowFocused { get; set; }
        public bool IsEditMode { get; set; }

        public StoreItem StoreItem;
        public StorePage Page;

        Dictionary<string, StoreItem.Categories> NameCategories;
        Dictionary<string, StoreItem.Brands> NameBrands;
        Dictionary<string, int> NamePrice;
        Dictionary<string, int> NameProductID;

        public void AcceptValidation()
        {
            if (StoreItem.Validation())
                Page.AcceptButton.IsEnabled = true;
            else
                Page.AcceptButton.IsEnabled = false;
        }

        public void InitComponent()
        {
            Page.IDBox.Text = StoreItem.StoreID.ToString();
            Page.ProductNameComboBox.Text = StoreItem.ProductName;
            Page.ProductCategoryComboBox.Text = StoreItem.Category.ToString().Replace("_", " ");
            Page.ProductBrandComboBox.Text = StoreItem.Brand.ToString().Replace("_", " ");
            Page.ProductPriceSpinBox.Text = StoreItem.Price.ToString();
            SetNameComboBox();
            Page.ExpCalender.SetDate(StoreItem.ExpDate);
            Page.ProductAmountSpinBox.Value = StoreItem.Amount;
        }

        private void SetNameComboBox()
        {
            DataTable table = Manager.AllDataTables[ProjectManager.SelectPart.PRODUCT];
            List<string> nameList = new List<string>();
            NameCategories = new Dictionary<string, Product.Categories>();
            NameBrands = new Dictionary<string, Product.Brands>();
            NamePrice = new Dictionary<string, int>();
            NameProductID = new Dictionary<string, int>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow row = table.Rows[i];
                string name = row["ProductName"].ToString();
                StoreItem.Categories category = StoreItem.GetCategory(row["Category"].ToString());
                StoreItem.Brands brands = StoreItem.GetBrand(row["Brand"].ToString());
                int price = int.Parse(row["Price"].ToString());
                int id = int.Parse(row["ProductID"].ToString());
                NameCategories.Add(name, category);
                NameBrands.Add(name, brands);
                NamePrice.Add(name, price);
                NameProductID.Add(name, id);
                nameList.Add(name);
            }

            Page.ProductNameComboBox.ItemsSource = nameList;
        }

        public void InitEvent()
        {
            Page.PriceChanged += Page_PriceChanged;
            Page.AmountChanged += Page_AmountChanged;
            Page.ExpDateChanged += Page_ExpDateChanged;
            Page.NameChanged += Page_NameChanged;
            if (IsEditMode)
                Page.AcceptButtonClick += Page_AcceptEditedProduct;
            else
                Page.AcceptButtonClick += Page_AcceptNewProduct;
            Page.CancelButtonClick += Page_CancelButtonClick;
        }

        private void Page_NameChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (Page.ProductNameComboBox.SelectedIndex < 0) return;
            StoreItem.ProductName = Page.ProductNameComboBox.Text;
            Page.ProductCategoryComboBox.Text = NameCategories[StoreItem.ProductName].ToString().Replace("_", " ");
            Page.ProductBrandComboBox.Text = NameBrands[StoreItem.ProductName].ToString().Replace("_", " ");
            Page.ProductPriceSpinBox.Value = NamePrice[StoreItem.ProductName];
            StoreItem.Category = NameCategories[StoreItem.ProductName];
            StoreItem.Brand = NameBrands[StoreItem.ProductName];
            StoreItem.Price = NamePrice[StoreItem.ProductName];
            StoreItem.ProductID = NameProductID[StoreItem.ProductName];
            AcceptValidation();
        }

        private void Page_ExpDateChanged(DateTime dateTime)
        {
            StoreItem.ExpDate = dateTime;
            AcceptValidation();
        }

        private void Page_AmountChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            StoreItem.Amount = (int)Page.ProductAmountSpinBox.Value;
            AcceptValidation();
        }

        private void Page_PriceChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            StoreItem.Price = (int)Page.ProductPriceSpinBox.Value;
            AcceptValidation();
        }

        public void InitNewObject()
        {
            StoreItem = new StoreItem().SetStoreID(Manager.StoreDatabase.GetNextID());
            Page = new StorePage();
            Viewer.Frame.Content = Page;
            InitPage();
        }

        public void InitObject()
        {
            StoreItem = StoreItem.GetStoreItem(RowFocused);
            Page = new StorePage();
            Viewer.Frame.Content = Page;
            InitPage();
        }

        public void InitPage()
        {
            InitComponent();
            InitEvent();
        }

        public void Page_AcceptEditedProduct(object sender, RoutedEventArgs e)
        {
            Manager.StoreDatabase.Update(StoreItem);
            Task task = Manager.LoadDatabase();
            task.Wait();
            RowFocused = null;
            Viewer.Frame.Content = null;
            Viewer.EditButtonEnable = false;
            Viewer.DeleteButtonEnable = false;
        }

        public void Page_AcceptNewProduct(object sender, RoutedEventArgs e)
        {
            Manager.StoreDatabase.Insert(StoreItem);
            Task task = Manager.LoadDatabase();
            task.Wait();
            InitNewObject();
        }

        public void Page_CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Viewer.Frame.Content = null;
        }

        public void Viewer_DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(RowFocused[0].ToString());
            Manager.StoreDatabase.Delete(id);
            Task task = Manager.LoadDatabase();
            task.Wait();
            RowFocused = null;
            Viewer.EditButtonEnable = false;
            Viewer.DeleteButtonEnable = false;
        }

        public void Viewer_EditButtonClick(object sender, RoutedEventArgs e)
        {
            IsEditMode = true;
            InitObject();
            AcceptValidation();
        }

        public void Viewer_NewButtonClick(object sender, RoutedEventArgs e)
        {
            IsEditMode = false;
            InitNewObject();
        }

        public void Viewer_TableSelectingRow(object sender, CanSelectRowEventArgs e)
        {
            DataRowView row = (DataRowView)e.Row;
            RowFocused = (row == null) ? null : row.Row;
            Viewer.EditButtonEnable = !(RowFocused == null);
            Viewer.DeleteButtonEnable = !(RowFocused == null);
        }
    }
}
