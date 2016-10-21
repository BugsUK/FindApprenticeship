namespace SFA.Apprenticeships.Infrastructure.EmployerDataService.Mappers
{
    using Domain.Entities.Raa.Parties;
    using EmployerDataService;

    public class EmployerMapper
    {
        public VerifiedOrganisationSummary ToVerifiedOrganisationSummary(ConciseEmployerStructure fromEmployer)
        {
            var addressMapper = new BsAddressMapper();

            return new VerifiedOrganisationSummary
            {
                ReferenceNumber = fromEmployer.URN,
                FullName = fromEmployer.Name,
                TradingName = fromEmployer.TradingName,
                PhoneNumber = fromEmployer.Phone,
                EmailAddress = fromEmployer.Email,
                Address = addressMapper.ToAddress(fromEmployer.Address),
                WebSite = fromEmployer.Website
            };
        }

        public VerifiedOrganisationSummary ToVerifiedOrganisationSummary(DetailedEmployerStructure fromEmployer)
        {
            var addressMapper = new BsAddressMapper();

            return new VerifiedOrganisationSummary
            {
                ReferenceNumber = fromEmployer.URN,
                FullName = fromEmployer.Name,
                TradingName = fromEmployer.TradingName,
                PhoneNumber = fromEmployer.Phone,
                EmailAddress = fromEmployer.Email,
                Address = addressMapper.ToAddress(fromEmployer.Address),
                WebSite = fromEmployer.Website
            };
        }
    }
}
