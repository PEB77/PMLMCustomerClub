using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PMLMCustomerClub.View
{
    /// <summary>
    /// Interaction logic for CustomerPage.xaml
    /// </summary>
    public partial class CustomerPage : Page
    {
        public CustomerPage()
        {
            InitializeComponent();
        }

        public delegate void CustomerFirstNameChangedEventHandler(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e);
        public delegate void CustomerLastNameChangedEventHandler(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e);
        public delegate void CustomerPhoneChangedEventHandler(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e);
        public delegate void CustomerReferralCodeChangedEventHandler(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e);
        public delegate void CustomerStateChangedEventHandler(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e);
        public delegate void CustomerCityChangedEventHandler(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e);
        public delegate void CustomerZipCodeChangedEventHandler(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e);
        public delegate void CustomerBirthdayChangedEventHandler(DateTime dateTime);
        public delegate void CustomerLocationChangedEventHandler(object sender, TextChangedEventArgs e);
        public delegate void AcceptButtonClickEventHandler(object sender, RoutedEventArgs e);
        public delegate void CancelButtonClickEventHandler(object sender, RoutedEventArgs e);

        public event CustomerFirstNameChangedEventHandler FirstNameChanged;
        public event CustomerLastNameChangedEventHandler LastNameChanged;
        public event CustomerPhoneChangedEventHandler PhoneChanged;
        public event CustomerReferralCodeChangedEventHandler ReferralCodeChanged;
        public event CustomerStateChangedEventHandler StateChanged;
        public event CustomerCityChangedEventHandler CityChanged;
        public event CustomerZipCodeChangedEventHandler ZipCodeChanged;
        public event CustomerBirthdayChangedEventHandler BirthdayChanged;
        public event CustomerLocationChangedEventHandler LocationChanged;
        public event AcceptButtonClickEventHandler AcceptButtonClick;
        public event CancelButtonClickEventHandler CancelButtonClick;

        private void FirstNameTextEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            FirstNameChanged?.Invoke(sender, e);
        }

        private void LastNameTextEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            LastNameChanged?.Invoke(sender, e);
        }

        private void PhoneTextEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            PhoneChanged?.Invoke(sender, e);
        }

        private void BirthDayCalender_CalenderChange(DateTime date)
        {
            BirthdayChanged?.Invoke(date);
        }

        private void ReferralCodeSpinEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            ReferralCodeChanged?.Invoke(sender, e);
        }

        private void StateTextEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            StateChanged?.Invoke(sender, e);
        }

        private void CityTextEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            CityChanged?.Invoke(sender, e);
        }

        private void LocationTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LocationChanged?.Invoke(sender, e);
        }

        private void ZipCodeTextEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            ZipCodeChanged?.Invoke(sender, e);
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            AcceptButtonClick?.Invoke(sender, e);
        }

        private void CancleButton_Click(object sender, RoutedEventArgs e)
        {
            CancelButtonClick?.Invoke(sender, e);
        }
    }
}
