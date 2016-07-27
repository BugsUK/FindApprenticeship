namespace SFA.Apprenticeships.Infrastructure.Security
{
    public interface IEncryptionProvider
    {
        string Encrypt(string input);
        string Decrypt(string cipherText);
    }
}