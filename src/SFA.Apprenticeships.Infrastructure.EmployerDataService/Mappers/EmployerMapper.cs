namespace SFA.Apprenticeships.Infrastructure.EmployerDataService.Mappers
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Raa.Parties;
    using EmployerDataService;

    public class EmployerMapper
    {
        public VerifiedOrganisationSummary ToVerifiedOrganisationSummary(
            ConciseEmployerStructure fromEmployer,
            IEnumerable<string> referenceNumberAliases)
        {
            var addressMapper = new BsAddressMapper();

            return new VerifiedOrganisationSummary
            {
                ReferenceNumber = fromEmployer.URN,
                Name = fromEmployer.Name,
                TradingName = fromEmployer.TradingName,
                PhoneNumber = fromEmployer.Phone,
                EmailAddress = fromEmployer.Email,
                Address = addressMapper.ToAddress(fromEmployer.Address),
                WebSite = fromEmployer.Website,                
                ReferenceNumberAliases = referenceNumberAliases.ToArray()
            };
        }
    }
}
