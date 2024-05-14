using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using API.Models;

namespace API.Models
{
    public class Option
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int StockIn { get; set; }
        public float ItemPrice { get; set; }
        public string AvailableOptions { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public Product Product { get; set; }
        public List<ProductOptionCart> ProductOptionCart { get; set; }
    }
}