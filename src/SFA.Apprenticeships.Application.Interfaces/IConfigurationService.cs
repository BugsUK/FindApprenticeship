namespace SFA.Apprenticeships.Application.Interfaces
{
    using System;

    public interface IConfigurationService
    {
        TSettings Get<TSettings>() where TSettings : class;
        object Get(Type settingsType);
    }
}
