using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using ecom.Repositories;
using ecom.Models;

namespace ecom.Controllers
{
    public class FaqsController : Controller
    {
        private IFaqsRepository _faqsRepository;

        public FaqsController(IFaqsRepository faqsRepository)
        {
            _faqsRepository = faqsRepository;            
        }

        public IActionResult Index()
        {
            var faqsEntities = _faqsRepository.GetFaqs();
            var faqs = faqsEntities.Select(p => new Faqs()
            {
                Id = p.FaqId,
                Question = p.Question,
                Answer = p.Answer,
                Sort = p.Sort,
                Active = p.Active
            }).ToList();
            return View(faqs);
        }

        //================================ API ===============================

        [HttpGet("api/faqs")]
        public IActionResult GetFaqsApi()
        {
            var faqs = _faqsRepository.GetFaqs();
            if (faqs == null)
            {
                return NotFound();
            }
            return Ok(faqs);
        }
    }
}