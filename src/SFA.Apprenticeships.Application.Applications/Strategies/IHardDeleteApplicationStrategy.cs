namespace SFA.Apprenticeships.Application.Applications.Strategies
{
    using System;
    using Domain.Entities.Vacancies;

    public interface IHardDeleteApplicationStrategy
    {
        void Delete(VacancyType vacancyType, Guid applicationId);
    }
}