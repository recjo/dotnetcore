using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ecom.Models;
using ecom.Services;

namespace ecom.Controllers
{
    public class StoreController : Controller
    {
        private IProductService _productService;

        public StoreController(IProductService productService)
        {
            _productService = productService;            
        }

        //GET: /store
        public IActionResult Index()
        {
            var categories = _productService.GetProductCategories();
            return View(categories);
        }

        //GET: /store/browse/{catId}
        public IActionResult Browse(int id)
        {
            var products = _productService.GetProductsInCategory(id);
            return View(products);
        }

        //GET: /store/details/{productId}
        public IActionResult Details(int id)
        {
            var product = _productService.GetProduct(id);
            if (product.isSizeMenu)
                ViewData["SizeMenu"] = _productService.getMenuItems(product.SizeName);
            if (product.isColorMenu)
                ViewData["ColorMenu"] = _productService.getMenuItems(product.ColorName);
            return View(product);
        }
    }
}
