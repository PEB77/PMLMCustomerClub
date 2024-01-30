using PMLMCustomerClub.CustomControls;
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
    /// Interaction logic for StorePage.xaml
    /// </summary>
    public partial class StorePage : Page
    {
        public StorePage()
        {
            InitializeComponent();
        }

        public delegate void StoreItemNameChangedEventHandler(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e);
        public delegate void StoreItemAmountChangedEventHandler(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e);
        public delegate void StoreItemPriceChangedEventHandler(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e);
        public delegate void StoreItemExpDateChangedEventHandler(DateTime dateTime);
        public delegate void AcceptButtonClickEventHandler(object sender, RoutedEventArgs e);
        public delegate void CancelButtonClickEventHandler(object sender, RoutedEventArgs e);

        public event StoreItemNameChangedEventHandler NameChanged;
        public event StoreItemAmountChangedEventHandler AmountChanged;
        public event StoreItemPriceChangedEventHandler PriceChanged;
        public event StoreItemExpDateChangedEventHandler ExpDateChanged;
        public event AcceptButtonClickEventHandler AcceptButtonClick;
        public event CancelButtonClickEventHandler CancelButtonClick;

        private void ProductNameComboBox_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            NameChanged?.Invoke(sender, e);
        }

        private void ProductAmountSpinBox_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            AmountChanged?.Invoke(sender, e);
        }

        private void ExpCalender_CalenderChange(DateTime date)
        {
            ExpDateChanged?.Invoke(date);
        }

        private void ProductPriceSpinBox_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            PriceChanged?.Invoke(sender, e);
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            AcceptButtonClick?.Invoke(sender, e);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CancelButtonClick?.Invoke(sender, e);
        }
    }
}
