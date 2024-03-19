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
using PMLMCustomerClub.Database;
using PMLMCustomerClub.Model;

namespace PMLMCustomerClub.Manager
{
    public class CustomerManager : IManager<CustomerPage, Customer>
    {
        public CustomerManager(ProjectManager manager, TableViewer viewer)
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

        public Customer Item { get; set; }
        public CustomerPage Page { get; set; }

        public void AcceptValidation()
        {
            if (Item.Validation())
                Page.AcceptButton.IsEnabled = true;
            else
                Page.AcceptButton.IsEnabled = false;
        }

        public void InitComponent()
        {
            Page.IDTextEdit.Text = Item.ID.ToString();
            Page.FirstNameTextEdit.Text = Item.FirstName;
            Page.LastNameTextEdit.Text = Item.LastName;
            Page.PhoneTextEdit.Text = Item.PhoneNumber;
            Page.BirthDayCalender.SetDate(Item.BirthDay);
            Page.ReferralCodeSpinEdit.Value = Item.ReferralCode;
            Page.StateTextEdit.Text = Item.Address.State;
            Page.CityTextEdit.Text = Item.Address.City;
            Page.LocationTextBox.Text = Item.Address.Location;
            Page.ZipCodeTextEdit.Text = Item.Address.ZipCode;
        }

        public void InitEvent()
        {
            Page.FirstNameChanged += Page_FirstNameChanged;
            Page.LastNameChanged += Page_LastNameChanged;
            Page.PhoneChanged += Page_PhoneChanged;
            Page.BirthdayChanged += Page_BirthdayChanged;
            Page.ReferralCodeChanged += Page_ReferralCodeChanged;
            Page.StateChanged += Page_StateChanged;
            Page.CityChanged += Page_CityChanged;
            Page.LocationChanged += Page_LocationChanged;
            Page.ZipCodeChanged += Page_ZipCodeChanged;
            if (IsEditMode)
                Page.AcceptButtonClick += Page_AcceptEditedProduct;
            else
                Page.AcceptButtonClick += Page_AcceptNewProduct;
            Page.CancelButtonClick += Page_CancelButtonClick;
        }

        private void Page_ZipCodeChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            Item.Address.ZipCode = (string)e.NewValue;
            AcceptValidation();
        }

        private void Page_LocationChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            Item.Address.Location = Page.LocationTextBox.Text;
            AcceptValidation();
        }

        private void Page_CityChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            Item.Address.City = (string)e.NewValue;
            AcceptValidation();
        }

        private void Page_StateChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            Item.Address.State = (string)e.NewValue;
            AcceptValidation();
        }

        private void Page_ReferralCodeChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            Item.ReferralCode = int.Parse(e.NewValue.ToString());
            AcceptValidation();
        }

        private void Page_BirthdayChanged(DateTime dateTime)
        {
            Item.BirthDay = dateTime;
            AcceptValidation();
        }

        private void Page_PhoneChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            Item.PhoneNumber = (string)e.NewValue;
            AcceptValidation();
        }

        private void Page_LastNameChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            Item.LastName = (string)e.NewValue;
            AcceptValidation();
        }

        private void Page_FirstNameChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            Item.FirstName = (string)e.NewValue;
            AcceptValidation();
        }

        public void InitNewObject()
        {
            Item = new Customer().SetID(Manager.CustomerDatabase.GetNextID());
            Page = new CustomerPage();
            Viewer.Frame.Content = Page;
            InitPage();
        }

        public void InitObject()
        {
            Item = Customer.GetCustomer(RowFocused);
            Page = new CustomerPage();
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
            FileManager.UpdateCustomer(Item);
            Manager.CustomerDatabase.Update(Item);
            Task task = Manager.LoadDatabase();
            task.Wait();
            RowFocused = null;
            Viewer.Frame.Content = null;
            Viewer.EditButtonEnable = false;
            Viewer.DeleteButtonEnable = false;
        }

        public void Page_AcceptNewProduct(object sender, RoutedEventArgs e)
        {
            Item.CreateFolderName();
            FileManager.SaveCustomer(Item);
            Manager.CustomerDatabase.Insert(Item);
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
            Item = Customer.GetCustomer(RowFocused);
            FileManager.DeleteCustomerFolder(Item.FolderName);
            Manager.CustomerDatabase.Delete(id);
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
