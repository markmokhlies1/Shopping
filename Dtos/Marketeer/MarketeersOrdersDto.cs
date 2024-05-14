namespace API.Dtos.Marketeer
{
    public class MarketeerOrdersDto
    {
        public int Quantity { get; set; }
        public float TotalOptionPrice { get; set; }
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public string ClientName { get; set; }
        public int Phone { get; set; }
        public string Address { get; set; }
        public string AvailableOptions { get; set; }
        public string ProductName { get; set; }
        public float DealPrice { get; set; }
        public float MarktingProfits { get; set; }
    }
}
