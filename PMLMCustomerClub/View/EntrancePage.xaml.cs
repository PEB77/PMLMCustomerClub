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

namespace PMLMCustomerClub.View
{
    /// <summary>
    /// Interaction logic for EntrancePage.xaml
    /// </summary>
    public partial class EntrancePage : Page
    {
        public EntrancePage(Frame frame)
        {
            InitializeComponent();
            Frame = frame;
            
        }

        public ProjectManager Manager { get; set; } = new ProjectManager();
        private Frame Frame;
        private int x = 5;

        private void GoToMainPage(ProjectManager.SelectPart part)
        {
            Manager.Part = part;
            Frame.Content = new MainPage(Manager);
        }

        private void CustomerTaleButton_Click(object sender, RoutedEventArgs e)
        {
            GoToMainPage(ProjectManager.SelectPart.CUSTOMER);
        }

        private void OrderTaleButton_Click(object sender, RoutedEventArgs e)
        {
            GoToMainPage(ProjectManager.SelectPart.ORDER);
        }

        private void StoreTaleButton_Click(object sender, RoutedEventArgs e)
        {
            GoToMainPage(ProjectManager.SelectPart.STORE);
        }

        private void ProductTaleButton_Click(object sender, RoutedEventArgs e)
        {
            GoToMainPage(ProjectManager.SelectPart.PRODUCT);
        }
    }
}
