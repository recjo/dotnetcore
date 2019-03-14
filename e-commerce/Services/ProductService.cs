using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using ecom.Models;
using ecom.Entities;
using ecom.Repositories;

namespace ecom.Services
{
    public class ProductService : IProductService
    {
        private ICategoryRepository _categoryRepository;
        private IProductRepository _productRepository;
        private StoreDbContext _storeDB;

        public ProductService(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _storeDB = new StoreDbContext();
        }

        public List<Category> GetProductCategories()
        {
            //fetch category entities
            var categoryEntities = _categoryRepository.GetCategories();
            if (categoryEntities == null)
            {
                return null;
            }

            //map entity to Category class model
            return categoryEntities.Select(c => new Category()
            {
                Id = c.CategoryId,
                Name = c.CategoryName,
                Sort = c.SortBy,
                Active = c.Active
            }).ToList();
        }

        public Category GetProductCategory(int id)
        {
            //fetch category entities
            var categoryEntity = _categoryRepository.GetCategory(id);
            if (categoryEntity == null)
            {
                return null;
            }

            //map entity to Category class model
            return new Category()
            {
                Id = categoryEntity.CategoryId,
                Name = categoryEntity.CategoryName,
                Sort = categoryEntity.SortBy,
                Active = categoryEntity.Active
            };
        }

        public List<Product> GetProductsInCategory(int id)
        {
            //fetch all active product entities in category
            var productEntities = _productRepository.GetProductsInCategory(id);
            if (productEntities == null)
            {
                return null;
            }

            //get all category models
            var categories = this.GetProductCategories();
            if (categories == null)
            {
                return null;
            }

            //map entity to Product class model
            return productEntities.Select(p => new Product()
            {
                Id = p.ProductId,
                Name = p.ProductName,
                Sku = p.Sku,
                Description = p.ProductDesc,
                Price = p.Price,
                SizeName = p.SizeName,
                ColorName = p.ColorName,
                Active = p.Active,
                Cat = categories.FirstOrDefault(c => c.Id == id)
            }).ToList();
        }

        public Product GetProduct(int id)
        {
            //get all category models
            var categories = this.GetProductCategories();
            if (categories == null)
            {
                return null;
            }

            //fetch product entity
            var productEntity = _productRepository.GetProduct(id);
            return new Product()
            {
                Id = productEntity.ProductId,
                Name = productEntity.ProductName,
                Sku = productEntity.Sku,
                Description = productEntity.ProductDesc,
                Price = productEntity.Price,
                SizeName = productEntity.SizeName,
                isSizeMenu = (productEntity.SizeName.Contains("|") ? true : false),
                ColorName = productEntity.ColorName,
                isColorMenu = (productEntity.ColorName.Contains("|") ? true : false),
                Active = productEntity.Active,
                Cat = categories.FirstOrDefault(c => c.Id == productEntity.CategoryId)
            };
        }

        //make menu items out of pipe-delimited string
        public List<SelectListItem> getMenuItems(string delim)
        {
            var MonthItems = new List<SelectListItem>();
            var list = new List<string>();
            list.AddRange(delim.Split('|'));
            list.ForEach(p => MonthItems.Add(new SelectListItem { Value = p.Trim(), Text = p.Trim() }));
            return MonthItems;
        }
    }
}