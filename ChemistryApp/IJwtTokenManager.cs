namespace ChemistryApp;

public interface IJwtTokenManager
{
    Task<string?> Authenticate(string login, string password);
}