using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ecom.Entities;

namespace ecom.Models
{
    public class Cart
    {
        public decimal CartTotal { get; set; }
        public decimal Tax { get; set; }
        public string ShipVia { get; set; }
        public decimal ShipCost { get; set; }
        public List<CartItem> CartItems { get; set; }
        public List<ShippingCostsByQties> ShipChart { get; set; }
    }
}
