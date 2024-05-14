namespace API.Dtos.Shipping
{
    public class ShippingOrdersStatusDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string ClientName { get; set; }
        public string shippingPolicy { get; set; }
    }
}