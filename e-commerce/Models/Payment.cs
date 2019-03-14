using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ecom.Models
{
    public class Payment
    {
        [DisplayName("Credit Card Number")]
        [Required(ErrorMessage = "Credit Card Number cannot be blank")]
        [RegularExpression(@"^(?:4[0-9]{12}(?:[0-9]{3})?|[25][1-7][0-9]{14}|6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|(?:2131|1800|35\d{3})\d{11})$", ErrorMessage = "Credit Card is is not valid.")]
        [StringLength(16)]
        public string CreditCardNumber { get; set; }

        //[DisplayName("Credit Card Name")]
        //[Required(ErrorMessage = "Credit Card Name cannot be blank")]
        //[StringLength(60)]
        //public string CreditCardName { get; set; }

        [DisplayName("Security Code")]
        [Required(ErrorMessage = "Security Code cannot be blank")]
        [RegularExpression(@"^[0-9]{3,4}$", ErrorMessage = "Security Code must 3 or 4 numbers.")]
        [StringLength(4)]
        public string SecurityCode { get; set; }

        [Required(ErrorMessage = "Please choose Expiration Month")]
        public string ExpMonth { get; set; }

        [Required(ErrorMessage = "Please choose Expiration Year")]
        public string ExpYear { get; set; }
    }
}