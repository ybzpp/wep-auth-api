using WebAuth.BL;

namespace WebAuth;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/auth")
            .WithOpenApi();
        group.MapPost("/login", Login);
        group.MapPost("/register", Registration);
        group.MapGet("/check", CheckAuth);
        group.MapGet("/logout", Logout);
    }

    public static async Task<IResult> Login(IAuthBL authBl ,string username, string password)
    {
        try
        {
            await authBl.Authenticate(username, password);
            return Results.Ok("Login success");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.BadRequest("Login failed");
        }
    }
    
    public static async Task<IResult> Registration(IAuthBL authBl ,string username, string password)
    {
        try
        {
            await authBl.CreateUser(username, password);
            return Results.Ok("Registration success");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.BadRequest("Registration failed");
        }
    }

    public static async Task<IResult> Logout(IAuthBL authBl)
    {
        try
        {
            await authBl.Logout();
            return Results.Ok("Logout success");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.BadRequest("Logout failed");
        }
    }
    
    public static async Task<IResult> CheckAuth(IHttpContextAccessor httpContextAccessor)
    {
        try
        {
            var isAuth = httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
            return Results.Ok($"isAuth: {isAuth}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.BadRequest("Logout failed");
        }
    }
}