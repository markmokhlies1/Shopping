using System.Collections;
using System.Collections.Generic;
using API.Models;

namespace API.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int MarketingId { get; set; }

        public User Marketing { get; set; }
        public List<ProductOptionCart> ProductOptionCart { get; set; }
    }
}