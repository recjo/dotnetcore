using System.Collections.Generic;
using ecom.Entities;

namespace ecom.Repositories
{
    public interface IPageContentsRepository
    {
        IEnumerable<PageContents> GetPageContents();
        PageContents GetPageContent(int pageContentId);
    }
}