namespace API.Dtos.User
{
    public class UserForWithdrawRequest
    {
        public int RequestId { get; set; }
        public bool Confirmed { get; set; }
        public string FullName { get; set; }
        public string UserRole { get; set; }
        public int Money { get; set; }
        public string UserName { get; set; }
        public string UserMail { get; set; }
        public string Phone { get; set; }
        public int WalletNumber { get; set; }
        public string Address { get; set; }
        public int TotalProfits { get; set; }
        public int WithdrawnProfits { get; set; }
    }
}
