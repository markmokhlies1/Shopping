namespace API.Dtos.Orders
{
    public class AddOrderDto
    {
        public float DealPrice { get; set; }
        public float SiteProfits { get; set; }
        public float ShippingProfits { get; set; }
        public float MarketingProfits { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShippedDate { get; set; }
        public int ShippingId { get; set; }
        public string ClientName { get; set; }
        public int Phone { get; set; }
        public string Address { get; set; }
        public AddOrderDto()
        {
            DateTime endDate = DateTime.Now;
            this.OrderDate = DateTime.Now;
        }
    }
}