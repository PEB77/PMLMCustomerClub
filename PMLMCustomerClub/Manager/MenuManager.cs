using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMLMCustomerClub.Database;
using System.Windows.Forms;
using PMLMCustomerClub.Model;

namespace PMLMCustomerClub.Manager
{
    public class MenuManager
    {
        public MenuManager(ProjectManager manager)
        {
            Manager = manager;
        }

        ProjectManager Manager { get; set; }

        public void MenuBarEventsHandler(MenuBarItems item)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            if (folderBrowser.ShowDialog() != DialogResult.OK) return;
            
            switch (item)
            {
                case MenuBarItems.EXPORT_STORE: Manager.StoreDatabase.Save(folderBrowser.SelectedPath, item.ToString().Replace("EXPORT_", "")); break;
                case MenuBarItems.EXPORT_ORDER: Manager.OrderDatabase.Save(folderBrowser.SelectedPath, item.ToString().Replace("EXPORT_", "")); break;
                case MenuBarItems.EXPORT_PRODUCT: Manager.ProductDatabase.Save(folderBrowser.SelectedPath, item.ToString().Replace("EXPORT_", "")); break;
                case MenuBarItems.EXPORT_CUSTOMER: Manager.CustomerDatabase.Save(folderBrowser.SelectedPath, item.ToString().Replace("EXPORT_", "")); break;
            }
        }

    }
}
