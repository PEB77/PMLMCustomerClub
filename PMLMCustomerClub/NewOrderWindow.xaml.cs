using DevExpress.Export.Xl;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Editors.Helpers;
using PMLMCustomerClub.Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
    /// Interaction logic for NewOrderWindow.xaml
    /// </summary>
    public partial class NewOrderWindow : Window
    {
        public NewOrderWindow(int orderID)
        {
            InitializeComponent();
            NextOrder.ID = orderID;
            SetCustomerComboBox();
            SetProductComboBox();

            ProductListDataTable.Columns.Add(Column0);
            ProductListDataTable.Columns.Add(Column1);
            ProductListDataTable.Columns.Add(Column2);
            ProductListDataTable.Columns.Add(Column3);
            ProductListDataTable.Columns.Add(Column4);
            ProductListDataTable.Columns.Add(Column5);
            ProductListDataTable.Columns.Add(Column6);
            ProductListDataTable.Columns.Add(Column7);
            ProductSelectedGridControl.ItemsSource = ProductListDataTable;

            NewOrderFlag = true;
        }        
        public NewOrderWindow(OrdersInfo oldOrder)
        {
            InitializeComponent();
            SetCustomerComboBox();
            SetProductComboBox();
            
            ProductListDataTable.Columns.Add(Column0);
            ProductListDataTable.Columns.Add(Column1);
            ProductListDataTable.Columns.Add(Column2);
            ProductListDataTable.Columns.Add(Column3);
            ProductListDataTable.Columns.Add(Column4);
            ProductListDataTable.Columns.Add(Column5);
            ProductListDataTable.Columns.Add(Column6);
            ProductListDataTable.Columns.Add(Column7);
            ProductSelectedGridControl.ItemsSource = ProductListDataTable;

            NewOrderFlag = false;
            InputOldOrderInComponent(oldOrder);
        }

        OrdersInfo NextOrder = new OrdersInfo();
        Dictionary<DateTime, StoreItem> ExpDate_Product;
        StoreItem SelectedProduct;
        DataTable ProductListDataTable = new DataTable("ProductTable");

        const string Column0 = "Store ID";
        const string Column1 = "Product ID";
        const string Column2 = "Product Name";
        const string Column3 = "Category";
        const string Column4 = "Brand";
        const string Column5 = "Price";
        const string Column6 = "ExpDate";
        const string Column7 = "Amount";

        DataRow RowSelected;
        bool NewOrderFlag;
        int RawPrice = 0;

        private void InputOldOrderInComponent(OrdersInfo order)
        {
            NextOrder.ID = order.ID;
            CustomerNameComboBox.SelectedIndex = order.Customer.ID - 1;
            InputLastOrderProductInTable(order.Products);
            NextOrder.Products = order.Products;
            AddProductToTable(ref ProductListDataTable, NextOrder.Products);
            NextOrder.Details = order.Details;
            NextOrder.OrderDate = order.OrderDate;
            DescriptionTextBox.Text = order.Details;
        }

        private void InputLastOrderProductInTable(List<StoreItem> products)
        {
            foreach(StoreItem p in products)
            {
                StoreItem newP = DataBase.SearchProductInStore(p);
                if (newP.StoreID != 0)
                    p.StoreID = newP.StoreID;
            }
        }

        private void SetCustomerComboBox()
        {
            DataTable dataTable = DataBase.GetCustomersDataTable();
            Dictionary<int, string> id_name = new Dictionary<int, string>();
            for(int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow dataRow = dataTable.Rows[i];
                int id = int.Parse(dataRow[0].ToString());
                string name = dataRow[1].ToString();
                id_name.Add(id, name);
            }
            List<string> nameList = new List<string>(id_name.Values);
            CustomerNameComboBox.ItemsSource = nameList;
        }

        private void SetProductComboBox()
        {
            DataTable dataTable = DataBase.GetProductsDataTable();
            Dictionary<int, string> id_name = new Dictionary<int, string>();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow dataRow = dataTable.Rows[i];
                int id = int.Parse(dataRow[0].ToString());
                string name = dataRow[1].ToString();
                id_name.Add(id, name);
            }
            List<string> nameList = new List<string>(id_name.Values);
            ProductNameComboBoxEdit.ItemsSource = nameList;
        }

        private void UpdateCustomerPart()
        {
            CustomerProfile customerProfile;
            if (DataBase.SearchCustomerProfile(CustomerNameComboBox.Text, out customerProfile))
            {
                NextOrder.Customer = new CustomerProfile(customerProfile);
                CustomerReferralCodeTextBlock.Text = NextOrder.Customer.ReferralCode.ToString();
                CustomerCreditTextBlock.Text = NextOrder.Customer.Credit.ToString();
                PersianCalendar persianCalendar = new PersianCalendar();
                string year = persianCalendar.GetYear(NextOrder.Customer.BirthDay).ToString();
                string month = persianCalendar.GetMonth(NextOrder.Customer.BirthDay).ToString();
                string day = persianCalendar.GetDayOfMonth(NextOrder.Customer.BirthDay).ToString();
                CustomerBirthDayTextBlock.Text = day + "-" + month + "-" + year;

                DateTime now = DateTime.Now;
                if (persianCalendar.GetMonth(now) == persianCalendar.GetMonth(NextOrder.Customer.BirthDay))
                {
                    BirthdayGiftAmountTextEdit.IsEnabled = true;
                    //UseBirthdayGiftCheckEdit.IsEnabled = true;
                }
                else
                {
                    BirthdayGiftAmountTextEdit.IsEnabled = false;
                    UseBirthdayGiftCheckEdit.IsEnabled = false;
                }

                if (NextOrder.Customer.Credit != 0)
                {
                    UseCreditSpinEdit.IsEnabled = true;
                    UsedCreditTextBlock.Text = "0";
                }
                else
                {
                    UseCreditSpinEdit.IsEnabled = false;
                    UsedCreditTextBlock.Text = "";
                }

            }
        }

        private void UpdateProductNamePart()
        {
            if (ProductNameComboBoxEdit.SelectedIndex == -1)
            {
                ProductInStoreDateExpComboBoxEdit.ItemsSource = null;
                ProductInStoreDateExpComboBoxEdit.IsEnabled = false;
                ProductInStoreDateExpComboBoxEdit.Text = "";
                ProductAmountSpinEdit.Text = "0";
                ProductAmountSpinEdit.IsEnabled = false;
                return;
            }

            DataTable dataTable = DataBase.SearchProductInStore(ProductNameComboBoxEdit.Text);
            ExpDate_Product = new Dictionary<DateTime, StoreItem>();

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow dataRow = dataTable.Rows[i];
                StoreItem product = DataBase.SearchProductInStore(int.Parse(dataRow[0].ToString()));
                ExpDate_Product.Add(product.ExpDate, product);
            }
            List<DateTime> expDates = new List<DateTime>(ExpDate_Product.Keys);
            ProductInStoreDateExpComboBoxEdit.IsEnabled = true;
            ProductInStoreDateExpComboBoxEdit.ItemsSource = expDates;
            ProductAmountSpinEdit.IsEnabled = true;
        }

        private void UpdateProductExpDate()
        {
            if (ProductInStoreDateExpComboBoxEdit.ItemsSource == null) return;

            DateTime dateTime = ProductInStoreDateExpComboBoxEdit.Text.TryConvertToDateTime();
            ProductAmountSpinEdit.IsEnabled = true;
            ProductAmountSpinEdit.MaxValue = ExpDate_Product[dateTime].Amount;
            SelectedProduct = (StoreItem)ExpDate_Product[dateTime].Clone();
        }

        private void UpdateProductAmount()
        {
            int amount = int.Parse(ProductAmountSpinEdit.Text);
            SelectedProduct.Amount = amount;
            if (amount != 0)
                AddProductButton.IsEnabled = true;
            else
                AddProductButton.IsEnabled = false;
        }

        private void AddProductToTable(ref DataTable dataTable, List<StoreItem> products)
        {
            int totalPrice = 0;
            foreach (StoreItem p in products)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow[0] = p.StoreID;
                dataRow[1] = p.ProductID;
                dataRow[2] = p.ProductName;
                dataRow[3] = p.Category;
                dataRow[4] = p.Brand;
                dataRow[5] = p.Price;
                dataRow[6] = p.ExpDate;
                dataRow[7] = p.Amount;
                dataTable.Rows.Add(dataRow);
               totalPrice += p.Amount * p.Price;
            }
            OrderTotalPriceTextEdit.Text = totalPrice.ToString();
            RawPrice = totalPrice;
        }

        private void UpdateProductList()
        {
            DataRow dataRow = ProductListDataTable.NewRow();
            dataRow[0] = SelectedProduct.StoreID;
            dataRow[1] = SelectedProduct.ProductID;
            dataRow[2] = SelectedProduct.ProductName;
            dataRow[3] = SelectedProduct.Category;
            dataRow[4] = SelectedProduct.Brand;
            dataRow[5] = SelectedProduct.Price;
            dataRow[6] = SelectedProduct.ExpDate;
            dataRow[7] = SelectedProduct.Amount;

            NextOrder.Products.Add((StoreItem)SelectedProduct.Clone());

            ProductListDataTable.Rows.Add(dataRow);
            ProductSelectedGridControl.ItemsSource = ProductListDataTable;

            if(OrderTotalPriceTextEdit.Text == "" || 
                OrderTotalPriceTextEdit.Text == "0")
            {
                int totalPrice = SelectedProduct.Amount * SelectedProduct.Price;
                RawPrice += SelectedProduct.Amount * SelectedProduct.Price;
                OrderTotalPriceTextEdit.Text = totalPrice.ToString();
            }
            else
            {
                int totalPrice = int.Parse(OrderTotalPriceTextEdit.Text);
                totalPrice += SelectedProduct.Amount * SelectedProduct.Price;
                RawPrice += SelectedProduct.Amount * SelectedProduct.Price;
                OrderTotalPriceTextEdit.Text = totalPrice.ToString();
            }

            ProductNameComboBoxEdit.SelectedIndex = -1;
            AddProductButton.IsEnabled = false;
        }

        private void SelectDataRow(object rowObj)
        {
            DataRowView row = (DataRowView)rowObj;
            if (row == null) return;
            RowSelected = row.Row;
            RemoveProductButton.IsEnabled = true;
        }

        private StoreItem ExportProductFromTable(DataRow dataRow)
        {
            StoreItem product = new StoreItem();
            product.StoreID = int.Parse(dataRow[0].ToString());
            product.ProductID = int.Parse(dataRow[1].ToString());
            product.ProductName = dataRow[2].ToString();
            product.Category = (StoreItem.Categories)Enum.Parse(typeof(StoreItem.Categories), dataRow[3].ToString());
            product.Brand = (StoreItem.Brands)Enum.Parse(typeof(StoreItem.Brands), dataRow[4].ToString());
            product.Price = int.Parse(dataRow[5].ToString());
            product.ExpDate = DateTime.Parse(dataRow[6].ToString());
            product.Amount = int.Parse(dataRow[7].ToString());
            return product;
        }

        private void RemoveFormList(List<StoreItem> products, StoreItem product)
        {
            int listMemebers = products.Count;
            int index = -1;
            for(int i = 0; i < listMemebers; i++)
            {
                if (products[i].Equals(product))
                    index = i;
            }
            if (index == -1) return;

            products.RemoveAt(index);
        }

        private void RemoveProductList()
        {
            StoreItem product = ExportProductFromTable(RowSelected);
            ProductListDataTable.Rows.Remove(RowSelected);
            ProductSelectedGridControl.ItemsSource = ProductListDataTable;
            RemoveFormList(NextOrder.Products, product);
            int totalPrice = int.Parse(OrderTotalPriceTextEdit.Text);
            totalPrice -= product.Amount * product.Price;
            RawPrice -= product.Amount * product.Price;
            OrderTotalPriceTextEdit.Text = totalPrice.ToString();
            RemoveProductButton.IsEnabled = false;
        }

        private void UseBirthday()
        {
            if (UseBirthdayGiftCheckEdit.IsChecked == true)
            {
                BirthdayGiftAmountTextEdit.IsEnabled = false;
                int gift = int.Parse(BirthdayGiftAmountTextEdit.Text);
                int price = 0;
                if (int.TryParse(BirthdayGiftAmountTextEdit.Text, out gift))
                {
                    price -= gift;
                }
                else
                {
                    price -= gift;
                }
                OrderTotalPriceTextEdit.Text = price.ToString();
                NextOrder.UseBirthdayGift = true;
                NextOrder.BirthdayGift = gift;
            }
        }

        private void AcceptOrder()
        {
            if (NewOrderFlag)
            {
                NextOrder.TotalPrice = int.Parse(OrderTotalPriceTextEdit.Text);
                NextOrder.FileName = "Order-" + DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH-mm-ss");
                NextOrder.OrderDate = DateTime.Now;
                FileManagment.WriteOrderFile(NextOrder);
                DataBase.InsertNewOrder(NextOrder);
                this.Close();
            }
            else
            {
                NextOrder.TotalPrice = int.Parse(OrderTotalPriceTextEdit.Text);
                NextOrder.FileName = "Order" + NextOrder.OrderDate.ToString("yyyy-MM-dd HH-mm-ss");
                FileManagment.WriteOrderFile(NextOrder);
                DataBase.UpdateOrdersList(NextOrder);
                this.Close();
            }
        }

        private void CustomerNameComboBox_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            UpdateCustomerPart();
        }

        private void ProductNameComboBoxEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            UpdateProductNamePart();
        }

        private void ProductInStoreDateExpComboBoxEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            UpdateProductExpDate();
        }

        private void ProductAmountSpinEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            UpdateProductAmount();
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateProductList();
        }

        private void RemoveProductButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveProductList();
        }

        private void ProductSelectedTable_CanSelectRow(object sender, DevExpress.Xpf.Grid.CanSelectRowEventArgs e)
        {
            SelectDataRow(e.Row);
        }

        private void UseBirthdayGiftCheckEdit_Checked(object sender, RoutedEventArgs e)
        {
            UseBirthday();
        }

        private void UseCreditSpinEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            double percentCredit = UseCreditSpinEdit.Value;
            double creditUsed = NextOrder.Customer.Credit * (percentCredit / 100);
            int price = (int)(RawPrice - creditUsed);
            UsedCreditTextBlock.Text = Math.Round(creditUsed, 0).ToString();
            OrderTotalPriceTextEdit.Text = price.ToString();
            NextOrder.CreditUsed = (int)creditUsed;
            NextOrder.UseCredit = true;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            OrdersView ordersView = new OrdersView();
            ordersView.Activate();
            this.Close();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            AcceptOrder();
        }

        private void BirthdayGiftAmountTextEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            int gift = int.Parse(BirthdayGiftAmountTextEdit.Text);
            if (gift == 0)
            {
                UseBirthdayGiftCheckEdit.IsEnabled = true;

            }
        }

        private void DescriptionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = DescriptionTextBox.Text;
            NextOrder.Details = text;
        }
        

    }
}
