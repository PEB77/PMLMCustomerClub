using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Protobuf.Reflection;
using Newtonsoft.Json;

namespace PMLMCustomerClub
{
    public static class FileManagment
    {
        private static string StartUpFilePath = Application.StartupPath;
        private static string CustomersFolderPath = "\\Database\\CustomerFolderPath";
        private const string CustomerProfileName = "\\CustomerProfile.cp";
        private const string OrderFileIndex = ".order";
        private const string DoubleSlash = "\\";

        public static void WriteCustomerProfile(CustomerProfile customerProfile)
        {
            string filePath = StartUpFilePath + CustomersFolderPath + DoubleSlash + customerProfile.FilePath;
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            string content = JsonConvert.SerializeObject(customerProfile);
            File.WriteAllText(filePath + CustomerProfileName, content);
            
        }

        public static CustomerProfile ReadCustomerProfile(string folderName)
        {
            string filePath = StartUpFilePath + CustomersFolderPath 
                + DoubleSlash + folderName + CustomerProfileName;
            CustomerProfile customerProfile = new CustomerProfile();
            try
            {
                string content = File.ReadAllText(filePath);
                customerProfile = JsonConvert.DeserializeObject<CustomerProfile>(content);
                
            }
            catch (Exception ex)
            {

            }
            return customerProfile;
        }

        public static void DeleteCustomerFolder(string folderName)
        {
            string filePath = StartUpFilePath + CustomersFolderPath + DoubleSlash + folderName;
            Directory.Delete(filePath, true);
        }
    
        public static void WriteOrderFile(OrdersInfo ordersInfo)
        {
            string filePath = StartUpFilePath + CustomersFolderPath + DoubleSlash + ordersInfo.Customer.FilePath;
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            string content = JsonConvert.SerializeObject(ordersInfo);
            File.WriteAllText(filePath + DoubleSlash + ordersInfo.FileName + OrderFileIndex, content);
        }

        public static OrdersInfo ReadOrdersInfo(CustomerProfile customer, string orderFileName)
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
