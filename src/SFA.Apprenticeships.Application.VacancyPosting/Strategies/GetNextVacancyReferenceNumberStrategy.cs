namespace SFA.Apprenticeships.Application.VacancyPosting.Strategies
{
    using Domain.Interfaces.Repositories;

    public class GetNextVacancyReferenceNumberStrategy : IGetNextVacancyReferenceNumberStrategy
    {
        private readonly IReferenceNumberRepository _referenceNumberRepository;

        public GetNextVacancyReferenceNumberStrategy(IReferenceNumberRepository referenceNumberRepository)
        {
            _referenceNumberRepository = referenceNumberRepository;
        }

        public int GetNextVacancyReferenceNumber()
        {
            return _referenceNumberRepository.GetNextVacancyReferenceNumber();
        }
    }
}