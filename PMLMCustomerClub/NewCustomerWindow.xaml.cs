using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
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
using System.IO;
using System.Reflection;
using System.Data;

namespace PMLMCustomerClub
{
    /// <summary>
    /// Interaction logic for NewCustomerWindow.xaml
    /// </summary>
    public partial class NewCustomerWindow : Window
    {
        public NewCustomerWindow(int nextCustomerID)
        {
            InitializeComponent();
            NextCustomerProfile.ID = nextCustomerID;
            SetDateBoxes();
            SetReferralNameTextBlock();
            IsNewCustomer = true;
        }
        public NewCustomerWindow(DataRow dataRow)
        {
            InitializeComponent();
            string filePath = dataRow[7].ToString();
            SetDateBoxes();
            SetReferralNameTextBlock();
            IsNewCustomer = false;
            CustomerProfile cProfile = FileManagment.ReadCustomerProfile(filePath);
            SetCustomerProfileToItems(cProfile);
            
        }

        CustomerProfile NextCustomerProfile = new CustomerProfile();
        private enum PersianMonths { Far, Ord, Khor, Tir, Mor, Shah, Mehr, Aba, Aza, Dey, Bah, Esf };
        private enum CristienMonths { Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov, Dec };
        private bool IsNewCustomer = false;

        private void SetDateBoxes()
        {
            List<string> monthsList = new List<string>();
            if (SelectCalenderToggleSwitch.IsChecked == false)
            {
                var persianMonths = Enum.GetValues(typeof(PersianMonths)).Cast<PersianMonths>().ToList();
                foreach(var item in persianMonths)
                {
                    string s = item.ToString();
                    monthsList.Add(s);
                }
                YearSpinEdit.Text = "1300";
                DaySpinEdit.Text = "1";
            }
            else
            {
                var cristienMonths = Enum.GetValues(typeof(CristienMonths)).Cast<CristienMonths>().ToList();
                foreach(var item in cristienMonths)
                {
                    string s = item.ToString();
                    monthsList.Add(s);
                }
                YearSpinEdit.Text = "1950";
                DaySpinEdit.Text = "1";
            }

            MonthComboBoxEdit.ItemsSource = monthsList;
            MonthComboBoxEdit.SelectedIndex = 0;
        }

        private void SetReferralNameTextBlock()
        {
            if (int.Parse(ReferralCodeSpinEdit.Text) == 0)
                ReferralerPersonTextBlock.Text = "No Referral Code";
            else
            {
                CustomerProfile referralCustomer;
                if (DataBase.SearchCustomerProfile(int.Parse(ReferralCodeSpinEdit.Text), out referralCustomer))
                {
                    if (NextCustomerProfile.ID == int.Parse(ReferralCodeSpinEdit.Text))
                        ReferralerPersonTextBlock.Text = "No Referral Code";
                    else
                        ReferralerPersonTextBlock.Text = referralCustomer.Name;
                }
                else
                {
                    ReferralerPersonTextBlock.Text = "No Referral Code";
                }
            }
        }

        private void UpdateCustomerValues()
        {
            NextCustomerProfile.Name = FirstNameTextEdit.Text + " " + LastNameTextEdit.Text;
            NextCustomerProfile.PhoneNumber = PhoneTextEdit.Text;
            NextCustomerProfile.Credit = 0;
            NextCustomerProfile.ReferralCode = int.Parse(ReferralCodeSpinEdit.Text);
            NextCustomerProfile.Address.State = StateTextEdit.Text;
            NextCustomerProfile.Address.City = CityTextEdit.Text;
            NextCustomerProfile.Address.Location = LocationTextBox.Text;
            NextCustomerProfile.Address.ZipCode = ZipCodeTextEdit.Text;
            NextCustomerProfile.BirthDay = SetBirthDayToDateTime();
            if(IsNewCustomer)
                NextCustomerProfile.FilePath = LastNameTextEdit.Text + "_" + FirstNameTextEdit.Text + "_" + DateTime.Now.ToString("yyyy-MM-dd");
        }

        private DateTime SetBirthDayToDateTime()
        {
            if (YearSpinEdit.Text == "" || DaySpinEdit.Text == "" || MonthComboBoxEdit.SelectedIndex == -1) return DateTime.Now;
            if (SelectCalenderToggleSwitch.IsChecked == false)
            {
                
                DateTime persianDateTime = new DateTime(int.Parse(YearSpinEdit.Text), (MonthComboBoxEdit.SelectedIndex + 1), 
                    int.Parse(DaySpinEdit.Text), new System.Globalization.PersianCalendar());
                return persianDateTime;
                
            }

            DateTime dateTime = new DateTime(int.Parse(YearSpinEdit.Text), MonthComboBoxEdit.SelectedIndex + 1,
                int.Parse(DaySpinEdit.Text));
            return dateTime;
        }
        
