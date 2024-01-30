using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace PMLMCustomerClub.Code
{
    public static class OrderDatabase
    {
        private const string ConnectString = "server=localhost;uid=root;pwd=Abtin1998;database=pmlm_customer_club";
        private const int ForReachingSameResult = 10;
        public static DataTable GetDataTable()
        {
            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            string quary = "SELECT * FROM pmlm_customer_club.orders_list";
            MySqlCommand commad = new MySqlCommand(quary, con);

            MySqlDataAdapter adapter = new MySqlDataAdapter(commad);
            DataTable dataTable = new DataTable("orders_list");

            adapter.Fill(dataTable);
            con.Close();
            return dataTable;
        }
        public static void InsertNewRow(Order order)
        {
            StoreDatabase.UpdateStore(order.Products);
            if (order.Customer.ReferralCode != 0)
                CustomerDatabase.UpdateCredit(order.Customer.ReferralCode, order.TotalPrice);

            if (order.UseCredit)
                CustomerDatabase.UpdateCredit(order.Customer.ID, order.CreditUsed * ForReachingSameResult, false);

            string quary = "INSERT INTO orders_list (order_id, order_date, customer_id, customer_firstname, customer_lastname, file_name, order_price) VALUES ";

            string value0 = order.ID.ToString();
            string value1 = ConvertDateTime(order.OrderDate);
            string value2 = order.Customer.ID.ToString();
            string value3 = order.Customer.FirstName;
            string value4 = order.Customer.LastName;
            string value5 = order.FileName;
            string value6 = order.TotalPrice.ToString();

            quary += $"('{value0}', '{value1}', '{value2}', '{value3}', '{value4}', '{value5}', '{value6}');";

            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            MySqlCommand command = new MySqlCommand(quary, con);
            command.ExecuteNonQuery();
            con.Close();
        }
        public static void UpdateRow(OrdersInfo order)
        {
            StoreDatabase.UpdateStore(order.Products);
            if (order.Customer.ReferralCode != 0)
                CustomerDatabase.UpdateCredit(order.Customer.ReferralCode, order.TotalPrice);

            if (order.UseCredit)
                CustomerDatabase.UpdateCredit(order.Customer.ID, order.CreditUsed * ForReachingSameResult, false);

            string quary = "UPDATE orders_list SET order_date = @value0, customer_id = @value1, customer_name = @value2, file_name = @value3, order_price = @value4 WHERE order_id = @id ";

            string value0 = ConvertDateTime(order.OrderDate);
            string value1 = order.Customer.ID.ToString();
            string value2 = order.Customer.Name;
            string value3 = order.FileName;
            string value4 = order.TotalPrice.ToString();
            string id = order.ID.ToString();

            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            using (MySqlCommand command = new MySqlCommand(quary, con))
            {
                command.Parameters.AddWithValue("@value0", value0);
                command.Parameters.AddWithValue("@value1", value1);
                command.Parameters.AddWithValue("@value2", value2);
                command.Parameters.AddWithValue("@value3", value3);
                command.Parameters.AddWithValue("@value4", value4);
                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();
            }

            con.Close();
        }
        public static void DeleteRow(OrdersInfo order, bool isDeleteProcessor = true)
        {
            StoreDatabase.InsertCanclation(order.Products);
            if (order.Customer.ReferralCode != 0)
                CustomerDatabase.UpdateCredit(order.Customer.ReferralCode, order.TotalPrice, false);

            if (isDeleteProcessor)
            {
                string quary = "DELETE FROM orders_list WHERE order_id = @id";

                MySqlConnection con = new MySqlConnection(ConnectString);
                con.Open();
                using (MySqlCommand command = new MySqlCommand(quary, con))
                {
                    command.Parameters.AddWithValue("@id", order.ID);
                    command.ExecuteNonQuery();
                }
                con.Close();
            }
        }

        private static string ConvertDateTime(DateTime dt)
        {
            string result = "";

            result = dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day.ToString() +
                " " + dt.Hour.ToString() + ":" + dt.Minute.ToString() + ":" + dt.Second.ToString();

            return result;

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
