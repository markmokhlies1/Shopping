using System.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int MarketingId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShippedDate { get; set; }
        public int BillId { get; set; }
        public int ShippingId { get; set; }
        public string Status { get; set; }
        public string ClientName { get; set; }
        public int Phone { get; set; }
        public string Address { get; set; }
        public string? shippingPolicy { get; set; }
        [ForeignKey("shippingCompany")]
        public int shippingCompaniesId { get; set; } = 1;

        public List<OrderDetails> OrderDetail { get; set; }
        public Shipping Shipping { get; set; }
        public ShippingCompany shippingCompany { get; set; }
        public Bill Bill { get; set; }
        public User Marketing { get; set; }


    }
}