using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMLMCustomerClub.Model;

namespace PMLMCustomerClub.Database
{
    public class OrderDatabase : Database<Order>
    {
        private const int ForReachingSameResult = 10;
        private StoreDatabase StoreDatabase = new StoreDatabase();
        private CustomerDatabase CustomerDatabase = new CustomerDatabase();
        
        private static string ConvertDateTime(DateTime dt)
        {
            string result = "";

            result = dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day.ToString() +
                " " + dt.Hour.ToString() + ":" + dt.Minute.ToString() + ":" + dt.Second.ToString();

            return result;

        }

        public override DataTable GetData()
        {
            using (SQLiteConnection con = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    con.Open();
                    string quary = "SELECT * FROM OrderList";
                    SQLiteCommand command = new SQLiteCommand(quary, con);
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    DataTable dataTable = new DataTable("Order");
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

        public override void Insert(Order item)
        {
            using (SQLiteConnection con = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    con.Open();
                    StoreDatabase.Update(item.Products);
                    if (item.Customer.ReferralCode != 0)
                        CustomerDatabase.UpdateCredit(item.Customer.ReferralCode, item.TotalPrice);
                    if (item.UseCredit)
                        CustomerDatabase.UpdateCredit(item.Customer.ID, item.CreditUsed * ForReachingSameResult, false);
                    string quary = "INSERT INTO OrderList (OrderID, OrderDate, CustomerID, CustomerFirstName, CustomerLastName, OrderPrice, FileName) VALUES ";
                    string value0 = item.ID.ToString();
                    string value1 = ConvertDateTime(item.OrderDate);
                    string value2 = item.Customer.ID.ToString();
                    string value3 = item.Customer.FirstName;
                    string value4 = item.Customer.LastName;
                    string value6 = item.FileName;
                    string value5 = item.TotalPrice.ToString();
                    quary += $"('{value0}', '{value1}', '{value2}', '{value3}', '{value4}', '{value5}', '{value6}');";
                    using (SQLiteCommand command = new SQLiteCommand(quary, con))
                        command.ExecuteNonQuery();
                    con.Close();
                }
                catch
                {
                    throw new Exception();
                }
            }
        }

        public override void Update(Order item)
        {
            using (SQLiteConnection con = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    con.Open();
                    StoreDatabase.Update(item.Products);
                    if (item.Customer.ReferralCode != 0)
                        CustomerDatabase.UpdateCredit(item.Customer.ReferralCode, item.TotalPrice);
                    if (item.UseCredit)
                        CustomerDatabase.UpdateCredit(item.Customer.ID, item.CreditUsed * ForReachingSameResult, false);
                    string quary = "UPDATE OrderList SET Order = @value0, CustomerID = @value1, CustomerFirstName = @value2, CustomerLastName = @value3, OrderPrice = @value5, FileName = @value4 WHERE OrderID = @id ";
                    string value0 = ConvertDateTime(item.OrderDate);
                    string value1 = item.Customer.ID.ToString();
                    string value2 = item.Customer.FirstName;
                    string value3 = item.Customer.LastName;
                    string value5 = item.FileName;
                    string value4 = item.TotalPrice.ToString();
                    string id = item.ID.ToString();
                    using (SQLiteCommand command = new SQLiteCommand(quary, con))
                    {
                        command.Parameters.AddWithValue("@value0", value0);
                        command.Parameters.AddWithValue("@value1", value1);
                        command.Parameters.AddWithValue("@value2", value2);
                        command.Parameters.AddWithValue("@value3", value3);
                        command.Parameters.AddWithValue("@value4", value4);
                        command.Parameters.AddWithValue("@value5", value5);
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
            using (SQLiteConnection con = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    string quary = "DELETE FROM OrderList WHERE OrderID = @id";
                    con.Open();
                    using (SQLiteCommand command = new SQLiteCommand(quary, con))
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
        public void Delete(Order order, bool isDeleteProcessor = true)
        {
            StoreDatabase.Cancel(order.Products);
            if (order.Customer.ReferralCode != 0)
                CustomerDatabase.UpdateCredit(order.Customer.ReferralCode, order.TotalPrice, false);
            if (isDeleteProcessor) Delete(order.ID);
        }

        public override Order Explore(int ID)
        {
            throw new NotImplementedException();
        }

        public override bool TryExplore(int ID, out Order result)
        {
            throw new NotImplementedException();
        }

    }
}
