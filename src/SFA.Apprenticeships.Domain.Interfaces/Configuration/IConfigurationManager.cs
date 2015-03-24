namespace SFA.Apprenticeships.Domain.Interfaces.Configuration
{
    //TODO: move to configuration infrastructure as private and expose everything through strongly typed configuration models
    //TODO: using the IConfigurationService. There will be some tricky IoC to sort.
    //TODO: Need to expose Configuration for Wcf service differently too.
    public interface IConfigurationManager
    {
        /// <summary>
        /// Exposing entire configuration file (required by Wcf when using custom factory)
        /// </summary>
        string ConfigurationFilePath { get; }

        string GetAppSetting(string key);

        T GetAppSetting<T>(string key);

        string TryGetAppSetting(string key);
    }
}
