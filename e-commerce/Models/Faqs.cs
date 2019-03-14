    using System;


namespace ecom.Models
{
    public class Faqs
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int Sort { get; set; }
        public bool Active { get; set; }
    }
}
