using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using PMLMCustomerClub.View;
using PMLMCustomerClub.Code;
using System.Data.SqlClient;

namespace PMLMCustomerClub.Database
{
    public class CustomerDatabase : Database<Customer>
    {
        
        public override DataTable GetData()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                try
                {
                    con.Open();
                    string query = "SELECT * Customer";
                    SqlCommand command = new SqlCommand(query, con);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable("customer_list");
                    adapter.Fill(dataTable);
                    con.Close();
                    return dataTable;
                }
                catch
                {
                    throw new Exception();
                }
            }
        }

        public override void Insert(Customer item)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                try
                {
                    string query = "INSERT INTO Customer (CustomerID, FirstName, LastName, PhoneNumber, Birthday, ReferralCode, Credit, Address, FolderName) VALUES ";
                    string value0 = item.ID.ToString();
                    string value1 = item.FirstName;
                    string value2 = item.LastName;
                    string value3 = item.PhoneNumber;
                    string value4 = item.BirthDay.Year.ToString() + "-" + item.BirthDay.Month.ToString() + "-" +
                        item.BirthDay.Day.ToString();
                    string value5 = item.ReferralCode.ToString();
                    string value6 = item.Credit.ToString();
                    string value7 = item.Address.ToString();
                    string value8 = item.FolderName;
                    query += $"('{value0}', '{value1}', '{value2}', '{value3}', '{value4}', '{value5}', '{value6}', '{value7}', '{value8}');"; // Add values to the SQL statement
                    con.Open();
                    using (SqlCommand command = new SqlCommand(query, con))
                        command.ExecuteNonQuery();
                    con.Close();
                }
                catch
                {
                    throw new Exception();
                }
            }
        }

        public override void Update(Customer item)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                try
                {
                    con.Open();
                    string quary = "UPDATE Customer SET FirstName = @value0, LastName = @value1, PhoneNumber = @value2, Birthday = @value3, ReferralCode = @value4, Credit = @value5, Address = @value6, FolderName = @value7 WHERE CustomerID = @id";
                    string value0 = item.FirstName;
                    string value1 = item.LastName;
                    string value2 = item.PhoneNumber;
                    string value3 = item.BirthDay.Year.ToString() + "-" + item.BirthDay.Month.ToString() + "-" +
                        item.BirthDay.Day.ToString();
                    string value4 = item.ReferralCode.ToString();
                    string value5 = item.Credit.ToString();
                    string value6 = item.Address.ToString();
                    string value7 = item.FolderName;
                    string id = item.ID.ToString();
                    using (SqlCommand command = new SqlCommand(quary, con))
                    {
                        command.Parameters.AddWithValue("@value0", value0);
                        command.Parameters.AddWithValue("@value1", value1);
                        command.Parameters.AddWithValue("@value2", value2);
                        command.Parameters.AddWithValue("@value3", value3);
                        command.Parameters.AddWithValue("@value4", value4);
                        command.Parameters.AddWithValue("@value5", value5);
                        command.Parameters.AddWithValue("@value6", value6);
                        command.Parameters.AddWithValue("@value7", value7);
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                    con.Close();
                }
                catch
                {
                    throw new Exception();
                }
            }
        }

        public override void Delete(int ID)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                try
                {
                    con.Open();
                    string quary = "DELETE FROM Customer WHERE CustomerID = @id";
                    using (SqlCommand command = new SqlCommand(quary, con))
                    {
                        command.Parameters.AddWithValue("@id", ID);
                        command.ExecuteNonQuery();
                    }
                    con.Close();
                }
                catch
                {
                    throw new Exception();
                }
            }
        }

        public override Customer Explore(int ID)
        {
            throw new NotImplementedException();
        }

        public override bool TryExplore(int ID, out Customer result)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                try
                {
                    result = new Customer();
                    bool findCustomerProfile = false;
                    con.Open();
                    string quary = $"SELECT * FROM Customer WHERE CustomerID = {ID}";
                    SqlCommand command = new SqlCommand(quary, con);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable("customer_list");
                    adapter.Fill(dataTable);
                    con.Close();
                    if (dataTable.Rows.Count > 0)
                    {
                        DataRow dataRow = dataTable.Rows[0];
                        result = Customer.GetCustomer(dataRow);
                        return findCustomerProfile = true;
                    }
                    return findCustomerProfile = false;
                }
                catch
                {
                    throw new Exception();
                }
            }
        }
        public bool TryExplore(string firstName, string lastName, out Customer customer)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                try
                {
                    customer = new Customer();
                    bool findCustomerProfile = false;
                    con.Open();
                    string quary = $"SELECT * FROM Customer WHERE FirstName = '{firstName}' AND LastName = '{lastName}'";
                    SqlCommand command = new SqlCommand(quary, con);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
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
                catch
                {
                    throw new Exception();
                }
            }
        }

        public override int GetNextID()
        {
            DataTable dataTable = GetData();
            DataRow row = dataTable.Rows[dataTable.Rows.Count - 1];
            int nextID = int.Parse(row[0].ToString()) + 1;
            return nextID;
        }

        internal void UpdateCredit(int customerID, int orderPrice, bool isIncrese = true)
        {
            Customer customer;
            TryExplore(customerID, out customer);
            if (isIncrese)
                customer.Credit += (int)(orderPrice * 0.1);
            else
                customer.Credit -= (int)(orderPrice * 0.1);
            Update(customer);
        }

    }
}
