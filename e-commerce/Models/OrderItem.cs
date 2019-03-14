using System;
using System.Linq;
using System.Collections.Generic;

namespace ecom.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string ProductName { get; set; }

        public string SizeName { get; set; }

        public string ColorName { get; set; }

        public string Sku { get; set; }
    }
}
