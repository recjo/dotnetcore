using System;
using System.Linq;
using System.Collections.Generic;

namespace ecom.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string OrderNumber { get; set; }

        public string OrderStatus { get; set; }

        public DateTime OrderDate { get; set; }

        public string Username { get; set; }

        public string BillingFirstName { get; set; }

        public string BillingLastName { get; set; }

        public string BillingAddress1 { get; set; }

        public string BillingAddress2 { get; set; }

        public string BillingCity { get; set; }

        public string BillingState { get; set; }

        public string BillingPostalCode { get; set; }

        public string BillingCountry { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string ShippingFirstName { get; set; }

        public string ShippingLastName { get; set; }

        public string ShippingAddress1 { get; set; }

        public string ShippingAddress2 { get; set; }

        public string ShippingCity { get; set; }

        public string ShippingState { get; set; }

        public string ShippingPostalCode { get; set; }

        public string ShippingCountry { get; set; }

        public decimal Total { get; set; }

        public decimal SubTotal { get; set; }

        public decimal Tax { get; set; }

        public decimal Shipping { get; set; }

        public string ShipVia { get; set; }

        public string Last4 { get; set; }

        public string PaymentType { get; set; }

        public DateTime PaymentDate { get; set; }

        public DateTime ShippingDate { get; set; }

        public int Ccv { get; set; }

        public int ExpMo { get; set; }

        public int ExpYear { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}
