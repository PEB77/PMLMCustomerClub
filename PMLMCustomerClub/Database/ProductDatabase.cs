using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DevExpress.Utils.About;
using MySql.Data.MySqlClient;
using PMLMCustomerClub.Model;

namespace PMLMCustomerClub.Database
{
    public class ProductDatabase : Database<Product>
    {
        
        public override DataTable GetData()
        {
            using (SQLiteConnection con = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    con.Open();
                    string query = "SELECT * FROM Product";
                    SQLiteCommand command = new SQLiteCommand(query, con);
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    DataTable dataTable = new DataTable("Product");
                    adapter.Fill(dataTable);
                    con.Close();
                    return dataTable;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            
        }

        public override void Insert(Product info)
        {
            using (SQLiteConnection con = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    con.Open();
                    string query = "INSERT INTO Product (ProductID, ProductName, Category, Brand, Price) VALUES ";
                    string value1 = info.ProductID.ToString();
                    string value2 = info.ProductName;
                    string value3 = info.Category.ToString().Replace("_", " ");
                    string value4 = info.Brand.ToString().Replace("_", " ");
                    string value5 = info.Price.ToString();
                    query += $"('{value1}', '{value2}', '{value3}', '{value4}', '{value5}');";
                    using (SQLiteCommand command = new SQLiteCommand(query, con))
                    {
                        command.CommandType = CommandType.Text;
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

        public override void Update(Product info)
        {
            using (SQLiteConnection con = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    con.Open();
                    string query = "UPDATE Product SET ProductName = @value1, Category = @value2, Brand = @value3, Price = @value4 WHERE ProductID = @id";
                    string value1 = info.ProductID.ToString();
                    string value2 = info.ProductName;
                    string value3 = info.Category.ToString().Replace("_", " ");
                    string value4 = info.Brand.ToString().Replace("_", " ");
                    string value5 = info.Price.ToString();
                    using (SQLiteCommand command = new SQLiteCommand(query, con))
                    {
                        command.Parameters.AddWithValue("@value1", value2);
                        command.Parameters.AddWithValue("@value2", value3);
                        command.Parameters.AddWithValue("@value3", value4);
                        command.Parameters.AddWithValue("@value4", value5);
                        command.Parameters.AddWithValue("@id", value1);
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

        public override void Delete(int id)
        {
            using (SQLiteConnection con = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    con.Open();
                    string query = "DELETE FROM Product WHERE ProductID = @id";
                    using (SQLiteCommand command = new SQLiteCommand(query, con))
                    {
                        command.Parameters.AddWithValue("@id", id);
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

        public override Product Explore(int ID)
        {
            throw new NotImplementedException();
        }

        public override bool TryExplore(int ID, out Product result)
        {
            throw new NotImplementedException();
        }


    }
}
