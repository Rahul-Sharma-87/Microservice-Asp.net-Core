using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseAccessLayer.Model {
    public enum AddressType {
        Home,
        Office
    }

    public enum Gender {
        Male,
        Female
    }

    public class Address {

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string PostalCode { get; set; }
    }
}
