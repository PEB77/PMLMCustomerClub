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

namespace PMLMCustomerClub.Code
{
    public class CustomerManager : IManager
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

        public Customer Customer;
        public CustomerPage Page;

        public void AcceptValidation()
        {
            if (Customer.Validation())
                Page.AcceptButton.IsEnabled = true;
            else
                Page.AcceptButton.IsEnabled = false;
        }

        public void InitComponent()
        {
            Page.IDBox.Text = Customer.ID.ToString();
            Page.FirstNameTextEdit.Text = Customer.FirstName;
            Page.LastNameTextEdit.Text = Customer.LastName;
            Page.PhoneTextEdit.Text = Customer.PhoneNumber;
            Page.BirthDayCalender.SetDate(Customer.BirthDay);
            Page.ReferralCodeSpinEdit.Value = Customer.ReferralCode;
            Page.StateTextEdit.Text = Customer.Address.State;
            Page.CityTextEdit.Text = Customer.Address.City;
            Page.LocationTextBox.Text = Customer.Address.Location;
            Page.ZipCodeTextEdit.Text = Customer.Address.ZipCode;
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
            Customer.Address.ZipCode = (string)e.NewValue;
            AcceptValidation();
        }

        private void Page_LocationChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            Customer.Address.Location = Page.LocationTextBox.Text;
            AcceptValidation();
        }

        private void Page_CityChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            Customer.Address.City = (string)e.NewValue;
            AcceptValidation();
        }

        private void Page_StateChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            Customer.Address.State = (string)e.NewValue;
            AcceptValidation();
        }

        private void Page_ReferralCodeChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            Customer.ReferralCode = int.Parse(e.NewValue.ToString());
            AcceptValidation();
        }

        private void Page_BirthdayChanged(DateTime dateTime)
        {
            Customer.BirthDay = dateTime;
            AcceptValidation();
        }

        private void Page_PhoneChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            Customer.PhoneNumber = (string)e.NewValue;
            AcceptValidation();
        }

        private void Page_LastNameChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            Customer.LastName = (string)e.NewValue;
            AcceptValidation();
        }

        private void Page_FirstNameChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            Customer.FirstName = (string)e.NewValue;
            AcceptValidation();
        }

        public void InitNewObject()
        {
            Customer = new Customer().SetID(Manager.CustomerDatabase.GetNextID());
            Page = new CustomerPage();
            Viewer.Frame.Content = Page;
            InitPage();
        }

        public void InitObject()
        {
            Customer = Customer.GetCustomer(RowFocused);
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
            FileManager.UpdateCustomer(Customer);
            Manager.CustomerDatabase.Update(Customer);
            Task task = Manager.LoadDatabase();
            task.Wait();
            RowFocused = null;
            Viewer.Frame.Content = null;
            Viewer.EditButtonEnable = false;
            Viewer.DeleteButtonEnable = false;
        }

        public void Page_AcceptNewProduct(object sender, RoutedEventArgs e)
        {
            Customer.CreateFolderName();
            FileManager.SaveCustomer(Customer);
            Manager.CustomerDatabase.Insert(Customer);
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
            Customer = Customer.GetCustomer(RowFocused);
            FileManager.DeleteCustomerFolder(Customer.FolderName);
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
