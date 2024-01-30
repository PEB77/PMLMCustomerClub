using DevExpress.Xpf.Editors.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PMLMCustomerClub.Code
{
    [DataContract]
    public class Customer
    {
        public Customer() { }
        public Customer SetID(int id)
        {
            this.ID = id;
            return this;
        }
        
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string PhoneNumber { get; set; }
        [DataMember]
        public DateTime BirthDay { get; set; } = DateTime.Now;
        [DataMember]
        public int ReferralCode { get; set; } = 0;
        [DataMember]
        public int Credit { get; set; } = 0;
        [DataMember]
        public Address Address { get; set; } = new Address();
        [DataMember]
        public string FolderName { get; set; }

        //public object Clone()
        //{
        //    return new Customer(this);
        //}

        public bool Validation()
        {
            if (this.ID == 0) return false;
            if (this.FirstName == "") return false;
            if (this.LastName == "") return false;
            return true;
        }

        public void CreateFolderName()
        {
            this.FolderName = LastName + "_" + FirstName + "_" + DateTime.Now.ToString("yyyy-MM-dd");
        }

        public static Customer GetCustomer(DataRow data)
        {
            Customer customer = new Customer();
            customer.ID = int.Parse(data[0].ToString());
            customer.FirstName = (data[1].ToString() == "<Empty>") ? "" : data[1].ToString();
            customer.LastName = (data[2].ToString() == "<Empty>") ? "" : data[2].ToString();
            customer.PhoneNumber = data[3].ToString();
            customer.BirthDay = DateTime.Parse(data[4].ToString());
            customer.ReferralCode = int.Parse(data[5].ToString());
            customer.Credit = int.Parse(data[6].ToString());
            customer.Address = Address.ToAddress(data[7].ToString());
            customer.FolderName = data[8].ToString();
            return customer;
        }
    }

    [DataContract]
    public class Address : ICloneable
    {
        public Address() { }
        public Address(string state, string city, string location, string zipCode)
        {
            State = state;
            City = city;
            Location = location;
            ZipCode = zipCode;
        }
        public Address(Address address)
        {
            this.State = address.State;
            this.City = address.City;
            this.Location = address.Location;
            this.ZipCode = address.ZipCode;
        }

        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string Location { get; set; }
        [DataMember]
        public string ZipCode { get; set; }

        public override string ToString()
        {
            string output = "";
            Type t = typeof(Address);
            var items = t.GetProperties();
            foreach (var item in items)
            {
                output += (item.Name + " = " + $"*{item.GetValue(this)}*");
                output += "/";
            }

            output = output.TrimEnd("/".ToCharArray());
            return output;
        }

        public static Address ToAddress(string input)
        {
            string[] inputSections = input.Split("/".ToCharArray());
            Address address = new Address();
            Type t = typeof(Address);
            var properties = t.GetProperties();
            for (int i = 0; i < inputSections.Length; i++)
            {
                string[] parts1 = inputSections[i].Split(" ".ToCharArray());
                string[] parts2 = inputSections[i].Split("*".ToCharArray());
                string propertyName = parts1[0];
                string propertyValue = parts2[1];
                foreach (var prop in properties)
                {
                    if (prop.Name == propertyName)
                        prop.SetValue(address, propertyValue);
                }

            }
            return address;
        }

        public object Clone()
        {
            return new Address(this);
        }
    }
}
