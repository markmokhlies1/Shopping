namespace API.Dtos.User
{
    public class UserProfitsDto
    {
        public int TotalProfits { get; set; }
        public int AvailableProfits { get; set; }
        public int ExpectedProfits { get; set; }
        public bool HasPaymentMethod { get; set; }
        public bool IsRequestAvailable { get; set; } 
        public int RecentTransferred { get; set; } 
    }
}
