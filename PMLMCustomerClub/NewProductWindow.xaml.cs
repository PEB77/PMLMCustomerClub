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
    /// Interaction logic for NewProductWindow.xaml
    /// </summary>
    public partial class NewProductWindow : Window
    {
        public NewProductWindow(int tableRows, ProductView productView)
        {
            InitializeComponent();
            SetComboBoxesItems();
            NewProductInfo.ProductID = tableRows + 1;
            IDBox.Text = NewProductInfo.ProductID.ToString();
            NewItem = true;
            ProductView1 = productView;
        }

        public NewProductWindow(DataRow dataRow, ProductView productView)
        {
            InitializeComponent();
            SetComboBoxesItems();

            IDBox.Text = dataRow[0].ToString();
            NameBox.Text = dataRow[1].ToString();
            string brand = dataRow[3].ToString().Replace("_", " ");
            BrandComboBox.Text = brand;
            
            string category = dataRow[2].ToString().Replace("_", " ");
            CategoryComboBox.Text = category;

            PriceBox.Text = dataRow[4].ToString();

            NewProductInfo.ProductID = int.Parse(dataRow[0].ToString());
            NewProductInfo.ProductName = NameBox.Text;
            NewProductInfo.ProductCategory = (ProductInfo.Category)Enum.Parse(typeof(ProductInfo.Category),
                dataRow[2].ToString().Replace(" ", "_"));
            NewProductInfo.ProductBrand = (ProductInfo.Brand)Enum.Parse(typeof(ProductInfo.Brand),
                dataRow[3].ToString().Replace(" ", "_"));
            NewProductInfo.ProductPrice = int.Parse(PriceBox.Text);
            EditItem = true;
            ProductView1 = productView;
        }

        ProductInfo NewProductInfo = new ProductInfo();
        bool NewItem = false;
        bool EditItem = false;
        ProductView ProductView1;
        private const int NameLimitationChar = 60;

        private void SetComboBoxesItems()
        {
            SetCategoryComboBoxItems();
            SetBrandComboBoxItems();
        }

        private void SetCategoryComboBoxItems()
        {
            var categoryType = Enum.GetValues(typeof(ProductInfo.Category)).Cast<ProductInfo.Category>().ToList();
            List<string> comboBoxItems = new List<string>();
            foreach(var category in categoryType)
            {
                string s = category.ToString();
                comboBoxItems.Add(s.Replace("_", " "));
            }

            CategoryComboBox.DataContext = comboBoxItems;
        }

        private void SetBrandComboBoxItems()
        {
            var brandType = Enum.GetValues(typeof(ProductInfo.Brand)).Cast<ProductInfo.Brand>().ToList();
            List<string> comboBoxItems = new List<string>();
            foreach (var brand in brandType)
            {
                string s = brand.ToString();
                comboBoxItems.Add(s.Replace("_", " "));
            }

            BrandComboBox.DataContext = comboBoxItems;
        }

        public void CancleButton_Click(object sender, EventArgs e)
        {

            ProductView1.Activate();
            this.Close();
        }

        public void AcceptButton_Click(object sender, EventArgs e)
        {
            if (NewItem)
            {
                DataBase.InsertNewProduct(NewProductInfo);
                ProductView1.Activate();
                //ProductView productView = new ProductView();
                //productView.Show();
                this.Close();
            }
            if (EditItem)
            {
                DataBase.UpdateProductRow(NewProductInfo);
                //ProductView productView = new ProductView();
                //productView.Show();
                ProductView1.Activate();
                this.Close();
            }
        }

        public void NameBox_EditValueChanged(object sender, EventArgs e)
        {
            string name = NameBox.Text;
            char[] nameChars = name.ToCharArray();
            if (nameChars.Length >= NameLimitationChar)
            {
                XtraMessageBox.Show("The input product name is too long.\nPlease summerized it.");
                NameBox.Text = "";
                return;
            }
            NewProductInfo.ProductName = NameBox.Text;

        }

        public void PriceBox_EditValueChanged(object sender, EventArgs e)
        {
            NewProductInfo.ProductPrice = int.Parse(PriceBox.Text);
        }

        public void CategoryComboBox_EditValueChanged(object sender, EventArgs e)
        {
            NewProductInfo.ProductCategory = (ProductInfo.Category)Enum.Parse(typeof(ProductInfo.Category), CategoryComboBox.SelectedIndex.ToString());
        }

        public void BrandComboBox_EditValueChanged(object sender, EventArgs e)
        {
            NewProductInfo.ProductBrand = (ProductInfo.Brand)Enum.Parse(typeof(ProductInfo.Brand), BrandComboBox.SelectedIndex.ToString());
        }

        public void IDBox_EditValueChanged(object sender, EventArgs e)
        {
            NewProductInfo.ProductID = int.Parse(IDBox.Text);
        }
    }
}
