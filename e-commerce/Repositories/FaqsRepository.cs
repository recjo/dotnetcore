using System.Linq;
using System.Collections.Generic;
using ecom.Entities;

namespace ecom.Repositories
{
    public class FaqsRepository : IFaqsRepository
    {
        private StoreDbContext _context;

        public FaqsRepository(StoreDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Faqs> GetFaqs()
        {
            return _context.Faqs.Where(w => w.Active == true).OrderBy(o => o.Sort).ToList();
        }

        public Faqs GetFaq(int faqId)
        {
            return _context.Faqs.Where(w => w.FaqId == faqId).FirstOrDefault();
        }
    }
}