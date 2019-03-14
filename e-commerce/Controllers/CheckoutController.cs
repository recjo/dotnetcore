using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using ecom.Models;
using ecom.Services;

namespace ecom.Controllers
{
    public class CheckoutController : Controller
    {
        private ICheckoutService _checkoutService;

        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Address()
        {
            var customer = _checkoutService.GetCustomer();
            return View(customer);
        }

        [HttpPost]
        public IActionResult Address(Customer customer)
        {
            _checkoutService.AddCustomer(customer);

            // Go to payment page
            return RedirectToAction("payment");
        }

        [HttpGet]
        public IActionResult Payment()
        {
            var paymentForm = _checkoutService.GetPaymentFormInfo();
            return View(paymentForm);
        }

        [HttpPost]
        public IActionResult Payment(PaymentForm paymentForm)
        {
            if (!ModelState.IsValid)
            {
                var errorsList = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage)).ToList();
                TempData["ModelErrors"] = errorsList;
                return RedirectToAction("error", "home");
            }

            var orderId = _checkoutService.ProcessOrder(paymentForm);

            // Go to payment page
            return RedirectToAction("order", new { id = orderId });
        }

        [HttpGet]
        public IActionResult Order(string id)
        {
            var orderDetails = _checkoutService.GetOrder(id);
            return View(orderDetails);
        }
    }
}
