using WebAuth.DAL.Models;

namespace WebAuth.BL;

public interface IAuthBL
{
    Task<UserAuthModel?> GetUser(string username);
    Task<UserAuthModel> CreateUser(string username, string password);
    Task Authenticate(string username, string password);
    Task Login(string userId);
    Task Logout();
}