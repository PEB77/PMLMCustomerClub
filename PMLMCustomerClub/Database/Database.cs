using DevExpress.Data.Entity;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMLMCustomerClub.Database
{
    public abstract class Database<T>
    {
        protected static string ConnectionString { get; set; } = ConfigurationManager.ConnectionStrings["PMLMCustomerClub.Properties.Settings.BaseDatabaseConnectionString"].ConnectionString;
        public abstract DataTable GetData();
        public abstract void Insert(T item);
        public abstract void Update(T item);
        public abstract void Delete(int ID);
        public abstract T Explore(int ID);
        public abstract bool TryExplore(int ID, out T result);
        public abstract int GetNextID();
    }
}
