using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using ecom.Models;
using ecom.Services;

namespace ecom.Controllers
{
    /*
    This controller used for REST API only
    (not used internally by site)
    */

    public class CategoryController : Controller
    {
        private IProductService _productService;

        public CategoryController(IProductService productService)
        {
            _productService = productService;     
        }

        [HttpGet("api/categories")]
        public IActionResult GetCategories()
        {
            var categories = _productService.GetProductCategories();
            //return json
            return Ok(categories);
        }

        [HttpGet("api/categories/{id}")]
        public IActionResult GetCategory(int id)
        {
            var category = _productService.GetProductCategory(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }
    }
}