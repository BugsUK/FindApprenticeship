namespace SFA.Apprenticeships.Application.Employer.Strategies
{
    using Domain.Entities.Raa.Parties;

    public interface IGetByEdsUrnStrategy
    {
        Employer Get(string edsUrn);
    }
}