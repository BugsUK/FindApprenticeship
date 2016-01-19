
namespace SFA.Apprenticeships.Data.Migrate
{
    using System;
    using System.Collections.Generic;
    using Avms = SFA.Apprenticeships.Avms.Domain.Entities;

    public interface IAvmsRepository
    {
        /// <summary>
        /// Get all vacancies in an open cursor
        /// </summary>
        /// <returns>An enumerable of the results. This must either be fully iterated or disposed of to avoid a resource leak. Using foreach will automatically dispose.</returns>
        IEnumerable<Avms.Vacancy> GetAllVacancies();
    }
}
