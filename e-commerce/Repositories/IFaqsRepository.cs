using System.Collections.Generic;
using ecom.Entities;

namespace ecom.Repositories
{
    public interface IFaqsRepository
    {
        IEnumerable<Faqs> GetFaqs();
        Faqs GetFaq(int faqId);
    }
}