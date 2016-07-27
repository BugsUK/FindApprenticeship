namespace SFA.Apprenticeships.Application.Interfaces.Security
{
    public interface IEncryptionService<T> where T : class
    {
        string Encrypt(T objectToEncrypt);
    }
}
