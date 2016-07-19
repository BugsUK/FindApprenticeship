namespace SFA.Apprenticeships.Application.Employer.Strategies
{
    using Domain.Entities.Raa.Parties;

    //TODO: temporary class. Remove after moving status checks to a higher tier
    public interface IGetByIdWithoutStatusCheckStrategy
    {
        Employer Get(int employerId);
    }
}
