namespace API.Models
{
    public class UserBill
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BillId { get; set; }
        public float UserProfit { get; set; }
        public bool Active { get; set; }

        public User User { get; set; }
        public Bill Bill { get; set; }
    }
}