using PMLMCustomerClub.Manager;
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
using System.Windows.Threading;

namespace PMLMCustomerClub.View
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage(ProjectManager manager)
        {
            InitializeComponent();

            this.Manager = manager;
            this.Manager.Main = this;
            this.Manager.FirstInit();
        }

        ProjectManager Manager;
        public delegate void SelectionTabEventHandler(object sender, DevExpress.Xpf.Core.TabControlSelectionChangedEventArgs e);
        public event SelectionTabEventHandler SelectionTab;

        public delegate void ExportStoreItemsEventHandler(object sender, RoutedEventArgs e);
        public event ExportStoreItemsEventHandler ExportStore;

        public Dispatcher UIDispatcher = Dispatcher.CurrentDispatcher;


        private void MainTabControl_SelectionChanged(object sender, DevExpress.Xpf.Core.TabControlSelectionChangedEventArgs e)
        {
            SelectionTab?.Invoke(sender, e);
        }

        private void ExportStoreItems_Click(object sender, RoutedEventArgs e)
        {
            ExportStore?.Invoke(sender, e);
        }

    }
}
