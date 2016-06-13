namespace SFA.Apprenticeships.Infrastructure.Common.Configuration
{
    public class ServicesConfiguration
    {
        public const string Legacy = "Legacy";
        public const string Raa = "Raa";

        public string ServiceImplementation { get; set; }

        public string VacanciesSource { get; set; }
    }
}