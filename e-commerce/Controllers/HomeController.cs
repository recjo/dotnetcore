using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ecom.Models;
using ecom.Services;

namespace ecom.Controllers
{
    public class HomeController : Controller
    {
        private IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;            
        }

        public IActionResult Index()
        {
            var pageContent = _homeService.GetPageContent(1);
            return View(pageContent);
        }

        public IActionResult Terms()
        {
            var pageContent = _homeService.GetPageContent(2);
            return View(pageContent);
        }

        public IActionResult Contact()
        {
            var pageContent = _homeService.GetPageContent(3);
            return View(pageContent);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //================================ API ===============================

        [HttpGet("api/pages")]
        public IActionResult GetPageContentsApi()
        {
            var pageContents = _homeService.GetPageContents();
            if (pageContents == null)
            {
                return NotFound();
            }
            return Ok(pageContents);
        }

        [HttpGet("api/pages/{id}")]
        public IActionResult GetPageContentApi(int id)
        {
            var pageContent = _homeService.GetPageContent(id);
            if (pageContent == null)
            {
                return NotFound();
            }
            return Ok(pageContent);
        }
    }
}
