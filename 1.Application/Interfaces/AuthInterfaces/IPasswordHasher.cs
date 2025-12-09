namespace _1.Application.Interfaces.AuthInterfaces;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hash);
}