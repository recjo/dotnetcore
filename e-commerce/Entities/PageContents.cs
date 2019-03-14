using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ecom.Entities
{
    public class PageContents
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PageContentId { get; set; }

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
