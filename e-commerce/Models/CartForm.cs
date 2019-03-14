using System;

namespace ecom.Models
{
    public class CartForm
    {
        public int Id { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public int Qty { get; set; }
    }

    public class CartUpdateForm
    {
        public int Id { get; set; }
        public int Qty { get; set; }
    }
}