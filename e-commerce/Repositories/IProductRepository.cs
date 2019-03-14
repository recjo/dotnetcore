using System.Collections.Generic;
using ecom.Entities;

namespace ecom.Repositories
{
    public interface IProductRepository
    {
        Products GetProduct(int id);
        List<Products> GetProductsInCategory(int id);
    }
}