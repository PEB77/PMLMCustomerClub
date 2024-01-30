using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using PMLMCustomerClub.View;

namespace PMLMCustomerClub.Code
{
    public static class CustomerDatabase
    {
        private const string ConnectString = "server=localhost;uid=root;pwd=Abtin1998;database=pmlm_customer_club";

        public static DataTable GetDataTable()
        {
            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            string query = "SELECT * FROM pmlm_customer_club.customer_list";
            MySqlCommand command = new MySqlCommand(query, con);

            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable dataTable = new DataTable("customer_list");

            adapter.Fill(dataTable);
            con.Close();
            return dataTable;
        }
        public static void InsertNewRow(Customer customer)
        {
            string query = "INSERT INTO customer_list (customer_id, customer_firstname, customer_lastname, customer_phone_number, customer_birthday, customer_referral_code, customer_credit, address, customer_filepath) VALUES ";

            string value0 = customer.ID.ToString();
            string value1 = customer.FirstName;
            string value2 = customer.LastName;
            string value3 = customer.PhoneNumber;
            string value4 = customer.BirthDay.Year.ToString() + "-" + customer.BirthDay.Month.ToString() + "-" +
                customer.BirthDay.Day.ToString();
            string value5 = customer.ReferralCode.ToString();
            string value6 = customer.Credit.ToString();
            string value7 = customer.Address.ToString();
            string value8 = customer.FolderName;


            query += $"('{value0}', '{value1}', '{value2}', '{value3}', '{value4}', '{value5}', '{value6}', '{value7}', '{value8}');"; // Add values to the SQL statement
            
            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            MySqlCommand command = new MySqlCommand(query, con);
            command.ExecuteNonQuery();
            con.Close();
        }
        public static void UpdateRow(Customer customer)
        {
            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            string quary = "UPDATE customer_list SET customer_firstname = @value0, customer_lastname = @value1, customer_phone_number = @value2, customer_birthday = @value3, customer_referral_code = @value4, customer_credit = @value5, address = @value6, customer_filepath = @value7 WHERE customer_id = @id";

            string value0 = customer.FirstName;
            string value1 = customer.LastName;
            string value2 = customer.PhoneNumber;
            string value3 = customer.BirthDay.Year.ToString() + "-" + customer.BirthDay.Month.ToString() + "-" +
                customer.BirthDay.Day.ToString();
            string value4 = customer.ReferralCode.ToString();
            string value5 = customer.Credit.ToString();
            string value6 = customer.Address.ToString();
            string value7 = customer.FolderName;
            string id = customer.ID.ToString();

            using (MySqlCommand cmd = new MySqlCommand(quary, con))
            {
                cmd.Parameters.AddWithValue("@value0", value0);
                cmd.Parameters.AddWithValue("@value1", value1);
                cmd.Parameters.AddWithValue("@value2", value2);
                cmd.Parameters.AddWithValue("@value3", value3);
                cmd.Parameters.AddWithValue("@value4", value4);
                cmd.Parameters.AddWithValue("@value5", value5);
                cmd.Parameters.AddWithValue("@value6", value6);
                cmd.Parameters.AddWithValue("@value7", value7);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }

            con.Close();

        }

        public static bool TryLookUp(int id, out Customer customer)
        {
            customer = new Customer();
            bool findCustomerProfile = false;

            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            string quary = $"SELECT * FROM pmlm_customer_club.customer_list WHERE customer_id = {id}";
            MySqlCommand command = new MySqlCommand(quary, con);

            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable dataTable = new DataTable("customer_list");

            adapter.Fill(dataTable);
            con.Close();

            if (dataTable.Rows.Count > 0)
            {
                DataRow dataRow = dataTable.Rows[0];
                customer = Customer.GetCustomer(dataRow);
                return findCustomerProfile = true;
            }
            return findCustomerProfile = false;
        }
        public static bool TryLookUp(string firstName, string lastName, out Customer customer)
        {
            customer = new Customer();
            bool findCustomerProfile = false;

            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            string quary = $"SELECT * FROM pmlm_customer_club.customer_list WHERE customer_firstname = '{firstName}' AND customer_lastname = '{lastName}'";
            MySqlCommand command = new MySqlCommand(quary, con);

            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable dataTable = new DataTable("customer_list");

            adapter.Fill(dataTable);
            con.Close();

            if (dataTable.Rows.Count > 0)
            {
                DataRow dataRow = dataTable.Rows[0];
                customer = Customer.GetCustomer(dataRow);
                return findCustomerProfile = true;
            }
            return findCustomerProfile = false;
        }
        public static void DeleteRow(int id)
        {
            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            string quary = "DELETE FROM customer_list WHERE customer_id = @id";
            using (MySqlCommand command = new MySqlCommand(quary, con))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
            con.Close();
        }

        internal static void UpdateCredit(int customerID, int orderPrice, bool isIncrese = true)
        {
            //Customer customer;
            //TryLookUp(customerID, out customer);
            //if (isIncrese)
            //    customer.Credit += (int)(orderPrice * 0.1);
            //else
            //    customer.Credit -= (int)(orderPrice * 0.1);
            //UpdateRow(customer);
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
