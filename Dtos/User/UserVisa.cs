using System.ComponentModel.DataAnnotations;

namespace API.Dtos.User
{
    public class UserVisa
    { 
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string ClientName { get; set; }
        public string BankName { get; set; }
        public int AccountNumber { get; set; }
        public string Section { get; set; }
        public int CardNumber { get; set; }
        public string CardName { get; set; }
        public int WalletNumber { get; set; }
    }
}


