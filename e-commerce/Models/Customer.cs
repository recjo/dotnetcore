using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ecom.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "User Name is required")]
        [StringLength(40)]
        public string Uname { get; set; }

        [DisplayName("Email Address")]
        [Required(ErrorMessage = "Email Address is required")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Email is is not valid.")]
        [DataType(DataType.EmailAddress)]
        [StringLength(50)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [StringLength(30)]
        public string Phone { get; set; }

        public Address BillingAddress { get; set; }

        public Address ShippingAddress { get; set; }

        [ScaffoldColumn(false)]
        public DateTime PostDate { get; set; }

        [ScaffoldColumn(false)]
        public bool isLoggedIn { get; set; }

        [ScaffoldColumn(false)]
        public List<SelectListItem> StateMenuItems;

        [ScaffoldColumn(false)]
        public List<SelectListItem> CountryMenuItems;

        public Customer()
        {
            PostDate = DateTime.Now;
        }
    }
}
