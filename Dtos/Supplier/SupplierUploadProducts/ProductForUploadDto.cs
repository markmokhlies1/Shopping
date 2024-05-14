namespace API.Dtos.Supplier.SupplierUploadProducts
{
    public class ProductForUploadDto
    {
        public string ProductName { get; set; }
        public float Weight { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string Dimension { get; set; }
        public int CategoryId { get; set; }
    }
}