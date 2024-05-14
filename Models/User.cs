using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Identity;

namespace API.Models
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }

        //********** Visa Info**********
        public string ClientName { get; set; }
        public string BankName { get; set; }
        public int AccountNumber { get; set; }
        public string Section { get; set; }
        public int CardNumber { get; set; }
        public string CardName { get; set; }
        public int WalletNumber { get; set; }
        public string lastName { get; set; }
        public string address { get; set; }
        public string ImageID { get; set; } // صوره الرقم القومي
        public int TotalProfits { get; set; } = 0;
        public int WithdrawnProfits { get; set; } = 0;

        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<WithdrawRequest> WithdrawRequests { get; set; }
        public virtual ICollection<UserBill> UserBills { get; set; }
    }
}
