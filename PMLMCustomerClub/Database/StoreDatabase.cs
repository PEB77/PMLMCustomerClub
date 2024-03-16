using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using PMLMCustomerClub.View;
using System.IO;
using PMLMCustomerClub.Model;
using System.Data.SQLite;
using DevExpress.Printing.Utils.DocumentStoring;
using System.Xml.Linq;
using ZstdSharp;

namespace PMLMCustomerClub.Database
{
    public class StoreDatabase : Database<StoreItem>
    {
        private const string FileName = "\\StoreItems.csv";

        private static string ConvertDateTime(DateTime dt)
        {
            string result = "";

            result = dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day.ToString() +
                " " + dt.Hour.ToString() + ":" + dt.Minute.ToString() + ":" + dt.Second.ToString();

            return result;

        }

        public void Save(string filePath)
        {
            DataTable table = GetData();

            StringBuilder sb = new StringBuilder();

            foreach (DataColumn column in table.Columns)
            {
                sb.Append(column.ColumnName + ",");
            }

            sb.Remove(sb.Length - 1, 1);
            sb.Append(Environment.NewLine);

            foreach (DataRow row in table.Rows)
            {
                foreach (DataColumn column in table.Columns)
                {
                    sb.Append(row[column].ToString() + ",");
                }

                sb.Remove(sb.Length - 1, 1);
                sb.Append(Environment.NewLine);
            }

            filePath += FileName;

            File.WriteAllText(filePath, sb.ToString());
        }

