using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using PMLMCustomerClub.View;
using System.IO;

namespace PMLMCustomerClub.Code
{
    public static class StoreDatabase
    {
        private const string ConnectString = "server=localhost;uid=root;pwd=Abtin1998;database=pmlm_customer_club";
        private const string FileName = "\\StoreItems.csv";

        public static DataTable GetDataTable()
        {
            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            string query = "SELECT * FROM pmlm_customer_club.store";
            MySqlCommand cmd = new MySqlCommand(query, con);

            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable dataTable = new DataTable("store");

            adapter.Fill(dataTable);
            con.Close();
            return dataTable;
        }
        public static void InsertNewRow(StoreItem product)
        {
            string query = "INSERT INTO store (store_id, product_id, product_name, product_category, product_brand, product_price, product_expdate, store_stock) VALUES ";

            string value0 = product.StoreID.ToString();
            string value1 = product.ProductID.ToString();
            string value2 = product.ProductName;
            string value3 = product.Category.ToString().Replace("_", " ");
            string value4 = product.Brand.ToString().Replace("_", " ");
            string value5 = product.Price.ToString();
            string value6 = product.ExpDate.Year.ToString() + "-" + product.ExpDate.Month.ToString() + "-" + product.ExpDate.Day.ToString() +
                " " + product.ExpDate.Hour.ToString() + ":" + product.ExpDate.Minute.ToString() + ":" + product.ExpDate.Second.ToString();
            string value7 = product.Amount.ToString();

            query += $"('{value0}', '{value1}', '{value2}', '{value3}', '{value4}', '{value5}', '{value6}', '{value7}'),"; // Add values to the SQL statement

            query = query.TrimEnd(',') + ";"; // Remove the trailing comma and add a semicolon

            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand(query, con))
                cmd.ExecuteNonQuery();
            con.Close();
        }
        public static void UpdateRow(StoreItem item)
        {
            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            string updateSql = "UPDATE store SET product_id = @value0, product_name = @value1, product_category = @value2, product_brand = @value3, product_price = @value4, product_expdate = @value5, store_stock = @value6 WHERE store_id = @id";

            string value0 = item.StoreID.ToString();
            string value1 = item.ProductID.ToString();
            string value2 = item.ProductName;
            string value3 = item.Category.ToString().Replace("_", " ");
            string value4 = item.Brand.ToString().Replace("_", " ");
            string value5 = item.Price.ToString();
            string value6 = item.ExpDate.Year.ToString() + "-" + item.ExpDate.Month.ToString() + "-" + item.ExpDate.Day.ToString() +
                " " + item.ExpDate.Hour.ToString() + ":" + item.ExpDate.Minute.ToString() + ":" + item.ExpDate.Second.ToString();
            string value7 = item.Amount.ToString();

            using (MySqlCommand cmd = new MySqlCommand(updateSql, con))
            {
                cmd.Parameters.AddWithValue("@value0", value1);
                cmd.Parameters.AddWithValue("@value1", value2);
                cmd.Parameters.AddWithValue("@value2", value3);
                cmd.Parameters.AddWithValue("@value3", value4);
                cmd.Parameters.AddWithValue("@value4", value5);
                cmd.Parameters.AddWithValue("@value5", value6);
                cmd.Parameters.AddWithValue("@value6", value7);
                cmd.Parameters.AddWithValue("@id", value0);

                cmd.ExecuteNonQuery();
            }
        }
        public static DataTable LookUp(string name)
        {
            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            string query = $"SELECT * FROM pmlm_customer_club.store WHERE product_name = '{name}';";
            MySqlCommand command = new MySqlCommand(query, con);

            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable dataTable = new DataTable("store");

            adapter.Fill(dataTable);
            con.Close();
            return dataTable;
        }
        public static StoreItem LookUp(int storeID)
        {
            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            string query = $"SELECT * FROM pmlm_customer_club.store WHERE store_id = {storeID}";
            MySqlCommand command = new MySqlCommand(query, con);

            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
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
                DateTime expDate = (DateTime)dataRow[6];
                int amount = int.Parse(dataRow[7].ToString());
                product = new StoreItem()
                    .SetStoreID(id)
                    .SetProductData(productID, name, category, brand, price)
                    .SetOtherDetaild(expDate, amount);
            }


            return product;
        }
        public static StoreItem LookUp(StoreItem product)
        {
            string dateTimeString = ConvertDateTime(product.ExpDate);
            string quary = $"SELECT * FROM pmlm_customer_club.store WHERE product_id = '{product.ProductID}' AND product_expdate = '{dateTimeString}'; ";

            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            MySqlCommand command = new MySqlCommand(quary, con);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
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

        public static bool TryLookUp(string name, out List<StoreItem> items)
        {
            items = new List<StoreItem>();
            bool findCustomerProfile = false;

            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            string quary = $"SELECT * FROM pmlm_customer_club.store WHERE product_name = '{name}'";
            MySqlCommand command = new MySqlCommand(quary, con);

            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
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

        public static void DeleteRow(int storeID)
        {
            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            string deleteSql = "DELETE FROM store WHERE store_id = @id";

            using (MySqlCommand deleteCommand = new MySqlCommand(deleteSql, con))
            {
                deleteCommand.Parameters.AddWithValue("@id", storeID);

                deleteCommand.ExecuteNonQuery();
            }
            con.Close();
        }

        internal static void UpdateStore(List<StoreItem> products)
        {
            foreach (StoreItem p in products)
            {
                StoreItem storeProduct = LookUp(p.StoreID);
                storeProduct.Amount -= p.Amount;

                if (storeProduct.Amount == 0)
                    DeleteRow(storeProduct.StoreID);

                else
                    UpdateRow(storeProduct);
                
            }
        }
        internal static void InsertCanclation(List<StoreItem> products)
        {
            foreach (StoreItem p in products)
            {
                StoreItem storeIDSearch = LookUp(p.StoreID);
                StoreItem otherProductSearch = LookUp(p);
                if (storeIDSearch.StoreID == 0)
                {
                    InsertNewRow(p);
                }
                else
                {
                    if (storeIDSearch.StoreID == otherProductSearch.StoreID)
                    {
                        storeIDSearch.Amount += p.Amount;
                        UpdateRow(storeIDSearch);
                    }
                    else
                    {
                        DataTable dataTable = GetDataTable();
                        p.StoreID = dataTable.Rows.Count + 1;
                        InsertNewRow(p);
                    }
                }

            }
        }

        public static int GetNextID()
        {
            DataTable dataTable = GetDataTable();
            if (dataTable.Rows.Count == 0) return 1;
            DataRow row = dataTable.Rows[dataTable.Rows.Count - 1];
            int nextID = int.Parse(row[0].ToString()) + 1;
            return nextID;
        }

        private static string ConvertDateTime(DateTime dt)
        {
            string result = "";

            result = dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day.ToString() +
                " " + dt.Hour.ToString() + ":" + dt.Minute.ToString() + ":" + dt.Second.ToString();

            return result;

        }

        public static void SaveCsv(string filePath)
        {
            DataTable table = GetDataTable();

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

    }
}
