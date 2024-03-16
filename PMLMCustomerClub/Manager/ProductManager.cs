using DevExpress.XtraEditors;
using PMLMCustomerClub.CustomControls;
using PMLMCustomerClub.View;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMLMCustomerClub.Database;
using PMLMCustomerClub.Model;

namespace PMLMCustomerClub.Manager
{

    public class ProductManager : IManager
    {

        public ProductManager(ProjectManager manager, TableViewer viewer)
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
        internal ProductPage Page;
        public Product Product;
        public DataRow RowFocused { get; set; }

        private const int NameLimitChars = 60;
        public bool IsEditMode { get; set; } = false;

        public void Viewer_TableSelectingRow(object sender, DevExpress.Xpf.Grid.CanSelectRowEventArgs e)
        {
            DataRowView row = (DataRowView)e.Row;
            RowFocused = (row == null) ? null : row.Row;
            Viewer.EditButtonEnable = !(RowFocused == null);
            Viewer.DeleteButtonEnable = !(RowFocused == null);
        }

        public void Viewer_DeleteButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            int id = int.Parse(RowFocused[0].ToString());
            Manager.ProductDatabase.Delete(id);
            Task task = Manager.LoadDatabase();
            task.Wait();
            RowFocused = null;
            Viewer.EditButtonEnable = false;
            Viewer.DeleteButtonEnable = false;
        }

        public void Viewer_EditButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            IsEditMode = true;
            InitObject();
            AcceptValidation();
        }

        public void Viewer_NewButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            IsEditMode = false;
            InitNewObject();
        }

        public void InitObject()
        {
            Product = Product.GetProduct(RowFocused);
            Page = new ProductPage();
            Viewer.Frame.Content = Page;
            InitPage();
        }

        public void InitNewObject()
        {
            Product = new Product();
            Product.ProductID = Manager.ProductDatabase.GetNextID();
            Page = new ProductPage();
            Viewer.Frame.Content = Page;
            InitPage();
        }

        public void InitPage()
        {
            InitComponent();
            InitEvent();
        }

        public void InitEvent()
        {
            Page.NameChanged += Page_NameChanged;
            Page.CategoryChanged += Page_CategoryChanged;
            Page.BrandChanged += Page_BrandChanged;
            Page.PriceChanged += Page_PriceChanged;
            if (IsEditMode)
                Page.AcceptButtonClick += Page_AcceptEditedProduct;
            else
                Page.AcceptButtonClick += Page_AcceptNewProduct;
            Page.CancelButtonClick += Page_CancelButtonClick;
        }

        public void Page_CancelButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            Viewer.Frame.Content = null;
        }

        public void Page_AcceptEditedProduct(object sender, System.Windows.RoutedEventArgs e)
        {
            Manager.ProductDatabase.Update(Product);
            Task task = Manager.LoadDatabase();
            task.Wait();
            RowFocused = null;
            Viewer.Frame.Content = null;
            Viewer.EditButtonEnable = false;
            Viewer.DeleteButtonEnable = false;
        }

        public void Page_AcceptNewProduct(object sender, System.Windows.RoutedEventArgs e)
        {
            Manager.ProductDatabase.Insert(Product);
            Task task = Manager.LoadDatabase();
            task.Wait();
            InitNewObject();
        }

        public void AcceptValidation()
        {
            if (Product.Validation())
                Page.AcceptButton.IsEnabled = true;
            else
                Page.AcceptButton.IsEnabled = false;
        }

        private void Page_PriceChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            Product.Price = int.Parse(Page.PriceBox.Text);
            AcceptValidation();
        }

        private void Page_BrandChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            string brandText = Page.BrandComboBox.SelectedItem as string;
            Product.Brand = (Product.Brands)Enum.Parse(typeof(Product.Brands), brandText.Replace(" ", "_"));
            AcceptValidation();
        }

        private void Page_CategoryChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            string categoryText = Page.CategoryComboBox.SelectedItem as string;
            Product.Category = (Product.Categories)Enum.Parse(typeof(Product.Categories), categoryText.Replace(" ", "_"));
            AcceptValidation();
        }

        private void Page_NameChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            string name = Page.NameBox.Text;
            char[] nameChars = name.ToCharArray();
            if (nameChars.Length >= NameLimitChars)
            {
                XtraMessageBox.Show("The input product name is too long.\nPlease summerized it.");
                Page.NameBox.Text = "";
                return;
            }
            Product.ProductName = Page.NameBox.Text;
            AcceptValidation();
        }

        public void InitComponent()
        {
            Page.IDBox.Text = Product.ProductID.ToString();
            Page.NameBox.Text = Product.ProductName;
            Page.CategoryComboBox.ItemsSource = SetComboBox(typeof(Product.Categories));
            Page.BrandComboBox.ItemsSource = SetComboBox(typeof(Product.Brands));
            if (Product.ProductName != null)
            {
                Page.CategoryComboBox.Text = Product.Category.ToString().Replace("_", " ");
                Page.BrandComboBox.Text = Product.Brand.ToString().Replace("_", " ");
                Page.PriceBox.Text = Product.Price.ToString();
            }

        }

        private List<string> SetComboBox(Type type)
        {
            List<string> items = type.GetEnumNames().ToList();
            List<string> outputItems = new List<string>();
            
            foreach (string item in items)
            {
                string s = item.ToString();
                outputItems.Add(s.Replace("_", " "));
            }

            return outputItems;
        }


    }

}
