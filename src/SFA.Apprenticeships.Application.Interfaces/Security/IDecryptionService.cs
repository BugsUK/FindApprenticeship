namespace SFA.Apprenticeships.Application.Interfaces.Security
{
    public interface IDecryptionService<T> where T : class
    {
        T Decrypt(string cipherText);
    }
}