namespace ChemistryApp;

public interface IJwtTokenManager
{
    string? Authenticate(string login, string password);
}