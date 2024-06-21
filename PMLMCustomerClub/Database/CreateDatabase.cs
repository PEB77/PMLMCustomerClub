using PMLMCustomerClub.Manager;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace PMLMCustomerClub.Database
{
    public class CreateDatabase
    {
        protected static string ConnectionString { get; set; } = "Data Source = " + Path.Combine(System.Windows.Forms.Application.StartupPath, "Database", "PMLMCustomerDatabase.db");

        private void CreateDatabaseFile()
        {
            FileStream stream = File.Create(FileManager.DatabaseFilePath);
            stream.Close();
        }

        private void SQLiteQuery(string query)
        {
            using (SQLiteConnection con = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    con.Open();
                    using (SQLiteCommand command = new SQLiteCommand(query, con))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        command.ExecuteNonQuery();
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        } 

        private void CreateProductTable()
        {
            string query = "CREATE TABLE [Product]([ProductID] INT PRIMARY KEY NOT NULL UNIQUE, [ProductName] CHAR(120) NOT NULL UNIQUE, [Category] CHAR(30) NOT NULL, [Brand] CHAR(30) NOT NULL, [Price] INT NOT NULL);";
            SQLiteQuery(query);
        }

        private void CreateStoreTable()
        {
            string query = "CREATE TABLE [Store]([StoreID] INT PRIMARY KEY NOT NULL, [ProductID] INT NOT NULL REFERENCES [Product]([ProductID]), [ProductName] CHAR(120) NOT NULL, [Category] CHAR(30) NOT NULL, [Brand] CHAR(30) NOT NULL, [Price] INT NOT NULL, [ExpDate] DATETEXT NOT NULL, [Stock] INT NOT NULL);";
            SQLiteQuery(query);
        }

        private void CreateCustomerTable()
        {
            string query = "CREATE TABLE [Customer](\r\n  [CustomerID] INT NOT NULL UNIQUE, \r\n  [FirstName] CHAR(60) DEFAULT Empty, \r\n  [LastName] CHAR(60) DEFAULT Empty, \r\n  [PhoneNumber] CHAR(11) DEFAULT Empty, \r\n  [Birthday] DATETEXT, \r\n  [ReferralCode] INT DEFAULT 0, \r\n  [Credit] INT DEFAULT 0, \r\n  [Address] TEXT DEFAULT Empty, \r\n  [FolderName] TEXT NOT NULL UNIQUE, \r\n  PRIMARY KEY([CustomerID], [FolderName]));";
            SQLiteQuery(query);
        }

        private void CreateOrderTable()
        {
            string query = "CREATE TABLE [OrderList](\r\n  [OrderID] INT PRIMARY KEY NOT NULL UNIQUE, \r\n  [OrderDate] DATETEXT NOT NULL, \r\n  [CustomerID] INT NOT NULL REFERENCES [Customer]([CustomerID]), \r\n  [CustomerFirstName] CHAR(60) NOT NULL, \r\n  [CustomerLastName] CHAR(60), \r\n  [OrderPrice] INT NOT NULL, \r\n  [FileName] TEXT NOT NULL);";
            SQLiteQuery(query);
        }

        public Task Run()
        {
            return Task.Run(() =>
            {
                CreateDatabaseFile();
                CreateProductTable();
                CreateStoreTable();
                CreateCustomerTable();
                CreateOrderTable();
            });
        }
    }
}
