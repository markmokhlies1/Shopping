using API.Models;

namespace API.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string publicId { get; set; } //to get from cloud
        public bool IsMain { get; set; }
        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}