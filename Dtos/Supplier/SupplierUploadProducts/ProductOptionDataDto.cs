namespace API.Dtos.Supplier.SupplierUploadProducts
{
    public class ProductOptionDataDto
    {
        public string Code { get; set; }
        public string StockIn { get; set; }
        public float ItemPrice { get; set; }
        public string AvailableOptions { get; set; }
        public int ProductId { get; set; }
    }
}