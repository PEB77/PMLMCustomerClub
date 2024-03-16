using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using PMLMCustomerClub.Database;
using PMLMCustomerClub.Manager;

namespace PMLMCustomerClub.Model
{
    [DataContract]
    public class Order
    {
        public Order()
        {
            this.Customer = new Customer();
            this.Products = new List<StoreItem>();
        }
        
        public Order SetID(int id)
        {
            this.ID = id;
            return this;
        }

        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public DateTime OrderDate { get; set; }
        [DataMember]
        public Customer Customer { get; set; }
        [DataMember]
        public List<StoreItem> Products { get; set; }
        [DataMember]
        public bool UseBirthdayGift { get; set; } = false;
        [DataMember]
        public int BirthdayGift { get; set; } = 0;
        [DataMember]
        public bool UseCredit { get; set; } = false;
        [DataMember]
        public int CreditUsed { get; set; } = 0;
        [DataMember]
        public string Details { get; set; }
        [DataMember]
        public int TotalPrice { get; set; } = 0;
        [DataMember]
        public string FileName { get; set; }

        public bool Validation()
        {
            if (ID <= 0) return false;
            if (Customer.ID == 0) return false;
            if (Products.Count == 0) return false;
            if (TotalPrice < 0) return false;
            return true;
        }

        public void FinalBill()
        {
            TotalPrice = 0;
            foreach(StoreItem item in Products)
            {
                TotalPrice += item.Price * item.Amount;
            }
            if (UseBirthdayGift) TotalPrice -= BirthdayGift;
            if (UseCredit) TotalPrice -= CreditUsed;
        }

        public void CreateFileName()
        {
            OrderDate = DateTime.Now;
            FileName = "Order-" + DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH-mm-ss");
            if (CreditUsed > 0) UseCredit = true; 
        }

        public static Order GetOrder(DataRow data)
        {
            Order order = new Order();
            order.ID = int.Parse(data[0].ToString());
            order.OrderDate = DateTime.Parse(data[1].ToString());
            int customerID = int.Parse(data[2].ToString());
            Customer customer;
            CustomerDatabase customerDatabase = new CustomerDatabase();
            customerDatabase.TryExplore(customerID, out customer);
            order.Customer = customer;
            order.TotalPrice = int.Parse(data[5].ToString());
            order.FileName = data[6].ToString();
            order.Products = FileManager.LoadProducts(order);
            order.FinalBill();
            return order;
        }

    }
}
