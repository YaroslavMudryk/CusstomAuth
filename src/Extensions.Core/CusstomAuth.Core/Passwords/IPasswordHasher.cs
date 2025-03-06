namespace CusstomAuth.Core.Passwords;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyHashedPassword(string hashedPassword, string providedPassword);
}

public class DefaultPasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        throw new NotImplementedException();
    }

    public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
        throw new NotImplementedException();
    }
}