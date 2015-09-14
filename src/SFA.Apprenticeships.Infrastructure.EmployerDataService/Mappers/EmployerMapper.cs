namespace SFA.Apprenticeships.Infrastructure.EmployerDataService.Mappers
{
    using Domain.Entities.Organisations;
    using EmployerDataService;

    public class EmployerMapper
    {
        public Organisation ToVerifiedOrganisation(ConciseEmployerStructure fromEmployer)
        {
            var addressMapper = new BsAddressMapper();

            return new Organisation
            {
                ReferenceNumber = fromEmployer.URN,
                Name = fromEmployer.Name,
                TradingName = fromEmployer.TradingName,
                PhoneNumber = fromEmployer.Phone,
                EmailAddress = fromEmployer.Email,
                Address = addressMapper.ToAddress(fromEmployer.Address)
            };
        }
    }
}
