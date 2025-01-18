namespace WebAuth.BL;

public interface IJwtProvider
{
    public string GenerateJwtToken(string value);
}