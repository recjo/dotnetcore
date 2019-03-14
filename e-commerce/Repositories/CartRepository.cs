using System.Linq;
using System.Collections.Generic;
using ecom.Entities;

namespace ecom.Repositories
{
    public class CartRepository : ICartRepository
    {
        private StoreDbContext _context;

        public CartRepository(StoreDbContext context)
        {
            _context = context;
        }

        public void AddToCart(Carts cart)
        {
            _context.Carts.Add(cart);
            _context.SaveChanges();
        }

        public List<Carts> GetCart(string cartId)
        {
            return _context.Carts.Where(w => w.CartId == cartId).Select(p => p).ToList();
        }

        public Carts GetCartItem(string cartId, string productName, string colorName, string sizeName)
        {
            return _context.Carts.Where(w => w.CartId == cartId && w.ProductName == productName && w.SizeName == sizeName && w.ColorName == colorName).FirstOrDefault();
        }

        public Carts GetCartItem(int recordId)
        {
            return _context.Carts.Where(w => w.RecordId == recordId).FirstOrDefault();
        }

        public void UpdateQty(int id, int qty)
        {
            var cartItem = GetCartItem(id);
            cartItem.Count = qty;
            _context.Carts.Update(cartItem);
            _context.SaveChanges();
        }

        public void DeleteCartItem(int recordId)
        {
            var cartItem = GetCartItem(recordId);
            _context.Carts.Remove(cartItem);
            _context.SaveChanges();
        }

        public void DeleteCart(string cartId)
        {
            _context.Carts.RemoveRange(_context.Carts.Where(w => w.CartId == cartId));
            _context.SaveChanges();
        }
    }
}