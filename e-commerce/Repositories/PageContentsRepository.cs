using System.Linq;
using System.Collections.Generic;
using ecom.Entities;

namespace ecom.Repositories
{
    public class PageContentsRepository : IPageContentsRepository
    {
        private StoreDbContext _context;

        public PageContentsRepository(StoreDbContext context)
        {
            _context = context;
        }

        public IEnumerable<PageContents> GetPageContents()
        {
            return _context.PageContents.OrderBy(o => o.PageTitle).ToList();
        }

        public PageContents GetPageContent(int pageContentId)
        {
            return _context.PageContents.Where(w => w.PageContentId == pageContentId).FirstOrDefault();
        }
    }
}