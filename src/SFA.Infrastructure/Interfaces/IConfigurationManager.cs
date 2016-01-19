namespace SFA.Infrastructure.Interfaces
{
    public interface IConfigurationManager
    {
        /// <summary>
        /// Exposing entire configuration file (required by Wcf when using custom factory) TODO: Remove!
        /// </summary>
        string ConfigurationFilePath { get; }

        string GetAppSetting(string key);

        T GetAppSetting<T>(string key);

        string TryGetAppSetting(string key);
    }
}
