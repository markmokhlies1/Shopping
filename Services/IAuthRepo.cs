namespace API.Services
{
    public interface IAuthRepo
    {
        Task<bool> EmailExists(string email);
        Task<bool> UserNameExists(string userName);
    }
}