namespace VebtechTestTask.Application.Configurations;

public class JwtConfig
{
    public string Issuer { get; init; }
    public string Audience { get; init; }
    public string SymmetricKey { get; init; }
    public int TokenValidityInMinutes { get; init; }
}
