using System;
using System.ComponentModel.DataAnnotations;

namespace ecom.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public string CartId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        [MaxLength(150)]
        public string ProductName { get; set; }

        [MaxLength(75)]
        public string SizeName { get; set; }

        [MaxLength(75)]
        public string ColorName { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(50)]
        public string Sku { get; set; }
    }
}
