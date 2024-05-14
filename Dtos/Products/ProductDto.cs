namespace API.Dtos.Products
{
    public class ProductDto
    {
        public int id { get; set; }
        public string productName { get; set; }
        public int stockIn { get; set; }
        public float price { get; set; }
        public List<string> images { get; set; }
        public List<OptionDto> options { get; set; }
    }
}
