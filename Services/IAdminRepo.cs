using API.Dtos.User;

namespace API.Services
{
    public interface IAdminRepo
    {
        Task<IEnumerable<UserForWithdrawRequest>> GetWithdrawnRequests();
        Task<int> ConfirmWithdrawnRequests(int id);
        Task<int> CancelConfirmWithdrawnRequest(int id);
        Task<int> RejectWithdrawnRequest(int id);
    }
}
