namespace SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers
{
    using AutoMapper;
    using Common.ViewModels.Locations;
    using Domain.Entities.Locations;
    using Domain.Entities.Users;
    using ViewModels;
    using ViewModels.Account;

    public static class SettingsViewModelResolvers
    {
        public class RegistrationDetailsToSettingsViewModelResolver : ITypeConverter<RegistrationDetails, SettingsViewModel>
        {
            public SettingsViewModel Convert(ResolutionContext context)
            {
                var registrationDetails = (RegistrationDetails)context.SourceValue;

                var model = new SettingsViewModel
                {
                    Firstname = registrationDetails.FirstName,
                    Lastname = registrationDetails.LastName,
                    Address = context.Engine.Map<Address, AddressViewModel>(registrationDetails.Address),
                    DateOfBirthOfBirth = new DateOfBirthViewModel
                    {
                        Day = registrationDetails.DateOfBirth.Day,
                        Month = registrationDetails.DateOfBirth.Month,
                        Year = registrationDetails.DateOfBirth.Year
                    },
                    PhoneNumber = registrationDetails.PhoneNumber
                };

                return model;
            }
        }
    }
}
