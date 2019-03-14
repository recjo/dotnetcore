using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ecom.Models
{
    public class PaymentForm
    {
        public Payment Payment { get; set; }
        public Address BillingAddress { get; set; }
        public Address ShippingAddress { get; set; }
        public Cart Cart { get; set; }
        public List<SelectListItem> ShipMenuItems { get; set; }
        public List<SelectListItem> ExpMonthMenuItems { get; set; }
        public List<SelectListItem> ExpYearMenuItems { get; set; }
    }
}