using System.Linq;
using System.Collections.Generic;
using ecom.Entities;

namespace ecom.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private StoreDbContext _context;

        public ProductRepository(StoreDbContext context)
        {
            _context = context;
        }

        public Products GetProduct(int productId)
        {
            return _context.Products.Where(w => w.ProductId == productId).FirstOrDefault();
        }

        public List<Products> GetProductsInCategory(int id)
        {
            return _context.Products.Where(w => w.CategoryId == id && w.Active == true).OrderBy(o => o.ProductName).ToList();
        }
    }
}