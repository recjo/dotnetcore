using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ecom.Models;
using ecom.Services;

namespace ecom.Controllers
{
    public class CartController : Controller
    {
        private IShoppingCartService _shoppingCartService;

        public CartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        // GET: /cart/
        public IActionResult Index()
        {
            var cart = _shoppingCartService.GetCart();
            return View(cart);
        }

        // POST: /cart/addtocart/
        [HttpPost]
        public IActionResult AddToCart([FromForm] CartForm cartForm)
        {
            var result = _shoppingCartService.AddToCart(cartForm);
            // Go to shopping cart page
            return RedirectToAction("Index");
        }

        // POST: /cart/update/
        [HttpPost]
        public IActionResult Update([FromForm] CartUpdateForm cartUpdateForm)
        {
            _shoppingCartService.UpdateQty(cartUpdateForm);
            // Go to shopping cart page
            return RedirectToAction("Index");
        }

        // GET: /cart/delete/{id}
        public IActionResult Delete(int id)
        {
            _shoppingCartService.DeleteCartItem(id);
            // Go to shopping cart page
            return RedirectToAction("Index");
        }
    }
}
