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
    /// Interaction logic for SpinEditor.xaml
    /// </summary>
    public partial class SpinEditor : UserControl
    {
        public SpinEditor()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public string Title { get; set; }
        public decimal SpinValue { get; set; }
        public decimal Value
        {
            get => Editor.Value;
            set => Editor.Value = value;
        }
        public bool EditorEnable { get; set; } = true;
        public double EditorWidth { get; set; } = 150;
        public decimal MaxValue { get; set; } = decimal.MaxValue;
        public decimal MinValue { get; set; } = decimal.MinValue;
        public decimal Increment { get; set; } = 1;

        public delegate void EditValueChangedEventHandler(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e);
        public event EditValueChangedEventHandler EditValueChanged;
        private void Editor_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            EditValueChanged?.Invoke(sender, e);
        }
    }
}
