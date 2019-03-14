using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ecom.Entities
{
    public class ShippingCarriers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShippingCarrierId { get; set; }
        public string CarrierName { get; set; }
        public string CarrierDesc { get; set; }
        public int Sort { get; set; }
    }
}
