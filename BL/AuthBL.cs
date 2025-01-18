using Microsoft.EntityFrameworkCore;
using WebAuth.DAL;
using WebAuth.DAL.Models;

namespace WebAuth.BL;

public class AuthBL : IAuthBL
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IJwtProvider _jwtProvider;

    public AuthBL(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor, IJwtProvider jwtProvider)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _jwtProvider = jwtProvider;
    }

    public async Task<UserAuthModel?> GetUser(string username)
    {
        return await _dbContext.AuthUsers.FirstOrDefaultAsync(u => u != null && u.Username == username);
    }
    
    public async Task<UserAuthModel> CreateUser(string username, string password)
    {
        var salt = Guid.NewGuid().ToString();
        var user = new UserAuthModel()
        {
            Username = username,
            Password = Encrypt.HashPassword(password, salt),
            Salt = salt,
        };
        
        _dbContext.AuthUsers.Add(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }

    public async Task Authenticate(string username, string password)
    {
        var user = await _dbContext.AuthUsers.FirstOrDefaultAsync(u => u.Username == username);
        if (user != null && user.Password == Encrypt.HashPassword(password, user.Salt))
        {
            
            System.Console.WriteLine($"Login success");
            Login(user.Id.ToString());
            return; 
        }
        
        throw new Exception("Invalid username or password");
    }

    public async Task Login(string userId)
    {
        var token = _jwtProvider.GenerateJwtToken(userId);
        _httpContextAccessor.HttpContext.Response.Cookies.Append(AuthConstants.AUTH_SESSION_PARAM_NAME, token);
    }

    public Task Logout()
    {
        _httpContextAccessor.HttpContext.Response.Cookies.Delete(AuthConstants.AUTH_SESSION_PARAM_NAME);
        return Task.CompletedTask;
    }
}