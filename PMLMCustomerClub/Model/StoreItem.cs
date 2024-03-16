using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PMLMCustomerClub.Model
{
    [Serializable]
    [DataContract]
    public class StoreItem : Product, ICloneable, IEquatable<StoreItem>
    {
        public StoreItem() { }
        
        public StoreItem SetStoreID(int id)
        {
            this.StoreID = id;
            return this;
        }
        public StoreItem SetProductData(int id, string name, Categories category, Brands brand, int price)
        {
            base.ProductID = id;
            base.ProductName = name;
            base.Category = category;
            base.Brand = brand;
            base.Price = price;
            return this;
        }
        public StoreItem SetOtherDetaild(DateTime expDate, int amount)
        {
            this.ExpDate = expDate;
            this.Amount = amount;
            return this;
        }

        public object Clone()
        {
            StoreItem item = new StoreItem()
                .SetStoreID(this.StoreID)
                .SetProductData(this.ProductID, this.ProductName, this.Category, this.Brand, this.Price)
                .SetOtherDetaild(this.ExpDate, this.Amount);
            return item;
        }

        [DataMember]
        public int StoreID { get; set; }
        [DataMember]
        public int Amount { get; set; } = 0;
        [DataMember]
        public DateTime ExpDate { get; set; } = DateTime.Now;

        public bool Validation()
        {
            if (StoreID == 0) return false;
            if (ProductID == 0) return false;
            if (ProductName == null && ProductName == "") return false;
            if (Category == Categories.Empty) return false;
            if (Brand == Brands.Empty) return false;
            if (Price == 0) return false;
            if (Amount == 0) return false;
            return true;
        }

        public static StoreItem GetStoreItem(DataRow data)
        {
            StoreItem item = new StoreItem();
            item.StoreID = int.Parse(data[0].ToString());
            item.ProductID = int.Parse(data[1].ToString());
            item.ProductName = data[2].ToString();
            item.Category = GetCategory(data[3].ToString());
            item.Brand = GetBrand(data[4].ToString());
            item.Price = int.Parse(data[5].ToString());
            item.Amount = int.Parse(data[7].ToString());
            item.ExpDate = DateTime.Parse(data[6].ToString());
            return item;
        }

        public bool Equals(StoreItem other)
        {
            if (this.StoreID != other.StoreID) return false;
            if (this.ProductID != other.ProductID) return false;
            if (this.ExpDate != other.ExpDate) return false;
            if (this.Price != other.Price) return false;
            return true;
        }
    }
}
