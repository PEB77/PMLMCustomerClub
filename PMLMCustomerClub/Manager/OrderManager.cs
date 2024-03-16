using DevExpress.Xpf.Bars.Themes;
using DevExpress.Xpf.Grid;
using PMLMCustomerClub.CustomControls;
using PMLMCustomerClub.View;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PMLMCustomerClub.Database;
using PMLMCustomerClub.Model;

namespace PMLMCustomerClub.Manager
{
    public class OrderManager : IManager
    {
        public OrderManager(ProjectManager manager, TableViewer viewer)
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

        public Order Order;
        public OrderPage Page;

        private List<StoreItem> Items;
        private StoreItem NewItem;
        private StoreItem ItemFocused;

        public void AcceptValidation()
        {
            Order.FinalBill();
            Page.OrderTotalPriceTextEdit.Text = Order.TotalPrice.ToString();
            if (Order.Validation())
                Page.AcceptButton.IsEnabled = true;
            else
                Page.AcceptButton.IsEnabled = false;
        }

        private void AddValidation()
        {
            if (NewItem.Validation())
                Page.AddProductButton.IsEnabled = true;
            else
                Page.AddProductButton.IsEnabled = false;
        }

        public void InitComponent()
        {
            Page.IDBox.Text = Order.ID.ToString();
            Page.CustomerNameComboBox.ItemsSource = PrepareCustomerSource(Manager.AllDataTables[ProjectManager.SelectPart.CUSTOMER]);
            Page.ProductNameComboBoxEdit.ItemsSource = PrepareProductSource(Manager.AllDataTables[ProjectManager.SelectPart.PRODUCT]);
            Page.ProductSelectedGridControl.ItemsSource = Order.Products;
            if (IsEditMode) Page.CustomerNameComboBox.Text = Order.Customer.FirstName + "-" + Order.Customer.LastName;
        }

        private List<string> PrepareCustomerSource(DataTable table)
        {
            List<string> names = new List<string>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow row = table.Rows[i];
                names.Add(row[1].ToString() + "-" + row[2].ToString());
            }
            return names;
        }

