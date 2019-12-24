using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseAccessLayer.Model {
    public class Order {

        public long OrderId { get; set; }

        public DateTime OrderDate { get; set; }

        public long ProductId { get; set; }

        public long CustomerId { get; set; }

        public int Quantity { get; set; }

        public double Discount { get; set; }

        public double TotalPrice { get; set; }
    }
}
