using System;
using ecom.Models;

namespace ecom.Services
{
    public interface ICheckoutService
    {
        Customer GetCustomer();
        void AddCustomer(Customer customer);
        PaymentForm GetPaymentFormInfo();
        string ProcessOrder(PaymentForm paymentForm);
        Order GetOrder(string id);
    }
}