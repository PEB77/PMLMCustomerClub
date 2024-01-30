using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PMLMCustomerClub
{
    [DataContract]
    public class ProductInfo
    {
        public ProductInfo() { }
        public ProductInfo(ProductInfo productInfo)
        {
            this.ProductID = productInfo.ProductID;
            this.ProductName = productInfo.ProductName;
            this.ProductCategory = productInfo.ProductCategory;
            this.ProductBrand = productInfo.ProductBrand;
            this.ProductPrice = productInfo.ProductPrice;
        }
        public ProductInfo(int productID, string productName, Category productCategory, Brand productBrand, int productPrice)
        {
            ProductID = productID;
            ProductName = productName;
            ProductCategory = productCategory;
            ProductBrand = productBrand;
            ProductPrice = productPrice;
        }

        public enum Category
        {
            Hair_Product = 0,
            SkinCare_Product = 1,
            Cosmetic_Product = 2,
            Cellulose_Product = 3,
            Fragrance_Product = 4,
            Food_Product = 5
        }
        public enum Brand
        {
            Ldora_Herbal = 0,
            Ldora_Care = 1,
            Ldora_Fragrance = 2,
            Ldora_Beauty = 3,
            Nurixo = 4,
            Pristive = 5
        }

        [DataMember]
        public int ProductID { set; get; }
        [DataMember]
        public string ProductName { set; get; }
        [DataMember]
        public Category ProductCategory { set; get; }
        [DataMember]
        public Brand ProductBrand { set; get; }
        [DataMember] 
        public int ProductPrice { set; get; }
    }
}
