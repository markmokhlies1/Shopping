using API.Models;

namespace API.Dtos.Orders
{
    public class OrderDetailsDto
    {
        public DateTime OrderDate { get; set; }
        public DateTime ShippedDate { get; set; }
        public string Status { get; set; }
        public string ClientName { get; set; }
        public int Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public float price { get; set; }
        public int Duration { get; set; }
        public float DealPrice { get; set; }
        public float SiteProfits { get; set; }
        public float ShippingProfits { get; set; }
        public float MarktingProfits { get; set; }
        public ICollection<OrderDetails> OrderDetail { get; set; }
    }
}