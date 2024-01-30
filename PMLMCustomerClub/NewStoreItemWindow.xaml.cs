using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Printing;
using PMLMCustomerClub.Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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
    /// Interaction logic for NewStoreItemWindow.xaml
    /// </summary>
    public partial class NewStoreItemWindow : Window
    {
        public NewStoreItemWindow(int lastRowNumber)
        {
            InitializeComponent();
            SetComboBoxesItems();
            NextRowInStore = lastRowNumber + 1;
            Product.StoreID = -1;
            NewItemProcess = true;
        }

        public NewStoreItemWindow(DataRow dataRow)
        {
            InitializeComponent();
            SetComboBoxesItems();

            Product.StoreID = int.Parse(dataRow[0].ToString());
            Product.ProductID = int.Parse(dataRow[1].ToString());
            ProductNameComboBox.Text = dataRow[2].ToString();
            ProductCategoryComboBox.Text = dataRow[3].ToString();
            ProductBrandComboBox.Text = dataRow[4].ToString();
            ProductPriceSpinBox.Text = dataRow[5].ToString();
            DateTime dateTime = DateTime.Parse(dataRow[6].ToString());
            YearComboBox.Text = dateTime.Year.ToString();
            MonthComboBox.SelectedIndex = dateTime.Month - 1;
            ProductAmountSpinBox.Text = dataRow[7].ToString();

            ProductAmountSpinBox.MinValue = 0;
            EditItemProcess = true;
        }

        DataTable DataTable1 = new DataTable();
        Dictionary<string, int> NameIDDic = new Dictionary<string, int>();
        Dictionary<string, int> NamePriceDic = new Dictionary<string, int>();
        Dictionary<string, string> NameCategoryDic = new Dictionary<string, string>();
        Dictionary<string, string> NameBrandDic = new Dictionary<string, string>();

        enum Months { Jan = 1, Feb = 2, Mar = 3, Apr = 4, May = 5, Jun = 6,
            Jul = 7, Aug = 8, Sep = 9, Oct = 10, Nov = 11, Dec = 12 };

        StoreItem Product = new StoreItem();
        int NextRowInStore = 0;
        bool NewItemProcess = false;
        bool EditItemProcess = false;

        private void SetComboBoxesItems()
        {
            SetNameComboBox();
            SetCategoryComboBox();
            SetBrandComboBox();
            SetYearAndMonthComboBox();
        }

        private void SetNameComboBox()
        {
            DataTable1 = DataBase.GetProductsDataTable();
            NamePriceDic.Clear();
            List<string> nameList = new List<string>();
            for(int i = 0; i < DataTable1.Rows.Count; i++)
            {
                DataRow dataRow = DataTable1.Rows[i];
                string productName = dataRow["product_name"].ToString();
                string productCategory = dataRow["product_category"].ToString();
                string productBrand = dataRow["product_brand"].ToString();
                int productPrice = int.Parse(dataRow["product_price"].ToString());
                int productID = int.Parse(dataRow["product_id"].ToString());

                NameIDDic.Add(productName, productID);
                NamePriceDic.Add(productName, productPrice);
                NameCategoryDic.Add(productName, productCategory);
                NameBrandDic.Add(productName, productBrand);

            }

            ProductNameComboBox.ItemsSource = new List<string>(NamePriceDic.Keys);
        }

        private void SetCategoryComboBox()
        {
            var categoryList = Enum.GetValues(typeof(Product.Category)).Cast<Product.Category>().ToList();
            List<string> newCategoryList = new List<string>();
            foreach(var item in categoryList)
            {
                string categoryString = item.ToString();
                newCategoryList.Add(categoryString.Replace("_", " "));
            }
            ProductCategoryComboBox.ItemsSource = newCategoryList;
        }

        private void SetBrandComboBox()
        {
            var brandList = Enum.GetValues(typeof(Product.Brand)).Cast<Product.Brand>().ToList();
            List<string> newBrandList = new List<string>();
            foreach(var item in brandList)
            {
                string brandString = item.ToString();
                newBrandList.Add(brandString.Replace("_", ""));
            }
            ProductBrandComboBox.ItemsSource = newBrandList;
        }

        private void SetYearAndMonthComboBox()
        {
            DateTime dateTime = DateTime.Now;
            int currentYear = dateTime.Year;
            List<int> years = new List<int>() { currentYear, currentYear + 1, currentYear + 2, 
                currentYear + 3, currentYear + 4, currentYear + 5 };
            List<string> comboBoxItems1 = new List<string>();
            foreach (int year in years)
            {
                string s = year.ToString();
                comboBoxItems1.Add(s.Replace("_", " "));
            }

            var monthsType = Enum.GetValues(typeof(Months)).Cast<Months>().ToList();
            List<string> comboBoxItems2 = new List<string>();
            foreach (var month in monthsType)
            {
                string s = month.ToString();
                comboBoxItems2.Add(s.Replace("_", " "));
            }

            YearComboBox.ItemsSource = comboBoxItems1;
            MonthComboBox.ItemsSource = comboBoxItems2;
        }

        private void ProductNameComboBox_EditValueChanged(object sender, EventArgs e)
        {
            string name = ProductNameComboBox.Text;
            List<string> productNames = new List<string>(NameCategoryDic.Keys);
            string category;
            string brand;
            int price;

            if (productNames.Contains(name))
            {
                category = NameCategoryDic[name];
                brand = NameBrandDic[name];
                price = NamePriceDic[name];
            }
            else return;

            ProductBrandComboBox.Text = brand.Replace("_", " ");
            ProductCategoryComboBox.Text = category.Replace("_", " ");
            ProductPriceSpinBox.Text = price.ToString();
            
            UpdataNewStoreItem();
        }

        private void ProductCategoryComboBox_EditValueChanged(object sender, EventArgs e)
        {
            UpdataNewStoreItem();
        }

        private void ProductBrandComboBox_EditValueChanged(object sender, EventArgs e)
        {
            UpdataNewStoreItem();
        }

        private void ProductPriceSpinBox_EditValueChanged(object sender, EventArgs e)
        {
            UpdataNewStoreItem();
        }

        private void ProductAmountSpinBox_EditValueChanged(object sender, EventArgs e)
        {
            UpdataNewStoreItem();
        }

        private void YearComboBox_EditValueChanged(object sender, EventArgs e)
        {
            UpdataNewStoreItem();
        }

        private void MonthComboBox_EditValueChanged(object sender, EventArgs e)
        {
            UpdataNewStoreItem();
        }

        private void UpdataNewStoreItem()
        {
            Product.ProductName = ProductNameComboBox.Text;
            Product.ProductID = NameIDDic[Product.ProductName];
            if (int.TryParse(ProductPriceSpinBox.Text, out int priceOutput))
                Product.Price = priceOutput;
            if (int.TryParse(ProductAmountSpinBox.Text, out int amountOutput))
                Product.Amount = amountOutput;
            if (ProductCategoryComboBox.Text != "")
                Product.Category = (StoreItem.Categories)Enum.Parse(typeof(StoreItem.Categories),
                    ProductCategoryComboBox.Text.Replace(" ", "_"));
            if (ProductBrandComboBox.Text != "")
                Product.Brand = (StoreItem.Brands)Enum.Parse(typeof(StoreItem.Brands),
                    ProductBrandComboBox.Text.Replace(" ", "_"));

            if (YearComboBox.SelectedIndex != -1 && MonthComboBox.SelectedIndex != -1)
            {
                int year = DateTime.Now.Year + YearComboBox.SelectedIndex;
                int month = MonthComboBox.SelectedIndex + 1;
                Product.ExpDate = new DateTime(year, month, 1);
            }

        }

        private void CancleButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            if (NewItemProcess)
            {
                NewItemAcceptAction();
            }
            if (EditItemProcess)
            {
                EditItemAcceptAction();
            }
        }

        private void NewItemAcceptAction()
        {
            if (Product.ProductName == "") return;
            StoreItem product;

            if (SearchForSimilarItemInTable(Product, false, out product))
            {
                DataBase.UpdateStoreRow(product);
            }
            else
            {
                Product.StoreID = NextRowInStore;
                DataBase.InsertNewStoreRow(Product);
            }
            
            this.Close();
        }

        private void EditItemAcceptAction()
        {
            if (Product.Amount != 0)
            {
                StoreItem product = new StoreItem();
                if (SearchForSimilarItemInTable(Product, true, out product))
                {
                    DataBase.DeleteStoreRow(Product.StoreID);
                    DataBase.UpdateStoreRow(product);
                }
                else
                    DataBase.UpdateStoreRow(Product);
            }
            else
                DataBase.DeleteStoreRow(Product.StoreID);

            this.Close();
        }

        private bool SearchForSimilarItemInTable(StoreItem sampleProduct, bool isEditItem, out StoreItem product)
        {
            bool findObject = false;
            DataTable dataTable = DataBase.SearchProductInStore(sampleProduct.ProductName);
            product = new StoreItem();
            for(int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow dataRow = dataTable.Rows[i];
                if (sampleProduct.ExpDate == (DateTime)dataRow["product_expdate"])
                {
                    if (sampleProduct.Price == int.Parse(dataRow["product_price"].ToString()))
                    {
                        if (sampleProduct.StoreID == int.Parse(dataRow[0].ToString()) && isEditItem)
                        {
                            findObject = false;
                            return findObject;
                        }
                        
                        product.StoreID = int.Parse(dataRow[0].ToString());
                        product.ProductID = int.Parse(dataRow[1].ToString());
                        product.ProductName = dataRow[2].ToString();
                        product.Category = (StoreItem.Categories)Enum.Parse(typeof(StoreItem.Categories),
                            dataRow[3].ToString().Replace(" ", "_"));
                        product.Brand = (StoreItem.Brands)Enum.Parse(typeof(StoreItem.Brands),
                            dataRow[4].ToString().Replace(" ", "_"));
                        product.Price = sampleProduct.Price;
                        product.Amount = int.Parse(dataRow[7].ToString()) + sampleProduct.Amount;
                        product.ExpDate = sampleProduct.ExpDate;
                        findObject = true;
                        return findObject;
                    }
                }
                else
                    findObject = false;
            }
            return findObject;
        }
    }
}
