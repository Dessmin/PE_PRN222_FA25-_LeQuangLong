using SportsLendDB.BO.Models;

namespace SportsLendDB.BLL.Interfaces
{
    public interface IAuthService
    {
        Task<User?> LoginAsync(string email, string password);
    }
}
