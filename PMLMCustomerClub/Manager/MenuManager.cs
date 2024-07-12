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
            if (item.ToString().Contains("EXPORT"))
            {
                ExportHandler(item);
            }
            else if (item.ToString().Contains("IMPORT"))
            {
                ImportHandler(item);
            }
        }

        private void ExportHandler(MenuBarItems item)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "csv files (*.csv)|*.csv";
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
                
                switch (item)
                {
                    case MenuBarItems.EXPORT_STORE: Manager.StoreDatabase.Save(saveFileDialog.FileName); break;
                    case MenuBarItems.EXPORT_PRODUCT: Manager.ProductDatabase.Save(saveFileDialog.FileName); break;
                    case MenuBarItems.EXPORT_CUSTOMER: Manager.CustomerDatabase.Save(saveFileDialog.FileName); break;
                }
            }
        }

        private void ImportHandler(MenuBarItems item)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "csv files (*.csv)|*.csv";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() != DialogResult.OK) return;

                switch (item)
                {
                    case MenuBarItems.IMPORT_STORE: Manager.StoreDatabase.Load(openFileDialog.FileName); break;
                    case MenuBarItems.IMPORT_PRODUCT: Manager.ProductDatabase. Load(openFileDialog.FileName); break;
                    case MenuBarItems.IMPORT_CUSTOMER: Manager.CustomerDatabase.Load(openFileDialog.FileName); break;
                }
            }
        }

    }
}
