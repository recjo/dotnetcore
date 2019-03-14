using System;
using System.Collections.Generic;
using ecom.Models;

namespace ecom.Services
{
    public interface IShoppingCartService
    {
        bool AddToCart(CartForm cartForm);
        string GetCartId();
        Cart GetCart();
        void DeleteCartItem(int id);
        void DeleteCart();
        void UpdateQty(CartUpdateForm cartUpdateForm);
    }
}