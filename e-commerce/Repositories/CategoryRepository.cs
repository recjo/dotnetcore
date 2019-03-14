using System.Linq;
using System.Collections.Generic;
using ecom.Entities;

namespace ecom.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private StoreDbContext _context;

        public CategoryRepository(StoreDbContext context)
        {
            _context = context;
        }

        public Categories GetCategory(int categorytId)
        {
            return _context.Categories.Where(w => w.CategoryId == categorytId).FirstOrDefault();
        }

        public List<Categories> GetCategories()
        {
            return _context.Categories.Where(w => w.Active == true).OrderBy(o => o.SortBy).ToList();
        }
    }
}