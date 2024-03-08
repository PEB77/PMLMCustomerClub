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
using DevExpress.Xpf.Grid;
using PMLMCustomerClub.CustomControls;

namespace PMLMCustomerClub.CustomControls
{
    /// <summary>
    /// Interaction logic for TableViewer.xaml
    /// </summary>
    public partial class TableViewer : UserControl
    {
        public TableViewer()
        {
            InitializeComponent();
        }

        public delegate void NewButtonClickEventHandler(object sender, RoutedEventArgs e);
        public delegate void EditButtonClickEventHandler(object sender, RoutedEventArgs e);
        public delegate void DeleteButtonClickEventHandler(object sender, RoutedEventArgs e);

        public delegate void TableSelectingRowEventHandler(object sender, DevExpress.Xpf.Grid.CanSelectRowEventArgs e);

        public event NewButtonClickEventHandler NewButtonClick;
        public event EditButtonClickEventHandler EditButtonClick;
        public event DeleteButtonClickEventHandler DeleteButtonClick;

        public event TableSelectingRowEventHandler TableSelectingRow;

        public object ItemSource
        {
            get => GridControl.ItemsSource;
            set => GridControl.ItemsSource = value;
        }

        public GridControl GridControlProp
        {
            get => GridControl;
            set => GridControl = value;
        }
        public Frame Frame
        {
            get => PageFrame;
            set => PageFrame = value;
        }
        public bool NewButtonEnable
        {
            get => NewButton.IsEnabled;
            set => NewButton.IsEnabled = value;
        }
        public bool EditButtonEnable
        {
            get => EditButton.IsEnabled;
            set => EditButton.IsEnabled = value;
        }
        public bool DeleteButtonEnable
        {
            get => DeleteButton.IsEnabled;
            set => DeleteButton.IsEnabled = value;
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            NewButtonClick?.Invoke(sender, e);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            EditButtonClick?.Invoke(sender, e);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteButtonClick?.Invoke(sender, e);
        }

        private void Table_CanSelectRow(object sender, DevExpress.Xpf.Grid.CanSelectRowEventArgs e)
        {
            TableSelectingRow?.Invoke(sender, e);
        }

    }
}
