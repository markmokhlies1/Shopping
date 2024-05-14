using System.ComponentModel;
using API.Models;

namespace API.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public float TotalOptionPrice { get; set; }
        public Order Order { get; set; }
        public int OrderId { get; set; }
        public Option Option { get; set; }
        public int OptionId { get; set; }
        [DefaultValue("false")]
        public bool Seen_Supplier { get; set; }
    }
}