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

    public class ProductController : Controller
    {
        private IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;            
        }

        [HttpGet("api/products/{id}")]
        public IActionResult GetProductsByCategory(int id)
        {
            var products = _productService.GetProductsInCategory(id);
            //return json
            return Ok(products);
        }

        [HttpGet("api/product/{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = _productService.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        /*public JsonResult GetProducts()
        {
            //return json
            return new JsonResult(ProductsDataStore.Current.Products);
        }*/

        /*public JsonResult GetProducts()
        {
            //return json
            return new JsonResult(new List<object>()
            {
                new { id=1, name="Hoodie", price=29.99},
                new { id=2, name="Shirt", price=19.99},
                new { id=2, name="Hat", price=9.99}
            });
        }*/
    }
}