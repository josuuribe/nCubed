using RaraAvis.nCubed.DDD.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.DDD.Test.FakeObjects
{
    public class Address
        : ValueObject<Address>
    {
        public string StreetLine1 { get; private set; }
        public string StreetLine2 { get; private set; }
        public string City { get; private set; }
        public string ZipCode { get; private set; }

        public Address(string streetLine1, string streetLine2, string city, string zipCode)
        {
            this.StreetLine1 = streetLine1;
            this.StreetLine2 = streetLine2;
            this.City = city;
            this.ZipCode = zipCode;
        }
    }
}
