using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PMLMCustomerClub
{
    [Serializable]
    [DataContract]
    public class Product : ProductInfo, ICloneable, IEquatable<Product>
    {
        public Product() { }
        public Product(Product item)
        {
            this.StoreID = item.StoreID;
            this.ProductID = item.ProductID;
            this.ProductName = item.ProductName;
            this.ProductBrand = item.ProductBrand;
            this.ProductCategory = item.ProductCategory;
            this.ProductPrice = item.ProductPrice;
            this.ExpDate = item.ExpDate;
            this.Amount = item.Amount;
        }
        public Product(int id, int productID, string name, Brand brand, Category category, int price, DateTime expDate, int amount)
        {
            this.StoreID = id;
            this.ProductID = productID;
            this.ProductName = name;
            this.ProductBrand = brand;
            this.ProductCategory = category;
            this.ProductPrice = price;
            this.ExpDate = expDate;
            this.Amount = amount;
        }

        [DataMember]
        public DateTime ExpDate { get; set; }
        [DataMember]
        public int Amount { get; set; }
        [DataMember]
        public int StoreID { get; set; }

        public object Clone()
        {
            return new Product(this);
        }

        public bool Equals(Product other)
        {
            Type t = typeof(Product);
            var properties = t.GetProperties();
            foreach(var prop in properties)
            {
                var input1 = prop.GetValue(this);
                var input2 = prop.GetValue(other);
                if (!input1.Equals(input2))
                    return false;
            }
            return true;
        }

    }
}
