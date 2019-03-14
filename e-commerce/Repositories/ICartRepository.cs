using System.Collections.Generic;
using ecom.Entities;

namespace ecom.Repositories
{
    public interface ICartRepository
    {
        void AddToCart(Carts cart);
        List<Carts> GetCart(string cartId);
        Carts GetCartItem(int recordId);
        Carts GetCartItem(string cartId, string productName, string colorName, string sizeName);
        void UpdateQty(int id, int qty);
        void DeleteCartItem(int recordId);
        void DeleteCart(string cartId);
    }
}