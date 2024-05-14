using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class WithdrawRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AmountOfMoney { get; set; }
        public bool Confirmed { get; set; } = false;
        public DateTime DateOfRequest { get; set; } = DateTime.Now;

        public virtual User User { get; set; }
    }
}
