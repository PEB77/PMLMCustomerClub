using PMLMCustomerClub.CustomControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMLMCustomerClub.Manager
{
    public interface IManager<T1, T2>
    {

        public ProjectManager Manager { get; set; }
        public TableViewer Viewer { get; set; }

        public DataRow RowFocused { get; set; }
        public bool IsEditMode { get; set; }

        public T1 Page { get; set; }
        public T2 Item { get; set; }

        public void Viewer_TableSelectingRow(object sender, DevExpress.Xpf.Grid.CanSelectRowEventArgs e);
        public void Viewer_DeleteButtonClick(object sender, System.Windows.RoutedEventArgs e);
        public void Viewer_EditButtonClick(object sender, System.Windows.RoutedEventArgs e);
        public void Viewer_NewButtonClick(object sender, System.Windows.RoutedEventArgs e);

        public void InitNewObject();
        public void InitObject();
        public void InitPage();
        public void InitComponent();
        public void InitEvent();

        public void Page_CancelButtonClick(object sender, System.Windows.RoutedEventArgs e);
        public void Page_AcceptEditedProduct(object sender, System.Windows.RoutedEventArgs e);
        public void Page_AcceptNewProduct(object sender, System.Windows.RoutedEventArgs e);

        public void AcceptValidation();
    }
}
