namespace SFA.Apprenticeships.Application.Interfaces
{
    public interface IConfigurationService
    {
        TSettings Get<TSettings>() where TSettings : class;
    }
}
