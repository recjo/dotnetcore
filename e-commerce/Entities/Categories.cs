using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ecom.Entities
{
    public class Categories
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(75)]
        public string CategoryName { get; set; }

        [Required]
        public int SortBy { get; set; }

        [Required]
        public bool Active { get; set; }
    }
}