        private List<string> PrepareProductSource(DataTable table)
        {
            List<string> names = new List<string>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow row = table.Rows[i];
                names.Add(row[1].ToString());
            }
            return names;
        }

        public void InitEvent()
        {
            if (IsEditMode)
                Page.AcceptButtonClick += Page_AcceptEditedProduct;
            else
                Page.AcceptButtonClick += Page_AcceptNewProduct;
            Page.CancelButtonClick += Page_CancelButtonClick;
            Page.CustomerNameChanged += Page_CustomerNameChanged;
            Page.ProductChanged += Page_ProductChanged;
            Page.StoreItemChanged += Page_StoreItemChanged;
            Page.AmountItemChanged += Page_AmountItemChanged;
            Page.AddItemButtonClick += Page_AddItemButtonClick;
            Page.RemoveItemButtonClick += Page_RemoveItemButtonClick;
            Page.ItemSelected += Page_ItemSelected;
            Page.UseBirthDayGiftChanged += Page_UseBirthDayGiftChanged;
            Page.BirthDayGiftChanged += Page_BirthDayGiftChanged;
            Page.CreditChanged += Page_CreditChanged;
            Page.DescriptionChanged += Page_DescriptionChanged;
        }

        private void Page_DescriptionChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            Order.Details = Page.DescriptionTextBox.Text;
        }

        private void Page_CreditChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            int val = int.Parse(e.NewValue.ToString());
            Order.CreditUsed = val * Order.Customer.Credit / 100;
            AcceptValidation();
        }

        private void Page_BirthDayGiftChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            Order.BirthdayGift = (int)Page.BirthdayGiftAmountTextEdit.Value;
            AcceptValidation();
        }

        private void Page_UseBirthDayGiftChanged(object sender, RoutedEventArgs e)
        {
            if (Page.UseBirthdayGiftCheckEdit.IsChecked == true)
                Page.BirthdayGiftAmountTextEdit.IsEnabled = true;
            else
            {
                Page.BirthdayGiftAmountTextEdit.Value = 0;
                Page.BirthdayGiftAmountTextEdit.IsEnabled = false;
                Order.BirthdayGift = 0;
            }
            Order.UseBirthdayGift = (bool)Page.UseBirthdayGiftCheckEdit.IsChecked;
            AcceptValidation();
        }

        private void Page_ItemSelected(object sender, CanSelectRowEventArgs e)
        {
            StoreItem row = (StoreItem)e.Row;
            ItemFocused = (row == null) ? null : row;
            Page.RemoveProductButton.IsEnabled = !(row == null);
        }

        private void Page_RemoveItemButtonClick(object sender, RoutedEventArgs e)
        {
            
            int indexItem = -1;
            for(int i = 0; i < Order.Products.Count; i++)
            {
                if (Order.Products[i].Equals(ItemFocused))
                    indexItem = i;
            }
            if (indexItem != -1)
            {
                Order.Products.RemoveAt(indexItem);
                Page.ProductSelectedGridControl.ItemsSource = Order.Products;
            }
            Page.RemoveProductButton.IsEnabled = false;
            AddValidation();
            AcceptValidation();
        }

        private void Page_AddItemButtonClick(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i < Order.Products.Count; i++)
            {
                if (Order.Products[i].Equals(NewItem))
                {
                    Order.Products[i].Amount += NewItem.Amount;
                    NewItem = new StoreItem();
                    AddValidation();
                    return;
                }
            }
            Order.Products.Add((StoreItem)NewItem.Clone());
            NewItem = new StoreItem();
            AddValidation();
            ResetStoreItemPart();
            AcceptValidation();
        }

        private void ResetStoreItemPart()
        {
            Page.ProductNameComboBoxEdit.SelectedIndex = -1;
            Page.ProductInStoreDateExpComboBoxEdit.SelectedIndex = -1;
            Page.ProductInStoreDateExpComboBoxEdit.IsEnabled = false;
            Page.ProductAmountSpinEdit.Value = 0;
            Page.ProductAmountSpinEdit.IsEnabled = false;
            Page.ProductSelectedGridControl.ItemsSource = Order.Products;
        }

        private void Page_AmountItemChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            int amount = (int)Page.ProductAmountSpinEdit.Value;
            NewItem.Amount = amount;
            AddValidation();
        }

        private void Page_StoreItemChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            DateTime date;
            if (e.NewValue == null) return;
            if (DateTime.TryParse(e.NewValue.ToString(), out date))
            {
                foreach (StoreItem item in Items)
                {
                    if (item.ExpDate == date)
                    {
                        Page.ProductAmountSpinEdit.IsEnabled = true;
                        Page.ProductAmountSpinEdit.MaxValue = item.Amount;
                        Page.ProductAmountSpinEdit.Value = 0;
                        NewItem = (StoreItem)item.Clone();
                        NewItem.Amount = 0;
                        break;
                    }
                }
            }
            AddValidation();
        }

        private void Page_ProductChanged(int index, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            Items = new List<StoreItem>();
            NewItem = new StoreItem();
            if (e.NewValue == null)
            {
                Page.ProductInStoreDateExpComboBoxEdit.IsEnabled = false;
                Page.ProductInStoreDateExpComboBoxEdit.SelectedIndex = -1;
                Page.ProductAmountSpinEdit.Value = 0;
                Page.ProductAmountSpinEdit.IsEnabled = false;
                AddValidation();
                return;
            }
            string name = e.NewValue.ToString();
            if (Manager.StoreDatabase.TryExplore(name, out Items))
            {
                Page.ProductInStoreDateExpComboBoxEdit.IsEnabled = true;
                Page.ProductInStoreDateExpComboBoxEdit.SelectedIndex = -1;
                Page.ProductAmountSpinEdit.Value = 0;
                Page.ProductAmountSpinEdit.IsEnabled = false;
                List<string> expDates = new List<string>();
                foreach (StoreItem item in Items)
                {
                    expDates.Add(item.ExpDate.ToString("yyyy-MM-dd"));
                }
                Page.ProductInStoreDateExpComboBoxEdit.ItemsSource = expDates;
            }
            
            AddValidation();
        }

        private void Page_CustomerNameChanged(int index, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            Customer customer = new Customer();
            string rawInput = e.NewValue.ToString();
            string[] namesParts = rawInput.Split('-');
            if (Manager.CustomerDatabase.TryExplore(namesParts[0], namesParts[1], out customer))
            {
                Order.Customer = customer;
                CheckDiscounts();
                AcceptValidation();
            }
        }

        private void CheckDiscounts()
        {
            if (Order.Customer.Credit != 0)
            {
                Page.CustomerCreditTextBlock.Text = Order.Customer.Credit.ToString();
                Page.UseCreditSpinEdit.IsEnabled = true;
            }
            PersianCalendar pCalendar = new PersianCalendar();
            if (pCalendar.GetMonth(Order.Customer.BirthDay) == pCalendar.GetMonth(DateTime.Now))
            {
                Page.UseBirthdayGiftCheckEdit.IsEnabled = true;
            }
            Page.CustomerReferralCodeTextBlock.Text = Order.Customer.ReferralCode.ToString();
            Page.CustomerBirthDayTextBlock.Text = $"{pCalendar.GetYear(Order.Customer.BirthDay)}-{pCalendar.GetMonth(Order.Customer.BirthDay)}-{pCalendar.GetDayOfMonth(Order.Customer.BirthDay)}";
        }

        public void InitNewObject()
        {
            Order = new Order().SetID(Manager.OrderDatabase.GetNextID());
            Page = new OrderPage();
            Viewer.Frame.Content = Page;
            InitPage();
        }

        public void InitObject()
        {
            Order = Order.GetOrder(RowFocused);
            Page = new OrderPage();
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
            FileManager.UpdateOrder(Order);
            Manager.OrderDatabase.Update(Order);
            Task task = Manager.LoadDatabase();
            task.Wait();
            RowFocused = null;
            Viewer.Frame.Content = null;
            Viewer.EditButtonEnable = false;
            Viewer.DeleteButtonEnable = false;
        }

        public void Page_AcceptNewProduct(object sender, RoutedEventArgs e)
        {
            Order.CreateFileName();
            FileManager.SaveOrder(Order);
            Manager.OrderDatabase.Insert(Order);
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
            Order = Order.GetOrder(RowFocused);
            FileManager.DeleteOrderFile(Order);
            Manager.OrderDatabase.Delete(Order);
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
