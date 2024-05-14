using System.Collections.Generic;
using API.Models;

namespace API.Models
{
    public class Shipping
    {
        public int Id { get; set; }
        public string City { get; set; }
        public float price { get; set; }
        public int Duration { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}