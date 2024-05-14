namespace API.Dtos.Supplier.SupplierUploadProducts
{
    public class ProductDataDto
    {
        public string ProductName { get; set; }
        public float Weight { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string Dimension { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public string[] Codes { get; set; }
        public int[] StockIns { get; set; }
        public float[] ItemPrices { get; set; }
        public string[] AvailableOptions { get; set; }
    }
}
