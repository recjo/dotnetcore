using System;
using System.Linq;
using System.Collections.Generic;
using ecom.Models;
using ecom.Repositories;

namespace ecom.Services
{
    public class HomeService : IHomeService
    {
        private IPageContentsRepository _pageContentsRepository;

        public HomeService(IPageContentsRepository pageContentsRepository)
        {
            _pageContentsRepository = pageContentsRepository;
        }

        public List<PageContent> GetPageContents()
        {
            var pageContentEntities = _pageContentsRepository.GetPageContents();
            return pageContentEntities.Select(p => new PageContent()
            {
                Id = p.PageContentId,
                LinkName = p.LinkName,
                PageKey = p.PageKey,
                PageTitle = p.PageTitle,
                PageText = p.PageText
            }).ToList();
        }

        public PageContent GetPageContent(int id)
        {
            var pageContentEntity = _pageContentsRepository.GetPageContent(id);
            return new PageContent()
            {
                Id = pageContentEntity.PageContentId,
                LinkName = pageContentEntity.LinkName,
                PageKey = pageContentEntity.PageKey,
                PageTitle = pageContentEntity.PageTitle,
                PageText = pageContentEntity.PageText
            };
        }
    }
}