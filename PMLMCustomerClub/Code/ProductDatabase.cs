using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace PMLMCustomerClub.Code
{
    public static class ProductDatabase
    {
        private const string ConnectString = "server=localhost;uid=root;pwd=Abtin1998;database=pmlm_customer_club";

        public static DataTable GetDataTable()
        {
            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            string query = "SELECT * FROM pmlm_customer_club.product";
            MySqlCommand cmd = new MySqlCommand(query, con);

            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable dataTable = new DataTable("product");

            adapter.Fill(dataTable);
            con.Close();
            return dataTable;
        }
        public static void InsertNewRow(Product info)
        {
            string query = "INSERT INTO product (product_id, product_name, product_category, product_brand, product_price) VALUES ";

            string value1 = info.ProductID.ToString();
            string value2 = info.ProductName;
            string value3 = info.Category.ToString().Replace("_", " ");
            string value4 = info.Brand.ToString().Replace("_", " ");
            string value5 = info.Price.ToString();

            query += $"('{value1}', '{value2}', '{value3}', '{value4}', '{value5}'),"; // Add values to the SQL statement

            query = query.TrimEnd(',') + ";"; // Remove the trailing comma and add a semicolon

            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand(query, con))
                cmd.ExecuteNonQuery();
            con.Close();

        }
        public static void UpdateRow(Product info)
        {

            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            string query = "UPDATE product SET product_name = @value1, product_category = @value2, product_brand = @value3, product_price = @value4 WHERE product_id = @id";

            string value1 = info.ProductID.ToString();
            string value2 = info.ProductName;
            string value3 = info.Category.ToString().Replace("_", " ");
            string value4 = info.Brand.ToString().Replace("_", " ");
            string value5 = info.Price.ToString();

            using (MySqlCommand cmd = new MySqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@value1", value2);
                cmd.Parameters.AddWithValue("@value2", value3);
                cmd.Parameters.AddWithValue("@value3", value4);
                cmd.Parameters.AddWithValue("@value4", value5);
                cmd.Parameters.AddWithValue("@id", value1);

                cmd.ExecuteNonQuery();
            }

            con.Close();
        }
        public static void DeleteRow(int id)
        {
            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            string query = "DELETE FROM product WHERE product_id = @id";

            using (MySqlCommand cmd = new MySqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
            con.Close();
        }
        public static int GetNextID()
        {
            DataTable dataTable = GetDataTable();

            DataRow row = dataTable.Rows[dataTable.Rows.Count - 1];
            int nextID = int.Parse(row[0].ToString()) + 1;
            return nextID;
        }

    }
}