        private void SetCustomerProfileToItems(CustomerProfile customerProfile)
        {
            string filePath = customerProfile.FilePath;
            string[] filePathParts1 = filePath.Split("_".ToCharArray());
            LastNameTextEdit.Text = filePathParts1[0];
            string[] filePathParts2 = filePathParts1[1].Split("-".ToCharArray());
            FirstNameTextEdit.Text = filePathParts2[0];
            PhoneTextEdit.Text = customerProfile.PhoneNumber;
            ReferralCodeSpinEdit.Text = customerProfile.ReferralCode.ToString();
            StateTextEdit.Text = customerProfile.Address.State;
            CityTextEdit.Text = customerProfile.Address.City;
            ZipCodeTextEdit.Text = customerProfile.Address.ZipCode;
            LocationTextBox.Text = customerProfile.Address.Location;
            if (SelectCalenderToggleSwitch.IsChecked == false)
            {
                PersianCalendar pCalendar = new PersianCalendar();
                YearSpinEdit.Text = pCalendar.GetYear(customerProfile.BirthDay).ToString();
                MonthComboBoxEdit.SelectedIndex = pCalendar.GetMonth(customerProfile.BirthDay) - 1;
                DaySpinEdit.Text = pCalendar.GetDayOfMonth(customerProfile.BirthDay).ToString();

            }
            else
            {
                YearSpinEdit.Text = customerProfile.BirthDay.Year.ToString();
                MonthComboBoxEdit.SelectedIndex = customerProfile.BirthDay.Month - 1;
                DaySpinEdit.Text = customerProfile.BirthDay.Day.ToString();
            }
            NextCustomerProfile.ID = customerProfile.ID;
            NextCustomerProfile.FilePath = customerProfile.FilePath;
        }

        private void FirstNameTextEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            UpdateCustomerValues();
        }

        private void LastNameTextEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            UpdateCustomerValues();
        }

        private void PhoneTextEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            UpdateCustomerValues();
        }

        private void ReferralCodeSpinEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            SetReferralNameTextBlock();
            UpdateCustomerValues();
        }

        private void SelectCalenderToggleSwitch_Checked(object sender, RoutedEventArgs e)
        {
            CustomerProfile customerProfile = (CustomerProfile)NextCustomerProfile.Clone();
            SetDateBoxes();
            YearSpinEdit.Text = customerProfile.BirthDay.Year.ToString();
            MonthComboBoxEdit.SelectedIndex = customerProfile.BirthDay.Month - 1;
            DaySpinEdit.Text = customerProfile.BirthDay.Day.ToString();
        }

        private void SelectCalenderToggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            CustomerProfile customerProfile = (CustomerProfile)NextCustomerProfile.Clone();
            SetDateBoxes();
            PersianCalendar pCalendar = new PersianCalendar();
            YearSpinEdit.Text = pCalendar.GetYear(customerProfile.BirthDay).ToString();
            MonthComboBoxEdit.SelectedIndex = pCalendar.GetMonth(customerProfile.BirthDay) - 1;
            DaySpinEdit.Text = pCalendar.GetDayOfMonth(customerProfile.BirthDay).ToString();
        }

        private void YearSpinEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            
            UpdateCustomerValues();
        }

        private void MonthComboBoxEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (MonthComboBoxEdit.SelectedIndex < 6) DaySpinEdit.MaxValue = 32;
            else
            {
                DaySpinEdit.MaxValue = 31;
                if (int.Parse(DaySpinEdit.Text) == 31)
                    DaySpinEdit.Text = "30";
            }
            UpdateCustomerValues();
        }

        private void DaySpinEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (DaySpinEdit.Text == "0") DaySpinEdit.Text = (DaySpinEdit.MaxValue - 1).ToString();
            else if (DaySpinEdit.Text == DaySpinEdit.MaxValue.ToString()) DaySpinEdit.Text = "1";
            UpdateCustomerValues();
        }

        private void StateTextEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            UpdateCustomerValues();
        }

        private void CityTextEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            UpdateCustomerValues();
        }

        private void ZipCodeTextEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            UpdateCustomerValues();
        }

        private void LocationTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateCustomerValues();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsNewCustomer)
            {
                FileManagment.WriteCustomerProfile(NextCustomerProfile);
                DataBase.InsertNewCustomer(NextCustomerProfile);
                this.Close();
            }
            else
            {
                FileManagment.WriteCustomerProfile(NextCustomerProfile);
                DataBase.UpdateCustomerProfile(NextCustomerProfile);
                this.Close();
            }
        }

        
    }
}
