using System.Collections.Generic;
using ecom.Entities;

namespace ecom.Repositories
{
    public interface ICategoryRepository
    {
        List<Categories> GetCategories();
        Categories GetCategory(int categorytId);
    }
}