using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class AuthRepo : IAuthRepo
    {
        private readonly DataContext _context;
        public AuthRepo(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> EmailExists(string email)
        {
            if (await _context.Users.AnyAsync(u => u.Email == email)) return true;
            return false;
        }

        public async Task<bool> UserNameExists(string userName)
        {
            if (await _context.Users.AnyAsync(i => i.UserName == userName)) return true;
            return false;
        }
    }
}