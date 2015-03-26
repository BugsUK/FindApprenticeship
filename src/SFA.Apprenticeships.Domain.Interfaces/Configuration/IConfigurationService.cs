namespace SFA.Apprenticeships.Domain.Interfaces.Configuration
{
    public interface IConfigurationService
    {
        TSettings Get<TSettings>(string settingName) where TSettings : class;
    }
}
