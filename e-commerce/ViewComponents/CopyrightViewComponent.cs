using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ecom.ViewComponents
{
    public class CopyrightViewComponent : ViewComponent
    {
        private IConfiguration _configuration { get; }

        public CopyrightViewComponent(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IViewComponentResult Invoke()
        {
            var copy = _configuration["Copyright"];
            return View<string>(copy);
        }
    }
}