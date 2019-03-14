using System;

namespace ecom.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Category Cat { get; set; }
        public string Description { get; set; }
        public string SizeName { get; set; }
        public bool isSizeMenu { get; set; }
        public string ColorName { get; set; }
        public bool isColorMenu { get; set; }
        public bool Active { get; set; }
    }
}