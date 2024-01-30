using PMLMCustomerClub.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PMLMCustomerClub
{
    [DataContract]
    public class OrdersInfo : ICloneable
    {
        public OrdersInfo() 
        {
            this.Customer = new CustomerProfile();
            this.Products = new List<StoreItem>();
        }
        public OrdersInfo(OrdersInfo ordersInfo)
        {
            this.ID = ordersInfo.ID;
            this.OrderDate = ordersInfo.OrderDate;
            this.Customer = new CustomerProfile(ordersInfo.Customer);
            this.Products = new List<StoreItem>();
            foreach(StoreItem product in ordersInfo.Products)
            {
                this.Products.Add(product);
            }
            this.UseBirthdayGift = ordersInfo.UseBirthdayGift;
            this.BirthdayGift = ordersInfo.BirthdayGift;
            this.UseCredit = ordersInfo.UseCredit;
            this.CreditUsed = ordersInfo.CreditUsed;
            this.Details = ordersInfo.Details;
            this.TotalPrice = ordersInfo.TotalPrice;
            this.FileName = ordersInfo.FileName;
        }
        public OrdersInfo(int iD, DateTime orderDate, CustomerProfile customer, List<StoreItem> products, bool useBirthdayGift, int birthdayGift, bool useCredit, int creditUsed, string details, int totalPrice, string fileName)
        {
            ID = iD;
            OrderDate = orderDate;
            Customer = customer;
            Products = products;
            UseBirthdayGift = useBirthdayGift;
            BirthdayGift = birthdayGift;
            UseCredit = useCredit;
            CreditUsed = creditUsed;
            Details = details;
            TotalPrice = totalPrice;
            FileName = fileName;
        }

        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public DateTime OrderDate { get; set; }
        [DataMember]
        public CustomerProfile Customer { get; set; }
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
        
        public object Clone()
        {
            return new OrdersInfo(this);
        }

    }
}
