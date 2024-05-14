namespace API.Dtos.Supplier
{
    public class SupplierOrderDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public float TotalOptionPrice { get; set; }
        public string Status { get; set; }
    }
}
