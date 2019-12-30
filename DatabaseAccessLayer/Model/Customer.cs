using System.Collections.Generic;

namespace DatabaseAccessLayer.Model {
    public class Customer {
        public long CustomerId { get; set; }

        public string Name { get; set; }

        public Gender Gender { get; set; }

        public string EmailAddress { get; set; }

        public string PrimaryPhone { get; set; }

        public Dictionary<AddressType, Address> Addresses { get; set; } = 
            new Dictionary<AddressType, Address>();
    }
}
