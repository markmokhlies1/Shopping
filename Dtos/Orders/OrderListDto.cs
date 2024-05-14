namespace API.Dtos.Orders
{
    public class OrderListDto
    {
        public DateTime OrderDate { get; set; }
        public DateTime ShippedDate { get; set; }
        public string Status { get; set; }
        public float MarktingProfits { get; set; }
    }
}