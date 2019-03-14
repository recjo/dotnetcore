using System;
using System.ComponentModel.DataAnnotations;


namespace ecom.Models
{
    public class PageContent
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string PageKey { get; set; }

        [MaxLength(40)]
        public string LinkName { get; set; }

        [Required]
        [MaxLength(100)]
        public string PageTitle { get; set; }

        [Required]
        public string PageText { get; set; }
    }
}
