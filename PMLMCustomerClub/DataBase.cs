using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using PMLMCustomerClub.Code;

namespace PMLMCustomerClub
{
    public static class DataBase
    {
        private const string ConnectString = "server=localhost;uid=root;pwd=Abtin1998;database=pmlm_customer_club";
        private const int ForReachingSameResult = 10;

        public static DataTable GetProductsDataTable()
        {
            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            string query = "SELECT * FROM pmlm_customer_club.product";
            MySqlCommand command = new MySqlCommand(query, con);

            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable dataTable = new DataTable("product");

            adapter.Fill(dataTable);
            con.Close();
            return dataTable;
        }

        public static void InsertNewProduct(ProductInfo productInfo)
        {
            string query = "INSERT INTO product (product_id, product_name, product_category, product_brand, product_price) VALUES ";

            string value1 = productInfo.ProductID.ToString();
            string value2 = productInfo.ProductName;
            string value3 = productInfo.ProductCategory.ToString().Replace("_", " ");
            string value4 = productInfo.ProductBrand.ToString().Replace("_", " ");
            string value5 = productInfo.ProductPrice.ToString();

            query += $"('{value1}', '{value2}', '{value3}', '{value4}', '{value5}'),"; // Add values to the SQL statement

            query = query.TrimEnd(',') + ";"; // Remove the trailing comma and add a semicolon

            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            MySqlCommand command = new MySqlCommand(query, con);
            command.ExecuteNonQuery();
            con.Close();

        }

        public static void UpdateProductRow(ProductInfo productInfo)
        {
            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            string updateSql = "UPDATE product SET product_name = @value1, product_category = @value2, product_brand = @value3, product_price = @value4 WHERE product_id = @id";

            string value1 = productInfo.ProductID.ToString();
            string value2 = productInfo.ProductName;
            string value3 = productInfo.ProductCategory.ToString().Replace("_", " ");
            string value4 = productInfo.ProductBrand.ToString().Replace("_", " ");
            string value5 = productInfo.ProductPrice.ToString();

            using (MySqlCommand updateCommand = new MySqlCommand(updateSql, con))
            {
                updateCommand.Parameters.AddWithValue("@value1", value2);
                updateCommand.Parameters.AddWithValue("@value2", value3);
                updateCommand.Parameters.AddWithValue("@value3", value4);
                updateCommand.Parameters.AddWithValue("@value4", value5);
                updateCommand.Parameters.AddWithValue("@id", value1);

                updateCommand.ExecuteNonQuery();
            }
            
            con.Close();
        }

        public static void DeleteProductRow(int productID)
        {
            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            string deleteSql = "DELETE FROM product WHERE product_id = @id";

            using (MySqlCommand deleteCommand = new MySqlCommand(deleteSql, con))
            {
                deleteCommand.Parameters.AddWithValue("@id", productID);

                deleteCommand.ExecuteNonQuery();
            }
            con.Close();
        }

        public static void InsertNewStoreRow(StoreItem product)
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

            MySqlCommand command = new MySqlCommand(query, con);
            command.ExecuteNonQuery();
            con.Close();
        }

