using System;
using System.Collections.Generic;
using ecom.Models;

namespace ecom.Services
{
    public interface IHomeService
    {
        PageContent GetPageContent(int id);
        List<PageContent> GetPageContents();
    }
}