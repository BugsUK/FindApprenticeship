
namespace SFA.Apprenticeships.Data.Migrate
{
    using System;
    using System.Collections.Generic;
    using Vacancy = SFA.Apprenticeships.NewDB.Domain.Entities.Vacancy;

    public interface INewDBRepository
    {
        
        int AddVacancy(Vacancy.Vacancy vacancy);
        Vacancy.Vacancy GetVacancy(int id);
    }
}
