using PMLMCustomerClub.Code;
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
using PMLMCustomerClub.Code;

namespace PMLMCustomerClub.View
{
    /// <summary>
    /// Interaction logic for ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        public ProductPage()
        {
            InitializeComponent();

        }

        public delegate void ProductNameChangedEventHandler(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e);
        public delegate void ProductCategoryChangedEventHandler(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e);
        public delegate void ProductBrandChangedEventHandler(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e);
        public delegate void ProductPriceChangedEventHandler(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e);
        public delegate void AcceptButtonClickEventHandler(object sender, RoutedEventArgs e);
        public delegate void CancelButtonClickEventHandler(object sender, RoutedEventArgs e);

        public event ProductNameChangedEventHandler NameChanged;
        public event ProductCategoryChangedEventHandler CategoryChanged;
        public event ProductBrandChangedEventHandler BrandChanged;
        public event ProductPriceChangedEventHandler PriceChanged;
        public event AcceptButtonClickEventHandler AcceptButtonClick;
        public event CancelButtonClickEventHandler CancelButtonClick;

        private void NameBox_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            NameChanged?.Invoke(sender, e);
        }

        private void CategoryComboBox_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            CategoryChanged?.Invoke(sender, e);
        }

        private void BrandComboBox_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            BrandChanged?.Invoke(sender, e);
        }

        private void PriceBox_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            PriceChanged?.Invoke(sender, e);
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
