namespace SFA.Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies
{
    public class Duration
    {
        public Duration(DurationType type, int? length)
        {
            Type = type;
            Length = length;
        }

        public DurationType Type { get; private set; }
        public int? Length { get; private set; }
    }
}