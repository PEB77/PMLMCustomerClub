using DevExpress.Security.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PMLMCustomerClub.Code
{
    public static class FileManager
    {
        private static string StartUpFilePath = Application.StartupPath;
        private static string CustomersFolderPath = "\\Database\\CustomerFolderPath";
        private const string CustomerProfileName = "\\CustomerProfile.cp";
        private const string OrderFileIndex = ".order";
        private const string DoubleSlash = "\\";

        public static void SaveCustomer(Customer customer)
        {
            string filePath = StartUpFilePath + CustomersFolderPath + DoubleSlash + customer.FolderName;
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            string content = JsonConvert.SerializeObject(customer);
            File.WriteAllText(filePath + CustomerProfileName, content);

        }

        public static void UpdateCustomer(Customer customer)
        {
            string filePath = StartUpFilePath + CustomersFolderPath + DoubleSlash + customer.FolderName;

            if (!Directory.Exists(filePath) )
            {
                Directory.CreateDirectory(filePath);
                
            }
            string content = JsonConvert.SerializeObject(customer);
            File.WriteAllText(filePath + CustomerProfileName, content);
        }

        public static Customer LoadCustomer(string folderName)
        {
            string filePath = StartUpFilePath + CustomersFolderPath
                + DoubleSlash + folderName + CustomerProfileName;
            Customer customer = new Customer();
            try
            {
                string content = File.ReadAllText(filePath);
                customer = JsonConvert.DeserializeObject<Customer>(content);

            }
            catch (Exception ex)
            {

            }
            return customer;
        }

        public static void DeleteCustomerFolder(string folderName)
        {
            string filePath = StartUpFilePath + CustomersFolderPath + DoubleSlash + folderName;
            Directory.Delete(filePath, true);
        }

        public static void SaveOrder(Order order)
        {
            string filePath = StartUpFilePath + CustomersFolderPath + DoubleSlash + order.Customer.FolderName;
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            string content = JsonConvert.SerializeObject(order);
            File.WriteAllText(filePath + DoubleSlash + order.FileName + OrderFileIndex, content);
        }

        public static OrdersInfo LoadOrder(CustomerProfile customer, string orderFileName)
        {
            string filePath = StartUpFilePath + CustomersFolderPath
                + DoubleSlash + customer.FilePath + DoubleSlash + orderFileName + OrderFileIndex;
            OrdersInfo orderInfo = new OrdersInfo();
            try
            {
                string content = File.ReadAllText(filePath);
                orderInfo = JsonConvert.DeserializeObject<OrdersInfo>(content);
            }
            catch
            {

            }
            return orderInfo;
        }

        public static void DeleteOrderFile(CustomerProfile customer, string orderFileName)
        {
            string filePath = StartUpFilePath + CustomersFolderPath +
                DoubleSlash + customer.FilePath + DoubleSlash + orderFileName + OrderFileIndex;
            File.Delete(filePath);
        }
    }
}
