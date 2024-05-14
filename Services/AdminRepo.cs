using API.Dtos.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using API.Models;
using API.Services;

namespace API.Services
{
    public class AdminRepo : IAdminRepo
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;

        public AdminRepo(DataContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<UserForWithdrawRequest>> GetWithdrawnRequests()
        { 
            var result = await _context.WithdrawRequests.Include(obj => obj.User).ThenInclude(o => o.UserRoles).ThenInclude(o => o.Role)
                .Select(obj => new UserForWithdrawRequest()
                {
                    RequestId = obj.Id,
                    Confirmed = obj.Confirmed,
                    FullName = obj.User.FirstName + " " + obj.User.lastName,
                    UserRole = _userManager.GetRolesAsync(obj.User).Result.First(),
                    Money = obj.AmountOfMoney,
                    UserName = obj.User.UserName,
                    UserMail = obj.User.Email,
                    Phone = obj.User.PhoneNumber,
                    WalletNumber = obj.User.WalletNumber,
                    Address = obj.User.address,
                    TotalProfits = obj.User.TotalProfits,
                    WithdrawnProfits = obj.User.WithdrawnProfits
                }).ToListAsync();
            return result;
        }


        public async Task<int> ConfirmWithdrawnRequests(int id)
        {
            var obj = await _context.WithdrawRequests.Include(o => o.User).FirstOrDefaultAsync(w => w.Id == id);
            if (obj == null)
                return 0;
            obj.Confirmed = true;
            obj.DateOfRequest = DateTime.Now;
            obj.User.WithdrawnProfits += obj.AmountOfMoney;
            _context.SaveChanges();
            return 1;
        }

        public async Task<int> CancelConfirmWithdrawnRequest(int id)
        {
            var obj = await _context.WithdrawRequests.Include(o => o.User).FirstOrDefaultAsync(w => w.Id == id);
            if (obj == null)
                return 0;
            obj.Confirmed = false;
            obj.User.WithdrawnProfits -= obj.AmountOfMoney;
            _context.SaveChanges();
            return 1;

        }

        public async Task<int> RejectWithdrawnRequest(int id)
        {
            WithdrawRequest obj = await _context.WithdrawRequests.FirstOrDefaultAsync(w => w.Id == id);
            if (obj == null)
                return 0;
            _context.Remove(obj);
            _context.SaveChanges();
            return 1;

        }
    }
}
