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
    /// Interaction logic for TextEditor.xaml
    /// </summary>
    public partial class TextEditor : UserControl
    {
        public TextEditor()
        {
            InitializeComponent();
            this.DataContext = this;
            
        }

        public string Title { get; set; }
        public string TextValue { get; set; }
        public bool EditorEnable { get; set; } = true;
        public double EditorWidth { get; set; } = 150;
        
        public delegate void EditValueChangedEventHandler(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e);
        public event EditValueChangedEventHandler EditValueChanged;
        private void Editor_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            EditValueChanged?.Invoke(sender, e);
        }
    }
}
