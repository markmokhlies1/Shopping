namespace API.Dtos.Shipping
{
    public class ShippingOrderDetailDto
    {
        public string SuplierName { get; set; }
        public string ProductName { get; set; }
        public string AvailableOptions { get; set; }
        public int Quantity { get; set; }
    }
}
