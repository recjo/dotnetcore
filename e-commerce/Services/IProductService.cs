using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using ecom.Models;

namespace ecom.Services
{
    public interface IProductService
    {
        Product GetProduct(int id);
        List<Product> GetProductsInCategory(int id);
        List<Category> GetProductCategories();
        Category GetProductCategory(int id);
        List<SelectListItem> getMenuItems(string delim);
    }
}