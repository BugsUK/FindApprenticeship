namespace SFA.Apprenticeships.Application.Employer.Strategies
{
    using Domain.Entities.Raa.Parties;

    public interface IGetByIdStrategy
    {
        Employer Get(int employerId);
    }
}