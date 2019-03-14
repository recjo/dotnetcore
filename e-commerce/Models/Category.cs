using System;

namespace ecom.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Sort { get; set; }
        public bool Active { get; set; }
    }
}