        public override DataTable GetData()
        {
            using (SQLiteConnection con = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    con.Open();
                    string query = "SELECT * FROM Store";
                    SQLiteCommand command = new SQLiteCommand(query, con);
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    DataTable dataTable = new DataTable("store");
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

        public override void Insert(StoreItem item)
        {
            using (SQLiteConnection con = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    con.Open();
                    string query = "INSERT INTO Store (StoreID, ProductID, ProductName, Category, Brand, Price, ExpDate, Stock) VALUES ";
                    string value0 = item.StoreID.ToString();
                    string value1 = item.ProductID.ToString();
                    string value2 = item.ProductName;
                    string value3 = item.Category.ToString().Replace("_", " ");
                    string value4 = item.Brand.ToString().Replace("_", " ");
                    string value5 = item.Price.ToString();
                    string value6 = item.ExpDate.Year.ToString() + "-" + item.ExpDate.Month.ToString() + "-" + item.ExpDate.Day.ToString() +
                        " " + item.ExpDate.Hour.ToString() + ":" + item.ExpDate.Minute.ToString() + ":" + item.ExpDate.Second.ToString();
                    string value7 = item.Amount.ToString();
                    query += $"('{value0}', '{value1}', '{value2}', '{value3}', '{value4}', '{value5}', '{value6}', '{value7}');";
                    using (SQLiteCommand command = new SQLiteCommand(query, con))
                        command.ExecuteNonQuery();
                    con.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public override void Update(StoreItem item)
        {
            using (SQLiteConnection con = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    con.Open();
                    string updateSql = "UPDATE Store SET ProductID = @value0, ProductName = @value1, Category = @value2, Brand = @value3, Price = @value4, ExpDate = @value5, Stock = @value6 WHERE StoreID = @id";
                    string value0 = item.StoreID.ToString();
                    string value1 = item.ProductID.ToString();
                    string value2 = item.ProductName;
                    string value3 = item.Category.ToString().Replace("_", " ");
                    string value4 = item.Brand.ToString().Replace("_", " ");
                    string value5 = item.Price.ToString();
                    string value6 = item.ExpDate.Year.ToString() + "-" + item.ExpDate.Month.ToString() + "-" + item.ExpDate.Day.ToString() +
                        " " + item.ExpDate.Hour.ToString() + ":" + item.ExpDate.Minute.ToString() + ":" + item.ExpDate.Second.ToString();
                    string value7 = item.Amount.ToString();

                    using (SQLiteCommand command = new SQLiteCommand(updateSql, con))
                    {
                        command.Parameters.AddWithValue("@value0", value1);
                        command.Parameters.AddWithValue("@value1", value2);
                        command.Parameters.AddWithValue("@value2", value3);
                        command.Parameters.AddWithValue("@value3", value4);
                        command.Parameters.AddWithValue("@value4", value5);
                        command.Parameters.AddWithValue("@value5", value6);
                        command.Parameters.AddWithValue("@value6", value7);
                        command.Parameters.AddWithValue("@id", value0);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        public void Update(List<StoreItem> items)
        {
            foreach (StoreItem p in items)
            {
                StoreItem storeProduct = Explore(p.StoreID);
                storeProduct.Amount -= p.Amount;
                if (storeProduct.Amount == 0)
                    Delete(storeProduct.StoreID);
                else
                    Update(storeProduct);
            }
        }

        public override void Delete(int ID)
        {
            using (SQLiteConnection con = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    con.Open();
                    string deleteSql = "DELETE FROM Store WHERE StoreID = @id";
                    using (SQLiteCommand command = new SQLiteCommand(deleteSql, con))
                    {
                        command.Parameters.AddWithValue("@id", ID);
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

        public override StoreItem Explore(int storeID)
        {
            using (SQLiteConnection con = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    con.Open();
                    string query = $"SELECT * FROM Store WHERE StoreID = {storeID}";
                    SQLiteCommand command = new SQLiteCommand(query, con);
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    DataTable dataTable = new DataTable("store");
                    adapter.Fill(dataTable);
                    con.Close();
                    StoreItem product = new StoreItem();
                    if (dataTable.Rows.Count != 0)
                    {
                        DataRow dataRow = dataTable.Rows[0];
                        int id = int.Parse(dataRow[0].ToString());
                        int productID = int.Parse(dataRow[1].ToString());
                        string name = dataRow[2].ToString();
                        Product.Categories category = (Product.Categories)Enum.Parse(typeof(Product.Categories), dataRow[3].ToString().Replace(" ", "_"));
                        Product.Brands brand = (Product.Brands)Enum.Parse(typeof(Product.Brands), dataRow[4].ToString().Replace(" ", "_"));
                        int price = int.Parse(dataRow[5].ToString());
                        DateTime expDate = Convert.ToDateTime(dataRow[6].ToString());
                        int amount = int.Parse(dataRow[7].ToString());
                        product = new StoreItem()
                            .SetStoreID(id)
                            .SetProductData(productID, name, category, brand, price)
                            .SetOtherDetaild(expDate, amount);
                    }
                    return product;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        public DataTable Explore(string name)
        {
            using (SQLiteConnection con = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    con.Open();
                    string query = $"SELECT * FROM Store WHERE ProductName = '{name}';";
                    SQLiteCommand command = new SQLiteCommand(query, con);
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    DataTable dataTable = new DataTable("store");
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
        public StoreItem Explore(StoreItem product)
        {
            using (SQLiteConnection con = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    con.Open();
                    string dateTimeString = ConvertDateTime(product.ExpDate);
                    string quary = $"SELECT * FROM Store WHERE product_id = '{product.ProductID}' AND product_expdate = '{dateTimeString}'; ";
                    SQLiteCommand command = new SQLiteCommand(quary, con);
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    DataTable dataTable = new DataTable("store");
                    adapter.Fill(dataTable);
                    con.Close();
                    StoreItem newProduct = new StoreItem();
                    if (dataTable.Rows.Count != 0)
                    {
                        DataRow dataRow = dataTable.Rows[0];
                        int id = int.Parse(dataRow[0].ToString());
                        int productID = int.Parse(dataRow[1].ToString());
                        string name = dataRow[2].ToString();
                        Product.Categories category = (Product.Categories)Enum.Parse(typeof(Product.Categories), dataRow[3].ToString().Replace(" ", "_"));
                        Product.Brands brand = (Product.Brands)Enum.Parse(typeof(Product.Brands), dataRow[4].ToString().Replace(" ", "_"));
                        int price = int.Parse(dataRow[5].ToString());
                        DateTime expDate = (DateTime)dataRow[6];
                        int amount = int.Parse(dataRow[7].ToString());
                        newProduct = new StoreItem()
                            .SetStoreID(id)
                            .SetProductData(productID, name, category, brand, price)
                            .SetOtherDetaild(expDate, amount);
                    }
                    return newProduct;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public override bool TryExplore(int ID, out StoreItem result)
        {
            throw new NotImplementedException();
        }
        public bool TryExplore(string name, out List<StoreItem> items)
        {
            using (SQLiteConnection con = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    con.Open();
                    items = new List<StoreItem>();
                    bool findCustomerProfile = false;
                    string quary = $"SELECT * FROM Store WHERE ProductName = '{name}'";
                    SQLiteCommand command = new SQLiteCommand(quary, con);
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    DataTable table = new DataTable("customer_list");
                    adapter.Fill(table);
                    con.Close();
                    if (table.Rows.Count > 0)
                    {
                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            DataRow row = table.Rows[i];
                            items.Add(StoreItem.GetStoreItem(row));
                        }
                        return findCustomerProfile = true;
                    }
                    return findCustomerProfile = false;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        internal void Cancel(List<StoreItem> products)
        {
            foreach (StoreItem p in products)
            {
                StoreItem storeIDSearch = Explore(p.StoreID);
                StoreItem otherProductSearch = Explore(p);
                if (storeIDSearch.StoreID == 0)
                {
                    Insert(p);
                }
                else
                {
                    if (storeIDSearch.StoreID == otherProductSearch.StoreID)
                    {
                        storeIDSearch.Amount += p.Amount;
                        Update(storeIDSearch);
                    }
                    else
                    {
                        DataTable dataTable = GetData();
                        p.StoreID = dataTable.Rows.Count + 1;
                        Insert(p);
                    }
                }

            }
        }
    }
}
