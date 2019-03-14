using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ecom.Models;
using ecom.Entities;
using ecom.Repositories;

namespace ecom.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        public const string CartSessionKey = "CartId";
        private ICartRepository _cartRepository;
        private IShippingRepository _shippingRepository;
        private IProductService _productService;
        private IHttpContextAccessor _httpContextAccessor;
        private IConfiguration _configuration { get; }
        private string shoppingCartId { get; set; }

        public ShoppingCartService(ICartRepository cartRepository, IShippingRepository shippingRepository, IProductService productService, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _cartRepository = cartRepository;
            _shippingRepository = shippingRepository;
            _productService = productService;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            this.shoppingCartId = this.GetCartId();
        }

        public bool AddToCart(CartForm cartForm)
        {
            //fetch product
            var product = _productService.GetProduct(cartForm.Id);
            if (product == null)
            {
                return false;
            }

            //if item already in cart, increment qty
            var cartItem = _cartRepository.GetCartItem(shoppingCartId, product.Name, cartForm.ColorName, cartForm.SizeName);
            if (cartItem != null)
            {
                _cartRepository.UpdateQty(cartItem.RecordId, ++cartItem.Count);
                return true;
            }

            //create cart item from product info and form data
            cartItem = new Carts()
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ColorName = cartForm.ColorName,
                SizeName = cartForm.SizeName,
                CartId = shoppingCartId,
                Count = cartForm.Qty,
                Price = Convert.ToDecimal(product.Price),
                Sku = product.Sku,
                DateCreated = DateTime.Now
            };
            _cartRepository.AddToCart(cartItem);

            return true;
        }

        public Cart GetCart()
        {
            //get cart items
            var cartEntity = _cartRepository.GetCart(this.shoppingCartId);
            var cartItems = cartEntity.Select(p => new CartItem()
            {
                Id = p.RecordId,
                CartId = p.CartId,
                ProductId = p.ProductId,
                Quantity = p.Count,
                DateCreated = p.DateCreated,
                ProductName = p.ProductName,
                SizeName = p.SizeName,
                ColorName = p.ColorName,
                Price = p.Price,
                Sku = p.Sku
            }).ToList();
            var taxRate = Convert.ToDecimal(_configuration["TaxRate"]);
            //build cart
            return new Cart()
            {
                CartItems = cartItems,
                CartTotal = cartItems.Sum(c => c.Price * c.Quantity),
                ShipChart = _shippingRepository.GetShipCosts(),
                Tax = cartItems.Sum(c => c.Price * c.Quantity * taxRate)
            };
        }

        public string GetCartId()
        {
            var context = _httpContextAccessor.HttpContext;
            if (string.IsNullOrEmpty(context.Session.GetString(CartSessionKey)))
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session.SetString(CartSessionKey, context.User.Identity.Name);
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    context.Session.SetString(CartSessionKey, tempCartId.ToString());
                }
            }
            return context.Session.GetString(CartSessionKey);
        }

        public void UpdateQty(CartUpdateForm cartUpdateForm)
        {
            _cartRepository.UpdateQty(cartUpdateForm.Id, cartUpdateForm.Qty);
        }

        public void DeleteCartItem(int id)
        {
            _cartRepository.DeleteCartItem(id);
        }

        public void DeleteCart()
        {
            _cartRepository.DeleteCart(shoppingCartId);
        }
    }
}