using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PMLMCustomerClub.Code
{
    [Serializable]
    [DataContract]
    public class Product
    {
        public Product() { }
        public Product(Product product)
        {
            this.ProductID = product.ProductID;
            this.ProductName = product.ProductName;
            this.Category = product.Category;
            this.Brand = product.Brand;
            this.Price = product.Price;
        }
        public Product(int productID, string productName, Categories productCategory, Brands productBrand, int productPrice)
        {
            ProductID = productID;
            ProductName = productName;
            Category = productCategory;
            Brand = productBrand;
            Price = productPrice;
        }

        public enum Categories
        {
            Empty = 0,
            Hair_Product = 1,
            SkinCare_Product = 2,
            Cosmetic_Product = 3,
            Cellulose_Product = 4,
            Fragrance_Product = 5,
            Food_Product = 6
        }
        public enum Brands
        {
            Empty = 0,
            Ldora_Herbal = 1,
            Ldora_Care = 2,
            Ldora_Fragrance = 3,
            Ldora_Beauty = 4,
            Nurixo = 5,
            Pristive = 6
        }

        [DataMember]
        public int ProductID { set; get; }
        [DataMember]
        public string ProductName { set; get; }
        [DataMember]
        public Categories Category { set; get; }
        [DataMember]
        public Brands Brand { set; get; }
        [DataMember]
        public int Price { set; get; }

        public bool Validation()
        {
            if (ProductID == 0) return false;
            if (Category == Categories.Empty) return false;
            if (Brand == Brands.Empty) return false;
            if (ProductName == "" || ProductName == null) return false;
            if (Price == 0) return false;
            return true;
        }

        public static Product GetProduct(DataRow data)
        {
            Product product = new Product();
            product.ProductID = int.Parse(data[0].ToString());
            product.ProductName = data[1].ToString();
            product.Category = (Categories)Enum.Parse(typeof(Categories), data[2].ToString().Replace(" ", "_"));
            product.Brand = (Brands)Enum.Parse(typeof(Brands), data[3].ToString().Replace(" ", "_"));
            product.Price = int.Parse(data[4].ToString());
            return product;
        }

    }
}
