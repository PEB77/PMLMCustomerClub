using DevExpress.Data.Entity;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace PMLMCustomerClub.Database
{
    public abstract class Database<T>
    {
        //protected static string ConnectionString { get; set; } = ConfigurationManager.ConnectionStrings["PMLMCustomerClub.Properties.Settings.BaseDatabaseConnectionString"].ConnectionString;
        protected static string ConnectionString { get; set; } = "Data Source = " + Path.Combine(System.Windows.Forms.Application.StartupPath,"Database","PMLMCustomerDatabase.db"); 
        public abstract DataTable GetData();
        public abstract void Insert(T item);
        public abstract void Update(T item);
        public abstract void Delete(int ID);
        public abstract T Explore(int ID);
        public abstract bool TryExplore(int ID, out T result);
        public virtual int GetNextID()
        {
            DataTable dataTable = GetData();
            int nextID = 1;
            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[dataTable.Rows.Count - 1];
                nextID = int.Parse(row[0].ToString()) + 1;
            }
            return nextID;
        }
    }
}
