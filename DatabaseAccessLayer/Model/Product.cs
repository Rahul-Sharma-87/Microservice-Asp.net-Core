using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseAccessLayer.Model {
    public class Product {
        public string Name { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public string Origin { get; set; }

        public int Warranty { get; set; }

        public double MRP { get; set; }
    }
}