        public static void UpdateStoreRow(StoreItem product)
        {
            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            string updateSql = "UPDATE store SET product_id = @value0, product_name = @value1, product_category = @value2, product_brand = @value3, product_price = @value4, product_expdate = @value5, store_stock = @value6 WHERE store_id = @id";

            string value0 = product.StoreID.ToString();
            string value1 = product.ProductID.ToString();
            string value2 = product.ProductName;
            string value3 = product.Category.ToString().Replace("_", " ");
            string value4 = product.Brand.ToString().Replace("_", " ");
            string value5 = product.Price.ToString();
            string value6 = product.ExpDate.Year.ToString() + "-" + product.ExpDate.Month.ToString() + "-" + product.ExpDate.Day.ToString() +
                " " + product.ExpDate.Hour.ToString() + ":" + product.ExpDate.Minute.ToString() + ":" + product.ExpDate.Second.ToString();
            string value7 = product.Amount.ToString();
            
            using (MySqlCommand updateCommand = new MySqlCommand(updateSql, con))
            {
                updateCommand.Parameters.AddWithValue("@value0", value1);
                updateCommand.Parameters.AddWithValue("@value1", value2);
                updateCommand.Parameters.AddWithValue("@value2", value3);
                updateCommand.Parameters.AddWithValue("@value3", value4);
                updateCommand.Parameters.AddWithValue("@value4", value5);
                updateCommand.Parameters.AddWithValue("@value5", value6);
                updateCommand.Parameters.AddWithValue("@value6", value7);
                updateCommand.Parameters.AddWithValue("@id", value0);

                updateCommand.ExecuteNonQuery();
            }

            con.Close();
        }

