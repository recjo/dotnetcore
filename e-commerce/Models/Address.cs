using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ecom.Entities;

namespace ecom.Models
{
    public class Address
    {

        [DisplayName("First Name")]
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(60)]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(60)]
        public string LastName { get; set; }

        [DisplayName("Address 1")]
        [Required(ErrorMessage = "Address is required")]
        [StringLength(50)]
        public string Address1 { get; set; }

        [DisplayName("Address 2")]
        [StringLength(50)]
        public string Address2 { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(40)]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        [StringLength(40)]
        public string State { get; set; }

        [DisplayName("Zip Code")]
        [Required(ErrorMessage = "Zip Code is required")]
        [StringLength(11)]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [StringLength(40)]
        public string Country { get; set; }
    }
}
