using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization;
using System.Data;
using DevExpress.Xpf.Editors.Helpers;

namespace PMLMCustomerClub
{
    [DataContract]
    public class CustomerProfile : ICloneable
    {
        public CustomerProfile() 
        {
            this.Address = new Address();
        }
        public CustomerProfile(CustomerProfile customerProfile)
        {
            this.ID = customerProfile.ID;
            this.Name = customerProfile.Name;
            this.PhoneNumber = customerProfile.PhoneNumber;
            this.BirthDay = customerProfile.BirthDay;
            this.ReferralCode = customerProfile.ReferralCode;
            this.Credit = customerProfile.Credit;
            this.Address = customerProfile.Address;
            this.FilePath = customerProfile.FilePath;
        }
        public CustomerProfile(int iD, string name, string phoneNumber, DateTime birthDay, int referralCode, int credit, Address address, string filePath)
        {
            ID = iD;
            Name = name;
            PhoneNumber = phoneNumber;
            BirthDay = birthDay;
            ReferralCode = referralCode;
            Credit = credit;
            Address = (Address)address.Clone();
            FilePath = filePath;
        }
        public CustomerProfile(DataRow customerRow)
        {
            ID = (int)customerRow[0];
            Name = customerRow[1].ToString();
            PhoneNumber = customerRow[2].ToString();
            BirthDay = customerRow[3].TryConvertToDateTime();
            ReferralCode = (int)customerRow[4];
            Credit = (int)customerRow[5];
            Address = Address.ToCustomrProfile(customerRow[6].ToString());
            FilePath = customerRow[7].ToString();

        }

        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string PhoneNumber { get; set; }
        [DataMember]
        public DateTime BirthDay { get; set; }
        [DataMember]
        public int ReferralCode { get; set; }
        [DataMember]
        public int Credit { get; set; }
        [DataMember]
        public Address Address { get; set; }
        [DataMember]
        public string FilePath { get; set; }

        public object Clone()
        {
            return new CustomerProfile(this);
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
            foreach(var item in items)
            {
                output += (item.Name + " = " + $"*{item.GetValue(this)}*");
                output += "/";
            }

            output = output.TrimEnd("/".ToCharArray());
            return output;
        }

        public static Address ToCustomrProfile(string input)
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
                foreach(var prop in properties)
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
