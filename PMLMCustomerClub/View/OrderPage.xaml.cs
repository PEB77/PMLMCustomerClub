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
    /// Interaction logic for OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        public OrderPage()
        {
            InitializeComponent();
        }

        public delegate void CustomerNameChangedEventHandler(int index, DevExpress.Xpf.Editors.EditValueChangedEventArgs e);
        public delegate void ProductChangedEventHandler(int index, DevExpress.Xpf.Editors.EditValueChangedEventArgs e);
        public delegate void StoreItemChangedEventHandler(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e);
        public delegate void AmountItemChangedEventHandler(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e);
        public delegate void AddItemButtonClickEventHandler(object sender, RoutedEventArgs e);
        public delegate void RemoveItemButtonClickEventHandler(object sender, RoutedEventArgs e);
        public delegate void ItemSelectedEventHandler(object sender, DevExpress.Xpf.Grid.CanSelectRowEventArgs e);
        public delegate void BirthDayGiftChangedEventHandler(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e);
        public delegate void UseBirthDayGiftChangedEventHandler(object sender, RoutedEventArgs e);
        public delegate void CreditChangedEventHandler(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e);
        public delegate void DescriptionChangedEventHandler(object sender, TextChangedEventArgs e);
        public delegate void AcceptButtonClickEventHandler(object sender, RoutedEventArgs e);
        public delegate void CancelButtonClickEventHandler(object sender, RoutedEventArgs e);

        public event CustomerNameChangedEventHandler CustomerNameChanged;
        public event ProductChangedEventHandler ProductChanged;
        public event StoreItemChangedEventHandler StoreItemChanged;
        public event AmountItemChangedEventHandler AmountItemChanged;
        public event AddItemButtonClickEventHandler AddItemButtonClick;
        public event RemoveItemButtonClickEventHandler RemoveItemButtonClick;
        public event ItemSelectedEventHandler ItemSelected;
        public event BirthDayGiftChangedEventHandler BirthDayGiftChanged;
        public event UseBirthDayGiftChangedEventHandler UseBirthDayGiftChanged;
        public event CreditChangedEventHandler CreditChanged;
        public event DescriptionChangedEventHandler DescriptionChanged;
        public event AcceptButtonClickEventHandler AcceptButtonClick;
        public event CancelButtonClickEventHandler CancelButtonClick;

        private void CustomerNameComboBox_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            int index = CustomerNameComboBox.SelectedIndex;
            CustomerNameChanged?.Invoke(index, e);
        }

        private void ProductNameComboBoxEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            int index = ProductNameComboBoxEdit.SelectedIndex;
            ProductChanged?.Invoke(index, e);
        }

        private void ProductInStoreDateExpComboBoxEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            StoreItemChanged?.Invoke(sender, e);
        }

        private void ProductAmountSpinEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            AmountItemChanged?.Invoke(sender, e);
        }

        private void RemoveProductButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveItemButtonClick?.Invoke(sender, e);
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            AddItemButtonClick?.Invoke(sender, e);
        }

        private void ProductSelectedTable_CanSelectRow(object sender, DevExpress.Xpf.Grid.CanSelectRowEventArgs e)
        {
            ItemSelected?.Invoke(sender, e);
        }

        private void BirthdayGiftAmountTextEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            BirthDayGiftChanged?.Invoke(sender, e);
        }

        private void UseBirthdayGiftCheckEdit_Checked(object sender, RoutedEventArgs e)
        {
            UseBirthDayGiftChanged?.Invoke(sender, e);
        }

        private void UseCreditSpinEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            CreditChanged?.Invoke(sender, e);
        }

        private void DescriptionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DescriptionChanged?.Invoke(sender, e);
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
