namespace API.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    public class Count
    {
        [NotMapped]
        public int NumOfSupplier { get; set; }
        [NotMapped]
        public int NumOfMarkiting { get; set; }
        [NotMapped]
        public int NumOfShipping { get; set; }
        [NotMapped]
        public int NumOfCategory { get; set; }
        [NotMapped]
        public int NumOfProduct { get; set; }
    }
}