        public static DataTable SearchProductInStore(string productName)
        {
            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            string query = $"SELECT * FROM pmlm_customer_club.store WHERE product_name = '{productName}';";
            MySqlCommand command = new MySqlCommand(query, con);

            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable dataTable = new DataTable("store");

            adapter.Fill(dataTable);
            con.Close();
            return dataTable;
        }
        public static StoreItem SearchProductInStore(int storeID)
        {
            MySqlConnection con = new  MySqlConnection(ConnectString);
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
                PMLMCustomerClub.Code.Product.Categories category = (PMLMCustomerClub.Code.Product.Categories)Enum.Parse(typeof(PMLMCustomerClub.Code.Product.Categories), dataRow[3].ToString().Replace(" ", "_"));
                PMLMCustomerClub.Code.Product.Brands brand = (PMLMCustomerClub.Code.Product.Brands)Enum.Parse(typeof(PMLMCustomerClub.Code.Product.Brands), dataRow[4].ToString().Replace(" ", "_"));
                int price = int.Parse(dataRow[5].ToString());
                DateTime expDate = (DateTime)dataRow[6];
                int amount = int.Parse(dataRow[7].ToString());
                product = new StoreItem()
                    .SetStoreID(id)
                    .SetProductData(productID, name, category, brand, price)
                    .SetOtherDetaild( expDate, amount);
            }

            
            return product;
        }
        public static StoreItem SearchProductInStore(StoreItem product)
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
                StoreItem.Categories category = (StoreItem.Categories)Enum.Parse(typeof(StoreItem.Categories), dataRow[3].ToString().Replace(" ", "_"));
                StoreItem.Brands brand = (StoreItem.Brands)Enum.Parse(typeof(StoreItem.Brands), dataRow[4].ToString().Replace(" ", "_"));
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

        public static DataTable GetStoreDataTable()
        {
            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            string query = "SELECT * FROM pmlm_customer_club.store";
            MySqlCommand command = new MySqlCommand(query, con);

            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable dataTable = new DataTable("store");

            adapter.Fill(dataTable);
            con.Close();
            return dataTable;
        }

        public static void DeleteStoreRow(int storeID)
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

        public static DataTable GetCustomersDataTable()
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

        public static void InsertNewCustomer(CustomerProfile newCustomer)
        {
            string query = "INSERT INTO customer_list (customer_id, customer_name, customer_phone_number, customer_birthday, customer_referral_code, customer_credit, address, customer_filepath) VALUES ";

            string value0 = newCustomer.ID.ToString();
            string value1 = newCustomer.Name;
            string value2 = newCustomer.PhoneNumber;
            string value3 = newCustomer.BirthDay.Year.ToString() + "-" + newCustomer.BirthDay.Month.ToString() + "-" +
                newCustomer.BirthDay.Day.ToString();
            string value4 = newCustomer.ReferralCode.ToString();
            string value5 = newCustomer.Credit.ToString();
            string value6 = newCustomer.Address.ToString();
            string value7 = newCustomer.FilePath;
            

            query += $"('{value0}', '{value1}', '{value2}', '{value3}', '{value4}', '{value5}', '{value6}', '{value7}'),"; // Add values to the SQL statement

            query = query.TrimEnd(',') + ";"; // Remove the trailing comma and add a semicolon

            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            MySqlCommand command = new MySqlCommand(query, con);
            command.ExecuteNonQuery();
            con.Close();
        }

        public static bool SearchCustomerProfile(int customerID, out CustomerProfile customerProfile)
        {
            customerProfile = new CustomerProfile();
            bool findCustomerProfile = false;

            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            string quary = $"SELECT * FROM pmlm_customer_club.customer_list WHERE customer_id = {customerID}";
            MySqlCommand command = new MySqlCommand(quary, con);

            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable dataTable = new DataTable("customer_list");

            adapter.Fill(dataTable);
            con.Close();

            if (dataTable.Rows.Count > 0)
            {
                DataRow dataRow = dataTable.Rows[0];
                customerProfile = new CustomerProfile(dataRow);
                return findCustomerProfile = true;
            }
            return findCustomerProfile = false;
        }
        public static bool SearchCustomerProfile(string customerName, out CustomerProfile customerProfile)
        {
            customerProfile = new CustomerProfile();
            bool findCustomerProfile = false;

            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            string quary = $"SELECT * FROM pmlm_customer_club.customer_list WHERE customer_name = '{customerName}'";
            MySqlCommand command = new MySqlCommand(quary, con);

            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable dataTable = new DataTable("customer_list");

            adapter.Fill(dataTable);
            con.Close();

            if (dataTable.Rows.Count > 0)
            {
                DataRow dataRow = dataTable.Rows[0];
                customerProfile = new CustomerProfile(dataRow);
                return findCustomerProfile = true;
            }
            return findCustomerProfile = false;
        }

        public static void UpdateCustomerProfile(CustomerProfile customerProfile)
        {
            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            string quary = "UPDATE customer_list SET customer_name = @value0, customer_phone_number = @value1, customer_birthday = @value2, customer_referral_code = @value3, customer_credit = @value4, address = @value5, customer_filepath = @value6 WHERE customer_id = @id";

            string value0 = customerProfile.Name;
            string value1 = customerProfile.PhoneNumber;
            string value2 = customerProfile.BirthDay.Year.ToString() + "-" + customerProfile.BirthDay.Month.ToString() + "-" +
                customerProfile.BirthDay.Day.ToString();
            string value3 = customerProfile.ReferralCode.ToString();
            string value4 = customerProfile.Credit.ToString();
            string value5 = customerProfile.Address.ToString();
            string value6 = customerProfile.FilePath;
            string id = customerProfile.ID.ToString();

            using (MySqlCommand command = new MySqlCommand(quary, con))
            {
                command.Parameters.AddWithValue("@value0", value0);
                command.Parameters.AddWithValue("@value1", value1);
                command.Parameters.AddWithValue("@value2", value2);
                command.Parameters.AddWithValue("@value3", value3);
                command.Parameters.AddWithValue("@value4", value4);
                command.Parameters.AddWithValue("@value5", value5);
                command.Parameters.AddWithValue("@value6", value6);
                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();
            }

            con.Close();
        }

        public static void DeleteCustomerProfile(int customerID)
        {
            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            string quary = "DELETE FROM customer_list WHERE customer_id = @id";
            using (MySqlCommand command = new MySqlCommand(quary, con))
            {
                command.Parameters.AddWithValue("@id", customerID);
                command.ExecuteNonQuery();
            }
            con.Close();
        }

        public static DataTable GetOrdersDataTable()
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

        public static void InsertNewOrder(OrdersInfo order)
        {
            UpdateStoreAfterOrder(order.Products);
            if (order.Customer.ReferralCode != 0)
                UpdateCustomersCredit(order.Customer.ReferralCode, order.TotalPrice);

            if (order.UseCredit)
                UpdateCustomersCredit(order.Customer.ID, order.CreditUsed * ForReachingSameResult, false);

            string quary = "INSERT INTO orders_list (order_id, order_date, customer_id, customer_name, file_name, order_price) VALUES ";

            string value0 = order.ID.ToString();
            string value1 = ConvertDateTime(order.OrderDate);
            string value2 = order.Customer.ID.ToString();
            string value3 = order.Customer.Name;
            string value4 = order.FileName;
            string value5 = order.TotalPrice.ToString();

            quary += $"('{value0}', '{value1}', '{value2}', '{value3}', '{value4}', '{value5}');";

            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            MySqlCommand command = new MySqlCommand(quary, con);
            command.ExecuteNonQuery();
            con.Close();
        }

        public static void DeleteOrder(OrdersInfo order, bool isDeleteProcessor = true)
        {
            UpdateStoreAfterDelete(order.Products);
            if (order.Customer.ReferralCode != 0)
                UpdateCustomersCredit(order.Customer.ReferralCode, order.TotalPrice, false);

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

        public static void UpdateOrdersList(OrdersInfo order)
        {
            UpdateStoreAfterOrder(order.Products);
            if (order.Customer.ReferralCode != 0)
                UpdateCustomersCredit(order.Customer.ReferralCode, order.TotalPrice);

            if (order.UseCredit)
                UpdateCustomersCredit(order.Customer.ID, order.CreditUsed * ForReachingSameResult, false);

            string quary = "UPDATE orders_list SET order_date = @value0, customer_id = @value1, customer_name = @value2, file_name = @value3, order_price = @value4 WHERE order_id = @id ";

            string value0 = ConvertDateTime(order.OrderDate);
            string value1 = order.Customer.ID.ToString();
            string value2 = order.Customer.Name;
            string value3 = order.FileName;
            string value4 = order.TotalPrice.ToString();
            string id = order.ID.ToString();

            MySqlConnection con = new MySqlConnection(ConnectString);
            con.Open();

            using (MySqlCommand command=new MySqlCommand(quary, con))
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

        private static void UpdateStoreAfterOrder(List<StoreItem> products)
        {
            foreach(StoreItem p in products)
            {
                StoreItem storeProduct = SearchProductInStore(p.StoreID);
                storeProduct.Amount -= p.Amount;
                if (storeProduct.Amount == 0)
                {
                    DeleteStoreRow(storeProduct.StoreID);
                }
                else
                {
                    UpdateStoreRow(storeProduct);
                }
            }
        }
        private static void UpdateStoreAfterDelete(List<StoreItem> products)
        {
            foreach (StoreItem p in products)
            {
                StoreItem storeIDSearch = SearchProductInStore(p.StoreID);
                StoreItem otherProductSearch = SearchProductInStore(p);
                if(storeIDSearch.StoreID == 0)
                {
                    InsertNewStoreRow(p);
                }
                else
                {
                    if(storeIDSearch.StoreID == otherProductSearch.StoreID)
                    {
                        storeIDSearch.Amount += p.Amount;
                        UpdateStoreRow(storeIDSearch);
                    }
                    else
                    {
                        DataTable dataTable = GetStoreDataTable();
                        p.StoreID = dataTable.Rows.Count + 1;
                        InsertNewStoreRow(p);
                    }
                }
                
            }
        }

        private static void UpdateCustomersCredit(int customerID, int orderPrice, bool isIncrese = true)
        {
            CustomerProfile customer;
            SearchCustomerProfile(customerID, out customer);
            if (isIncrese)
                customer.Credit += (int)(orderPrice * 0.1);
            else
                customer.Credit -= (int)(orderPrice * 0.1);
            UpdateCustomerProfile(customer);
        }
        
        private static string ConvertDateTime(DateTime dt)
        {
            string result = "";

            result = dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day.ToString() +
                " " + dt.Hour.ToString() + ":" + dt.Minute.ToString() + ":" + dt.Second.ToString();

            return result;
        } 
        

    }

}
