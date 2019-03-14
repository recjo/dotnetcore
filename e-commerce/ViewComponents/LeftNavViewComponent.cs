using System;
using Microsoft.AspNetCore.Mvc;
using ecom.Services;

namespace ecom.ViewComponents
{
    public class LeftNavViewComponent : ViewComponent
    {
        private IProductService _productService;

        public LeftNavViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public IViewComponentResult Invoke()
        {
            var categories = _productService.GetProductCategories();
            return View(categories);
        }
    }
}