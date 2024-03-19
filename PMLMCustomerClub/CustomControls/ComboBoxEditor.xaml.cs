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

namespace PMLMCustomerClub.CustomControls
{
    /// <summary>
    /// Interaction logic for ComboBoxEditor.xaml
    /// </summary>
    public partial class ComboBoxEditor : UserControl
    {
        public ComboBoxEditor()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public string Title { get; set; }
        public double EditorWidth { get; set; } = 150;
        public object EditorSource 
        {
            get => Editor.ItemsSource;
            set => Editor.ItemsSource = value;
        }
        public bool EditorEnable { get; set; } = true;
        public object SelectedItem 
        {
            get => Editor.SelectedItem;
            set => Editor.SelectedItem = value;
        }
        public int SelectedIndex
        {
            get => Editor.SelectedIndex;
            set => Editor.SelectedIndex = value;
        }
        public string Text
        {
            get => Editor.Text;
            set => Editor.Text = value;
        }

        public delegate void EditValueChangedEventHandler(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e);
        public event EditValueChangedEventHandler EditValueChanged;
        private void Editor_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            EditValueChanged?.Invoke(sender, e);
        }

    }
}
